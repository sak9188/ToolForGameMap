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
                    ScaleBackgroundImage(1);
                    TBoxImgPath.Text = openFileDialog.FileName;
                }
            }
        }


        private string lastScaleValue = "";
        private const string patternTBoxScale = "^[0-9](.|.[0-9])?$";
        private void TBoxScale_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = TBoxScale.Text;
            if (input == "")
            {
                lastScaleValue = "";
                return;
            }
            if (!Regex.IsMatch(input, patternTBoxScale))
            {
                TBoxScale.Text = lastScaleValue;
                TBoxScale.SelectionStart = lastScaleValue.Length;
                return;
            }
            // 保存上一个值
            lastScaleValue = input;
            double dValue = Convert.ToDouble(lastScaleValue);
            // 数字合理性判断
            if (dValue > 10) dValue = 10;
            else if (dValue <= 0) dValue = 1;
            ScaleBackgroundImage(dValue);
        }

        private string lastValueTBoxCell = "";
        private const string patternTBoxCell = "^[0-9]{0,3}$";
        private void TBoxCell_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = TBoxCell.Text;
            if (input == "")
            {
                lastValueTBoxCell = "";
                return;
            }
            if (!Regex.IsMatch(input, patternTBoxCell))
            {
                TBoxCell.Text = lastValueTBoxCell;
                TBoxCell.SelectionStart = lastValueTBoxCell.Length;
                return;
            }
            // 保存上一个值
            lastValueTBoxCell = input;
        }
        
        private int cellLen = 0;
        private void Button_Click_Generate_Cell(object sender, RoutedEventArgs e)
        {
            if (lastValueTBoxCell == "") return;
            int iValue = Convert.ToInt32(lastValueTBoxCell);
            // 数字合理性判断
            if (iValue > 100) iValue = 100;
            else if (iValue <= 0) iValue = 1;
            cellLen = iValue;
            // 生成按钮
            GenerateButton();
        }  

        private void Print(string str)
        {
            Console.WriteLine(str);
        }

        private void BackgroundImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ImageCanvas.Width = BackgroundImage.Width;
            ImageCanvas.Height = BackgroundImage.Height;
        }
     
    }
}
