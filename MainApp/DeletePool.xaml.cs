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
using System.Windows.Shapes;


namespace MainApp
{
    /// <summary>
    /// Interaction logic for DeletePool.xaml
    /// </summary>
    public partial class DeletePool : Window
    {
        public DeletePool()
        {
            InitializeComponent();

            Spaces pool = new Spaces();
            var listOfPools = pool.GetListOfAvailablePools();

            if (listOfPools.Count > 0)
            {
                foreach (string element in listOfPools)
                {
                    PoolSelection.Items.Add(element);
                }
            }

            var listOfDisks = pool.GetListOfLogicalDisks();

            if (listOfDisks.Count > 0)
            {
                foreach (string element in listOfDisks)
                {
                    LogicalDiskSelect.Items.Add(element);
                }
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

        private void Delete_Pool(object sender, RoutedEventArgs e)
        {

            Spaces pool = new Spaces();
            bool result = pool.DeleteStoragePool(PoolSelection.SelectedItem.ToString());
            if (result)
            {
                MessageBoxResult message = MessageBox.Show("Pool Successfully deleted");
            }
            else
            {
                MessageBoxResult message = MessageBox.Show("There is no pool to delete");
            }

            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Spaces obj = new Spaces();
            bool result = obj.DeleteLogicalDisk(LogicalDiskSelect.SelectedItem.ToString());
            
            if (result)
            {
                MessageBoxResult message = MessageBox.Show("Logical Disk Successfully deleted");
            }
            else
            {
                MessageBoxResult message = MessageBox.Show("There is no disk to delete");
            }

            this.Close();
        }

        
    }
}
