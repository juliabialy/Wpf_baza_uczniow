using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace WpfApp1
{
    public partial class Window1 : Window
    {
        public MyItem? EditingItem { get; set; }
        public event Action<MyItem>? DataSubmit;
        private bool isEditMode;

        public Window1(MyItem? item = null)
        {
            InitializeComponent();
            if (item != null)
            {
                isEditMode = true;
                EditingItem = item;
                FillForm(item);
            }
        }

        private void FillForm(MyItem item)
        {
            PeselTextBox.Text = item.Pesel;
            NameTextBox.Text = item.FirstName;
            MiddleNameTextBox.Text = item.MiddleName;
            SurnameTextBox.Text = item.LastName;
            BirthDatePicker.SelectedDate = item.BirthDate;
            PhoneTextBox.Text = item.PhoneNumber;
            AddressTextBox.Text = item.Address;
            CityTextBox.Text = item.City;
            PostalCodeTextBox.Text = item.PostalCode;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            var item = new MyItem
            {
                Pesel = PeselTextBox.Text,
                FirstName = DataValidator.FormatText(NameTextBox.Text),
                MiddleName = DataValidator.FormatText(MiddleNameTextBox.Text),
                LastName = DataValidator.FormatText(SurnameTextBox.Text),
                BirthDate = BirthDatePicker.SelectedDate!.Value,
                PhoneNumber = DataValidator.FormatPhoneNumber(PhoneTextBox.Text),
                Address = DataValidator.FormatText(AddressTextBox.Text),
                City = DataValidator.FormatText(CityTextBox.Text),
                PostalCode = PostalCodeTextBox.Text
            };

            DataSubmit?.Invoke(item);
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (HasChanges())
            {
                var result = MessageBox.Show("Czy na pewno chcesz zamknąć okno bez zapisywania zmian?", "Potwierdzenie", MessageBoxButton.YesNo);
                if (result != MessageBoxResult.Yes)
                    return;
            }

            DialogResult = false;
            Close();
        }

        private bool ValidateInputs()
        {
            bool isValid = true;

            isValid &= ValidateRequiredTextBox(PeselTextBox, 11, null, (tb) =>
                DataValidator.IsValidPesel(tb.Text, BirthDatePicker.SelectedDate));

            isValid &= ValidateRequiredTextBox(NameTextBox);
            isValid &= ValidateRequiredTextBox(SurnameTextBox);
            isValid &= ValidateRequiredTextBox(AddressTextBox);
            isValid &= ValidateRequiredTextBox(CityTextBox);
            isValid &= ValidateRequiredTextBox(PostalCodeTextBox, regex: @"^\d{2}-\d{3}$");

            if (BirthDatePicker.SelectedDate == null)
            {
                BirthDatePicker.BorderBrush = Brushes.Red;
                isValid = false;
            }
            else
            {
                BirthDatePicker.ClearValue(BorderBrushProperty);
            }

            return isValid;
        }

        private bool ValidateRequiredTextBox(TextBox textBox, int? exactLength = null, string? regex = null, Func<TextBox, bool>? extraValidator = null)
        {
            bool isValid = !string.IsNullOrWhiteSpace(textBox.Text);

            if (exactLength.HasValue)
                isValid &= textBox.Text.Length == exactLength.Value;

            if (regex != null)
                isValid &= Regex.IsMatch(textBox.Text, regex);

            if (extraValidator != null)
                isValid &= extraValidator.Invoke(textBox);

            textBox.BorderBrush = isValid ? Brushes.Gray : Brushes.Red;
            return isValid;
        }

        private bool HasChanges()
        {
            return !string.IsNullOrWhiteSpace(PeselTextBox.Text)
                || !string.IsNullOrWhiteSpace(NameTextBox.Text)
                || !string.IsNullOrWhiteSpace(SurnameTextBox.Text)
                || BirthDatePicker.SelectedDate != null
                || !string.IsNullOrWhiteSpace(AddressTextBox.Text)
                || !string.IsNullOrWhiteSpace(CityTextBox.Text)
                || !string.IsNullOrWhiteSpace(PostalCodeTextBox.Text);
        }
    }
}
