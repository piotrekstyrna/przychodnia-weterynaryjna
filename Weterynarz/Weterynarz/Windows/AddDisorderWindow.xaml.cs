using System.Windows;
using Weterynarz.Classes;
using Weterynarz.Entities;

namespace Weterynarz.Windows
{
    public partial class AddDisorderWindow : Window
    {
        public Disorder NewDisorder { get; private set; }

        public AddDisorderWindow()
        {
            InitializeComponent();
        }

        private async void BtnAddNewDisorder_Click(object sender, RoutedEventArgs e)
        {
            string name = TbxName.Text;
            bool isHealable = CkbxIsHealable.IsChecked.Value;
            string medicine = TbxMedicine.Text;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Niepoprawna nazwa.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newDisorder = new Disorder
            {
                Name = name,
                IsHealable = isHealable,
                Medicine = medicine,
            };

            NewDisorder = newDisorder;

            BtnAddNewDisorder.IsEnabled = false;
            StaticContext.Context.Disorders.Add(newDisorder);
            await StaticContext.Context.SaveChangesAsync();
            BtnAddNewDisorder.IsEnabled = true;

            DialogResult = true;
        }
    }
}