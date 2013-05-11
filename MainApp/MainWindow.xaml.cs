using System;
using System.Collections;
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
        ArrayList disks = new ArrayList();

        private void Button_CreateStoragePool(object sender, RoutedEventArgs e)
        {
            Spaces pool = new Spaces();
            disks = pool.GetDisks();
            bool result = pool.CreatePool(disks, "my_storage");
            if (result)
            {
                MessageBoxResult message = MessageBox.Show("Pool Successfully created");
            }
            else
            {
                MessageBoxResult message = MessageBox.Show("Something went wrong!!!");
            }
        }

        private void Button_DeleteStoragePool(object sender, RoutedEventArgs e)
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

        private void Button_AddLogicalDisc(object sender, RoutedEventArgs e)
        {
            Spaces pool = new Spaces();


            bool result = pool.CreateVirtualDisk("my_storage", "Test Disk", disks);
             
            if (result)
            {
                MessageBoxResult message = MessageBox.Show("Disk successfully added");
            }
            else
            {
                MessageBoxResult message = MessageBox.Show("Something went wrong!!!");
            }
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove(); 
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Minimalize(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
    }
}
