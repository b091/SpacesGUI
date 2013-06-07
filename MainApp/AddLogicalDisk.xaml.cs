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
    /// Interaction logic for AddLogicalDisk.xaml
    /// </summary>
    public partial class AddLogicalDisk : Window
    {
        public AddLogicalDisk()
        {
            InitializeComponent();
            ResilienceType.Items.Add("simple");
            ResilienceType.Items.Add("mirror");
            ResilienceType.Items.Add("parity");
            Spaces pool = new Spaces();
            var listOfPools = pool.GetListOfAvailablePools();

            if (listOfPools.Count > 0)
            {
                foreach (string element in listOfPools)
                {
                    PoolSelection2.Items.Add(element);
                }
            }

            List<char> availableLetters = pool.GetListOfAvailableLetters();
            if (availableLetters.Count > 0)
            {
                foreach (char letter in availableLetters)
                {
                    DriveLetter.Items.Add(letter);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Spaces pool = new Spaces();


            bool result = pool.CreateVirtualDisk(PoolSelection2.SelectedItem.ToString(), DiskName.Text, ResilienceType.SelectedItem.ToString(), DriveLetter.SelectedItem.ToString());

            if (result)
            {
                MessageBoxResult message = MessageBox.Show("Disk successfully added");
            }
            else
            {
                MessageBoxResult message = MessageBox.Show("Something went wrong!!!");
            }
            this.Close();
        }

    }
}
