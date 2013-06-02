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
    public partial class ConfigureStoragePool : Window
    {
        public ConfigureStoragePool()
        {
            InitializeComponent();
        }
        ArrayList disks = new ArrayList();

        string poolName = "";

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

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
        
            Spaces pool = new Spaces();
            disks = pool.GetDisks();
            bool result = pool.CreatePool(disks, poolName);
            if (result)
            {
                MessageBoxResult message = MessageBox.Show("Pool Successfully created");
            }
            else
            {
                MessageBoxResult message = MessageBox.Show("Something went wrong!!!");
            }

            this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            poolName = TextBox1.Text;
        }
    }
}
