using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    /// 
    class MyItem
    {
        public string Name { get; set; }

        public string Sname { get; set; }

        public string PESEL { get; set; }
    }
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {


            MyItem item = new MyItem();
            item.Name = "Julia";
            item.Sname = "Biały";
            item.PESEL = "12345678910";



        }
    }
}
