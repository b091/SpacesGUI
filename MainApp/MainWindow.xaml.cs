using System;
using System.Collections.Generic;
using System.Linq;
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


namespace MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Spaces pool = new Spaces();
            bool result = pool.CreatePool(pool.GetDisks(), "my_storage");
            if (result)
            {
                MessageBoxResult message = MessageBox.Show("Pool Successfully created");
            }
            else
            {
                MessageBoxResult message = MessageBox.Show("Something went wrong!!!");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Spaces pool = new Spaces();
            bool result = pool.DeleteStoragePool("my_storage");
            if (result)
            {
                MessageBoxResult message = MessageBox.Show("Pool Successfully deleted");
            }
            else
            {
                MessageBoxResult message = MessageBox.Show("There is no pool to delete");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Spaces pool = new Spaces();

            bool result = pool.CreateVirtualDisk("my_storage", "Test Disk", pool.GetDisks());
             
            if (result)
            {
                MessageBoxResult message = MessageBox.Show("Disk successfully added");
            }
            else
            {
                MessageBoxResult message = MessageBox.Show("Something went wrong!!!");
            }
        }
    }
}
