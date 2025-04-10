using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<MyItem> Items { get;set }
        public MainWindow()
        {
            InitializeComponent();
            items = new List<MyItem>();
            listView.ItemsSource = items;
        }



        private void button_Click(object sender, RoutedEventArgs e)
        {
            Window1 myWindow = new Window1();
            myWindow.DataSubmit += AddItemToList;
            myWindow.Show();
        }

        private void AddItemToList(MyItem item)
        {
            items.Add(item);
            listView.Items.Refresh();
        }

    }
}