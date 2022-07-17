using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Weterynarz.Classes;
using Weterynarz.Entities;
using Weterynarz.Windows;

namespace Weterynarz
{
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<Owner> _owners = new ObservableCollection<Owner>();
        private readonly ObservableCollection<Disorder> _disorders = new ObservableCollection<Disorder>();
        private readonly ObservableCollection<Visit> _visits = new ObservableCollection<Visit>();

        public MainWindow()
        {
            InitializeComponent();

            DGOwners.ItemsSource = _owners;
            DGDisorders.ItemsSource = _disorders;
            DGVisits.ItemsSource = _visits;
        }

        // Załadowanie danych z bazy na starcie aplikacji (na załadowanie głównego okna)
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var owners = await StaticContext.Context.Owners.ToListAsync();
            
            foreach (var owner in owners)
            {
                _owners.Add(owner);
            }

            var disorders = await StaticContext.Context.Disorders.ToListAsync();

            foreach (var disorser in disorders)
            {
                _disorders.Add(disorser);
            }

            var visits = await StaticContext.Context.Visits.ToListAsync();

            foreach (var visit in visits)
            {
                _visits.Add(visit);
            }
        }

        private void BtnAddNewOwner_Click(object sender, RoutedEventArgs e)
        {
            AddNewItem<AddOwnerWindow, Owner>(w => w.NewOwner, _owners);
        }

        private async void BtnDeleteOwner_Click(object sender, RoutedEventArgs e)
        {
            var deleteOwnerDialogResult = MessageBox.Show("Czy chcesz usunąć właściciela i wszystkie powiązane z nim wizyty?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (deleteOwnerDialogResult == MessageBoxResult.Yes)
            {
                await DeleteItem(sender, BtnAddNewOwner, _owners);
            }
        }

        private void BtnAddNewDisorder_Click(object sender, RoutedEventArgs e)
        {
            AddNewItem<AddDisorderWindow, Disorder>(w => w.NewDisorder, _disorders);
        }

        private async void BtnDeleteDisorder_Click(object sender, RoutedEventArgs e)
        {
            var deleteDisorderDialogResult = MessageBox.Show("Czy chcesz usunąć dolegliwość i wszystkie powiązane z nią wizyty?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (deleteDisorderDialogResult == MessageBoxResult.Yes)
            {
                await DeleteItem(sender, BtnAddNewDisorder, _disorders);
            }
        }

        private void BtnAddNewVisit_Click(object sender, RoutedEventArgs e)
        {
            if (!_owners.Any())
            {
                MessageBox.Show("Nie można dodać wizyty, ponieważ w bazie nie ma żadnego właściciela.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!_disorders.Any())
            {
                MessageBox.Show("Nie można dodać wizyty, ponieważ w bazie nie ma żadnej dolegliwości.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AddNewItem<AddVisitWindow, Visit>(w => w.NewVisit, _visits, w =>
            {
                foreach (var owner in _owners)
                {
                    w.InitialOwners.Add(owner);
                }

                foreach (var disorder in _disorders)
                {
                    w.InitialDisorders.Add(disorder);
                }
            });
        }

        private async void BtnDeleteVisit_Click(object sender, RoutedEventArgs e)
        {
            await DeleteItem(sender, BtnAddNewVisit, _visits);
        }

        /// <summary>
        /// Generyczna metoda (ponieważ kod powtarza się dla wszystkich 3 przypadków dodawania danych
        /// </summary>
        /// <typeparam name="TWindow">Typ otwieranego okna</typeparam>
        /// <typeparam name="TItem">Typ encji z którą powiązane jest okno</typeparam>
        /// <param name="newItemFunc">Funkcja wskazująca właściwość konkretnego okna, która przechowuje nowo utworzoną encją</param>
        /// <param name="gridCollection">Grid na encje w głównym oknie</param>
        /// <param name="initialization">Delegat inicjalizujący tworzone okno (w przypadku wizyt, trzeba mu na starcie przekazać dane)</param>
        private void AddNewItem<TWindow, TItem>(Func<TWindow, TItem> newItemFunc,
                                                ObservableCollection<TItem> gridCollection,
                                                Action<TWindow> initialization = null) where TWindow : Window, new()
        {
            var window = new TWindow
            {
                Owner = this
            };

            initialization?.Invoke(window);

            bool? dialogResult = window.ShowDialog();

            if (dialogResult.HasValue && dialogResult.Value)
            {
                var newItem = newItemFunc(window);

                gridCollection.Add(newItem);
            }
        }

        // Obsługa zdarzenia usuwania dla wszystkich 3 gridów (metoda generyczna)
        private async Task DeleteItem<TItem>(object clickedRowSender, Button deleteButton, ObservableCollection<TItem> gridCollection) where TItem : class
        {
            var itemToDelete = ((FrameworkElement)clickedRowSender).DataContext as TItem;

            if (typeof(TItem) == typeof(Owner))
            {
                var ownerVisits = await StaticContext.Context.Visits.Where(v => v.Owner == itemToDelete).ToListAsync();
                StaticContext.Context.Visits.RemoveRange(ownerVisits);

                foreach (var visit in ownerVisits)
                {
                    _visits.Remove(visit);
                }
            }
            else if (typeof(TItem) == typeof(Disorder))
            {
                var disorderVisits = await StaticContext.Context.Visits.Where(v => v.Disorder == itemToDelete).ToListAsync();
                StaticContext.Context.Visits.RemoveRange(disorderVisits);

                foreach (var visit in disorderVisits)
                {
                    _visits.Remove(visit);
                }
            }

            deleteButton.IsEnabled = false;
            StaticContext.Context.Set<TItem>().Remove(itemToDelete);
            await StaticContext.Context.SaveChangesAsync();
            deleteButton.IsEnabled = true;

            gridCollection.Remove(itemToDelete);
        }
    }
}