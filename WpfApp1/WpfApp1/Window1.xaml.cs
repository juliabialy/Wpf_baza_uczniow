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
            if (!DataValidator.IsValidPesel(peselTextBox.Text, birthDate))
            {
                MessageBox.Show("Nieprawidłowy PESEL lub data", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            MyItem item = new MyItem
            {
                Name = (nameTextBox.Text),
                Sname = snameTextBox.Text,
                PESEL = (peselTextBox.Text).Trim(),
                ScName = scnameTextBox.Text,
                HomeAdress = homeadressTextBox.Text,
                PhoneNumber = DataValidator.FormatPhoneNumber(PhoneNumber.Text)

                //MessageBox.Show(wnd.dataurodzneia.totring().splik()[0]);

            };

            DataSubmit?.Invoke(item);
            this.Close();


        }


    }
}