using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Weterynarz.Classes;
using Weterynarz.Entities;

namespace Weterynarz.Windows
{
    public partial class AddVisitWindow : Window
    {
        public Visit NewVisit { get; private set; }

        public ObservableCollection<Owner> InitialOwners { get; } = new ObservableCollection<Owner>();
        public ObservableCollection<Disorder> InitialDisorders { get; } = new ObservableCollection<Disorder>();

        public AddVisitWindow()
        {
            InitializeComponent();

            DtpcrDate.SelectedDate = DateTime.Now;

            CbxOwner.ItemsSource = InitialOwners;
            CbxDisorder.ItemsSource = InitialDisorders;
        }

        private async void BtnAddNewVisit_Click(object sender, RoutedEventArgs e)
        {
            var date = DtpcrDate.SelectedDate;
            string timeMinutesString = TbxTime.Text;
            string priceString = TbxPrice.Text;
            var selectedOwner = (Owner)CbxOwner.SelectedItem;
            var selectedDisorder = (Disorder)CbxDisorder.SelectedItem;

            var currentDate = DateTime.Now;

            bool isDateLaterThanNow = date > currentDate;

            if (isDateLaterThanNow)
            {
                MessageBox.Show("Nie można dodać wizyty z przyszłą datą.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(timeMinutesString))
            {
                MessageBox.Show("Niepoprawna ilość minut wizyty.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(timeMinutesString, out int timeMinutes))
            {
                MessageBox.Show("Niepoprawna ilość minut wizyty.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(priceString))
            {
                MessageBox.Show("Niepoprawna cena.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(priceString, out int price))
            {
                MessageBox.Show("Niepoprawna cena.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedOwner == null)
            {
                MessageBox.Show("Należy wybrać właściciela.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedDisorder == null)
            {
                MessageBox.Show("Należy wybrać dolegliwość.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newVisit = new Visit
            {
                Date = date.Value,
                Time = new TimeSpan(0, timeMinutes, 0),
                Price = price,
                OwnerId = selectedOwner.Id,
                DisorderId = selectedDisorder.Id
            };

            NewVisit = newVisit;

            BtnAddNewVisit.IsEnabled = false;
            StaticContext.Context.Visits.Add(newVisit);
            await StaticContext.Context.SaveChangesAsync();
            BtnAddNewVisit.IsEnabled = true;

            DialogResult = true;
        }

        private void CbxOwner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbxOwner.SelectedItem == null)
            {
                return;
            }

            var selectedOwner = (Owner)CbxOwner.SelectedItem;

            TbxAnimalName.Text = selectedOwner.Animal.Name;
        }
    }
}