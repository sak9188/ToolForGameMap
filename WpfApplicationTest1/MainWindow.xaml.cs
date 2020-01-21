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
using WpfApplicationTest1.Entity;
using WpfApplicationTest1.Help;
using WpfApplicationTest1.ToolControls;

namespace WpfApplicationTest1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private int buttonStartIndex = 2;

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
                    // 重置图片大小框
                    ResetTBoxImage();
                    // ToolPanel
                    ToolPanel.Visibility = System.Windows.Visibility.Visible;
                    RadioSingle.IsChecked = true; 
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
            if(isChangedMap == 1)
            {
                if(MessageBoxResult.Cancel == System.Windows.MessageBox.Show("你确定要重新生成吗？数据没了别怪自己手贱", "警告", MessageBoxButton.OKCancel))
                {
                    return;
                }
            }
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

        private string fileDirectory;
        private void Import_Cell_List_Button_Click(object sender, RoutedEventArgs e)
        {
            using (Winform.OpenFileDialog openFileDialog = new Winform.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                openFileDialog.Filter = "json文件 (*.json)|*.json|所有文件 (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == Winform.DialogResult.OK)
                {   
                    GenerateCellList(openFileDialog.FileName);
                    fileDirectory = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                }
            }
        }

        private void Export_Cell_List_Button_Click(object sender, RoutedEventArgs e)
        {
            if (fileDirectory == null) return;
            if (!Regex.IsMatch(json.name, outputPattern))
            {
                System.Windows.MessageBox.Show("你的list的name不要加各种垃圾符号");
                return;
            }
            JsonHelp.ListToFile(json, fileDirectory);
        }

        private string priviousName;
        private void Button_Click_Add_Modify(object sender, RoutedEventArgs e)
        {
            if (json == null) return;
            int index = CellListBox.SelectedIndex;
            if (priviousName != TBoxName.Text)
            {
                // 添加
                Cell cell = new Cell()
                {
                    name = TBoxName.Text,
                    avo = Convert.ToInt32(TBoxAVO.Text),
                    def = Convert.ToInt32(TBoxDEF.Text),
                    alt = Convert.ToInt32(TBoxALT.Text),
                    other = TBoxOTHER.Text
                }; 
                json.list.Insert(index+1, cell);
            }else
            {
                Cell cell = json.list[index];
                cell.name = TBoxName.Text;
                cell.avo = Convert.ToInt32(TBoxAVO.Text);
                cell.def = Convert.ToInt32(TBoxDEF.Text);
                cell.alt = Convert.ToInt32(TBoxALT.Text);
                cell.other = TBoxOTHER.Text;
            }

            ReloadCellList();
            CellListBox.SelectedIndex = index;
        }

        private readonly string outputPattern = @"(?!((^(con)$)|^(con)\\..*|(^(prn)$)|^(prn)\\..*|(^(aux)$)|^(aux)\\..*|(^(nul)$)|^(nul)\\..*|(^(com)[1-9]$)|^(com)[1-9]\\..*|(^(lpt)[1-9]$)|^(lpt)[1-9]\\..*)|^\\s+|.*\\s$)(^[^\\\\\\/\\:\\<\\>\\*\\?\\\\\\""\\\\|]{1,255}$)";
        private void Button_Click_Gen_Map_Json(object sender, RoutedEventArgs e)
        {
            MapJson map = new MapJson();
            if (json.md5 == null)
            {
                System.Windows.MessageBox.Show("你没有list怎么可以生成map？？？");
                return;
            }
            map.md5 = json.md5;
            if (!Regex.IsMatch(TBoxMapName.Text, outputPattern))
            {
                System.Windows.MessageBox.Show("不要给我乱搞");
                return;
            }
            map.name = TBoxMapName.Text;
            map.width = maxWidth;
            map.height = maxHeight;
            map.content = new Dictionary<int, string>();
            StringBuilder sb = new StringBuilder();
            int lastV = 0;
            for (int i = 0; i <= finalIndex; i++)
            {
                var btn = buttonlist[i];
                if(lastV != btn.Y)
                {
                    map.content.Add(lastV, sb.ToString(0, sb.Length-1));
                    sb.Clear();
                    lastV = btn.Y;
                    if (lastV > maxHeight) break;
                }
                if(btn.C == null)
                {
                    sb.Append(0);
                }else
                {
                    sb.Append(btn.C.idx);
                }
                sb.Append(" ");
            }
            JsonHelp.MapToFile(map, fileDirectory);
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if(CellListBox.SelectedItem != null)
            {
                json.list[CellListBox.SelectedIndex].delete = true;
                json.list.RemoveAt(CellListBox.SelectedIndex);
                ReloadCellList();
                RefreshButtton();
            }
        }

        private void RadioButton_Checked_Multi_Selecte(object sender, RoutedEventArgs e)
        {
            ChangeShellToTop();
        }

        private void RadioButton_Checked_Single_Selecte(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(ShellRectangle, 1);
        }


        private void ShellRectangle_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point p = Mouse.GetPosition(ShellRectangle);
            if (Mouse.LeftButton == MouseButtonState.Pressed && isUsePoint)
            {
                TBoxOTHER.Text = GetButtonIndexByPoint(p).ToString();
                IList<ToolButton> list = GetButtonsByTwoPoint(firstPoint, p);
                SelecteButtons(list);          
            }
        }

        private bool isUsePoint = false;
        private Point firstPoint;
        private void ShellRectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // TODO 记录一个ButtonPoint
            firstPoint = Mouse.GetPosition(ShellRectangle);
            isUsePoint = true;

        }

        private void ShellRectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isUsePoint = false;
        }

        private void ShellRectangle_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isUsePoint = false;
        }

        private void Button_Click_Reset_Image(object sender, RoutedEventArgs e)
        {
            if (BackgroundImage.Source == null) return;
            BackgroundImage.Width = BackgroundImage.Source.Width * scale;
            BackgroundImage.Height = BackgroundImage.Source.Height * scale;

            ResetTBoxImage();
        }



        private string lastValueTBoxImage = "";
        private const string patternTBoxImage = "^[0-9]{0,4}$";
        private void TBoxImage_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = TBoxCell.Text;
            if (input == "")
            {
                lastValueTBoxImage = "";
                return;
            }
            if (!Regex.IsMatch(input, patternTBoxImage))
            {
                TBoxCell.Text = lastValueTBoxImage;
                TBoxCell.SelectionStart = lastValueTBoxImage.Length;
                return;
            }
            // 保存上一个值
            lastValueTBoxImage = input;
        }

        
    }
}
