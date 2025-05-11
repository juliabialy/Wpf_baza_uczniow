using System;
using System.Windows;


namespace WpfApp1
{
    public class MyItem
    {
        public string Name { get; set; } = string.Empty;
        public string Sname { get; set; } = string.Empty;
        public string PESEL { get; set; } = string.Empty;
        public string ScName { get; set; } = string.Empty;
        public string HomeAdress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public partial class Window1 : Window
    {
        public event Action<MyItem> DataSubmit;

        public Window1()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var birthDate = BirthDatePicker.SelectedDate;

            if (birthDate == null)
            {
                MessageBox.Show("Wybierz datę urodzenia", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string pesel = peselTextBox.Text.Trim();

            if (!DataValidator.IsValidPesel(pesel, birthDate))
            {
                MessageBox.Show("Nieprawidłowy PESEL lub data urodzenia", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MyItem item = new MyItem
            {
                Name = DataValidator.FormatText(nameTextBox.Text),
                Sname = DataValidator.FormatText(snameTextBox.Text),
                PESEL = pesel,
                ScName = DataValidator.FormatText(scnameTextBox.Text),
                HomeAdress = DataValidator.FormatText(homeadressTextBox.Text),
                PhoneNumber = DataValidator.FormatPhoneNumber(PhoneNumber.Text)
            };

            DataSubmit?.Invoke(item);
            this.Close();
        }



    }
}