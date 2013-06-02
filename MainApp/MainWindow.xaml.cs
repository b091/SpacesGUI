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
            var newWindow = new ConfigureStoragePool();
            newWindow.Show();
        }

        private void Button_DeleteStoragePool(object sender, RoutedEventArgs e)
        {
            var deleteWindow = new DeletePool();
            deleteWindow.Show();
        }

        private void Button_AddLogicalDisc(object sender, RoutedEventArgs e)
        {
            var addDiskWindow = new AddLogicalDisk();
            addDiskWindow.Show();
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
