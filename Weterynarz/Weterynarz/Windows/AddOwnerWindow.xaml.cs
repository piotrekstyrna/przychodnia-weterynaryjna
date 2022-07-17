using System;
using System.Windows;
using Weterynarz.Classes;
using Weterynarz.Entities;

namespace Weterynarz.Windows
{
    public partial class AddOwnerWindow : Window
    {
        public Owner NewOwner { get; private set; }

        public AddOwnerWindow()
        {
            InitializeComponent();

            DtpcrAnimalBirthDate.SelectedDate = DateTime.Now;
            CbxAnimalGender.SelectedIndex = 0;
        }

        private async void BtnAddNewOwner_Click(object sender, RoutedEventArgs e)
        {
            string newOwnerName = TbxOwnerName.Text;
            string newOwnerSurname = TbxOwnerSurname.Text;
            string newOwnerPhoneNumber = TbxOwnerPhoneNumber.Text;
            string animalName = TbxAnimalName.Text;
            string animalGender = CbxAnimalGender.SelectedValue.ToString().Contains("Samiec") ? "Samiec" : "Samica";
            var animalBirthDate = DtpcrAnimalBirthDate.SelectedDate;
            string animalSpecie = TbxAnimalSpecie.Text;
            string animalRace = TbxAnimalRace.Text;

            if (string.IsNullOrEmpty(newOwnerName))
            {
                MessageBox.Show("Niepoprawne imię właściciela.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(newOwnerSurname))
            {
                MessageBox.Show("Niepoprawne nazwisko właściciela.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(newOwnerPhoneNumber))
            {
                MessageBox.Show("Niepoprawny numer telefonu właściciela.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(animalName))
            {
                MessageBox.Show("Niepoprawna nazwa zwierzątka.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(animalGender))
            {
                MessageBox.Show("Niepoprawna płeć zwierzątka.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var currentDate = DateTime.Now;

            bool isAnimalBirthDateLaterThanNow = animalBirthDate.Value > currentDate;

            if (isAnimalBirthDateLaterThanNow)
            {
                MessageBox.Show("Data urodzenia zwierzątka nie może być późniejsza niż obecna data.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(animalSpecie))
            {
                MessageBox.Show("Niepoprawny gatunek zwierzątka.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(animalRace))
            {
                MessageBox.Show("Niepoprawna rasa zwierzątka.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newOwner = new Owner
            {
                Name = newOwnerName,
                Surname = newOwnerSurname,
                PhoneNumber = newOwnerPhoneNumber,
                Animal = new Animal
                {
                    Name = animalName,
                    Gender = animalGender,
                    BirthDate = animalBirthDate.Value,
                    Specie = animalSpecie,
                    Race = animalRace
                }
            };

            NewOwner = newOwner;

            BtnAddNewOwner.IsEnabled = false;
            StaticContext.Context.Owners.Add(newOwner);
            await StaticContext.Context.SaveChangesAsync();
            BtnAddNewOwner.IsEnabled = true;

            DialogResult = true;
        }
    }
}