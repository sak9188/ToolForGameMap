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
using System.Windows.Forms;

using Winform = System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WpfApplicationTest1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string pattern = "^[0-9](.|.[0-9])?$";
        private string lastScaleValue = "";


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Search_Img(object sender, RoutedEventArgs e)
        {
            using (Winform.OpenFileDialog openFileDialog = new Winform.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                openFileDialog.Filter = "jpg文件 (*.jpg)|*.jpg|png文件 (*.png)|*.png";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == Winform.DialogResult.OK)
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri(openFileDialog.FileName, UriKind.Absolute);
                    bi.EndInit();
                    BackgroundImage.Source = bi;
                    TBoxImgPath.Text = openFileDialog.FileName;
                }
            }
        }

        private void TBoxScale_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            string input = TBoxScale.Text;
            if (input == "")
            {
                lastScaleValue = "";
                return;
            }
            if (!Regex.IsMatch(input, pattern))
            {
                TBoxScale.Text = lastScaleValue;
                TBoxScale.SelectionStart = lastScaleValue.Length;
                return;
            }
            lastScaleValue = input;

        }

        private void Print(string str)
        {
            Console.WriteLine(str);
        }
    }
}
