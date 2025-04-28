using System.Collections.Generic;
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public List<MyItem> Items { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Items = new List<MyItem>();
            listView.ItemsSource = Items;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Window1 myWindow = new Window1();
            myWindow.DataSubmit += AddItemToList;
            myWindow.Show();
        }

        private void AddItemToList(MyItem item)
        {
            if (DataValidator.IsValidPesel(item.PESEL))
            {
                Items.Add(item);
                listView.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Nieprawidłowy PESEL");
            }
        }
    }
}