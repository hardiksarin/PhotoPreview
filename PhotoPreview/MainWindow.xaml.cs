using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoPreview
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
        private void BtnLoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                imgDynamic.Source = new BitmapImage(fileUri);
                imgDynamic2.Source = new BitmapImage(fileUri);

            }
        }
        private void BtnLoadFromResource_Click(object sender, RoutedEventArgs e)
        {
            Uri resourceUri = new Uri("C:\\Users\\sarin\\Documents\\Scented Candels\\FrontLogo.png", UriKind.Absolute);
            imgDynamic.Source = new BitmapImage(resourceUri);
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            MessageBox.Show(list1.SelectedItem.ToString());
        }
    }
}
