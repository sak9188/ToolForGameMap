﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using WpfApplicationTest1.Entity;
using WpfApplicationTest1.Help;
using WpfApplicationTest1.ToolControls;

namespace WpfApplicationTest1
{
    public partial class MainWindow : Window
    {

        public void Init()
        {
            SetLastWindowSize();
        }

        private void SetLastWindowSize()
        {
            lastWindowHeight = WindowArea.Height;
            lastWindowWidth = WindowArea.Width;
        }

        private double scale = 1;
        // 缩放图片
        private void ScaleBackgroundImage(double scale)
        {
            if(BackgroundImage.Source != null)
            {
                this.scale = scale;
                BackgroundImage.Width = BackgroundImage.Source.Width * scale;
                BackgroundImage.Height = BackgroundImage.Source.Height * scale;
            }
            GenerateButton();
            ChangeShell();
        }

        private int maxWidth = 0;
        private int maxHeight = 0;
        private List<ToolButton> buttonlist = new List<ToolButton>();
        private int finalIndex = 0;
        // 生成button
        private void GenerateButton()
        {
            // cellLen 不能太小否则内存爆炸，并且卡炸
            if (BackgroundImage.Source != null && this.cellLen >=32)
            {
                var iter = buttonlist.GetEnumerator();
                iter.MoveNext();
                finalIndex = 0;
                double dCellLen = this.cellLen * this.scale;
                // 清理 
                ResetButton();
                ImageCanvas.Children.RemoveRange(buttonStartIndex, ImageCanvas.Children.Count - buttonStartIndex);
                maxWidth = (int)Math.Ceiling(BackgroundImage.Source.Width / this.cellLen) - 1;
                maxHeight = (int)Math.Ceiling(BackgroundImage.Source.Height / this.cellLen) - 1;
                int y = 0;
                for (double i = 0; i + dCellLen < BackgroundImage.Source.Height * scale; i += dCellLen)
                {
                    int x = 0;
                    for (double j = 0; j + dCellLen < BackgroundImage.Source.Width * scale; j += dCellLen)
                    {                        
                        if (iter.Current != null)
                        {
                            // 设置xy
                            iter.Current.X = x;
                            iter.Current.Y = y;
                            iter.Current.SetCellLen(dCellLen);
                            // 放置
                            ImageCanvas.Children.Add(iter.Current);
                            Canvas.SetLeft(iter.Current, j + BackgroundImage.Margin.Left);
                            Canvas.SetTop(iter.Current, i + BackgroundImage.Margin.Top);
                            
                            iter.MoveNext();
                        }
                        else
                        {
                            ToolButton button = new ToolButton(finalIndex, dCellLen);
                            buttonlist.Add(button);
                            // 放置
                            ImageCanvas.Children.Add(button);
                            Canvas.SetLeft(button, j + BackgroundImage.Margin.Left);
                            Canvas.SetTop(button, i + BackgroundImage.Margin.Top);
                            Canvas.SetZIndex(button, 10);
                            button.Click += new RoutedEventHandler(ToolButton_Click);
                            // 设置xy
                            button.X = x;
                            button.Y = y;

                        }
                        // 其他操作
                        finalIndex++;
                        x++;
                    }
                    y++;
                }
                isChangedMap = 0;
            }
        }

        private CellListJson json;
        /// 生成cell list
        private void GenerateCellList(String path)
        {
            json = JsonHelp.LoadJsonFile(path);
            ReloadCellList();
        }

        private void ReloadCellList()
        {
            IList<Cell> list = json.list;
            if (list != null)
            {
                if (CellListBox.Items.Count != 0) CellListBox.Items.Clear();
                int index = 1;
                foreach (Cell item in list)
                {
                    var listItem = new ToolListItem(item, index);
                    item.idx = index;
                    listItem.Selected += ToolListButton_Click;
                    CellListBox.Items.Add(listItem);
                    index++;
                }
            }
            listName.Content = json.name;
        }

        private void RefreshButtton()
        {
            foreach (ToolButton item in buttonlist)
            {
                if (item.C != null && item.C.delete)
                {
                    item.Background = Brushes.Transparent;
                    item.C = null;
                }
            }
        }

        private void ResetButton()
        {
            foreach (ToolButton item in buttonlist)
            {
                BindingOperations.ClearAllBindings(item);
                item.Background = Brushes.Transparent;
                item.C = null;
            }
        }

        private void ChangeShellToTop()
        {
            ChangeShell();
            Canvas.SetZIndex(ShellRectangle, 100);
        }

        private void ChangeShell()
        {
            int offset = cellLen;
            ShellRectangle.Width = BackgroundImage.Width + offset;
            ShellRectangle.Height = BackgroundImage.Height + offset;
        }

        private int GetButtonIndexByPoint(Point p)
        {
            int x = (int)Math.Floor(p.X / (cellLen * scale));
            int y = (int)Math.Floor(p.Y / (cellLen * scale));
            if (x >= maxWidth)
            {
                x = maxWidth - 1;
            }
            if (y >= maxHeight)
            {
                y = maxHeight - 1;
            }
            int index = Convert.ToInt32(y * maxWidth + x);
            return index;
        }

        private ToolButton GetButtonByPoint(Point p)
        {            
            // 计算是哪个按钮被按到了
            int index = GetButtonIndexByPoint(p);
            return buttonlist[index] as ToolButton;
        }

        private ToolButton GetLeftTopButon(Point p1, Point p2)
        {
            double x = p1.X < p2.X ? p1.X : p2.X;
            double y = p1.Y < p2.Y ? p1.Y : p2.Y;
            return GetButtonByPoint(new Point(x, y));
        }

        private IList<ToolButton> GetButtons(ToolButton btn, int offsetX, int offsetY)
        {
            IList<ToolButton> list = new List<ToolButton>();
            int desY = btn.Y + offsetY + 1;
            int desX = btn.X + offsetX + 1;
            for (int i = btn.Y; i < desY; i++)
            {
                for (int j = btn.X; j < desX; j++)
                {
                    list.Add(buttonlist[i * maxWidth + j] as ToolButton);
                }
            }
            return list;
        }

        private IList<ToolButton> GetButtonsByTwoPoint(Point p1, Point p2)
        {
            ToolButton leftTopButton = GetLeftTopButon(p1, p2);
            ToolButton btn1 = GetButtonByPoint(p1);
            ToolButton btn2 = GetButtonByPoint(p2);
            int offsetX = Math.Abs(btn1.X - btn2.X);
            int offsetY = Math.Abs(btn1.Y - btn2.Y);
            return GetButtons(leftTopButton, offsetX, offsetY);
        }

        private void ButtonSelecte(ToolButton btn)
        {
            if (CellListBox.SelectedIndex == -1) return;
            int index = CellListBox.SelectedIndex;
            // 处理一下颜色问题
            ToolListItem item = CellListBox.SelectedItem as ToolListItem;
            Binding binding = new Binding();
            binding.Source = item.indexer;
            binding.Path = new System.Windows.PropertyPath(BorderBrushProperty);
            btn.SetBinding(BackgroundProperty, binding);
            // 数据层
            btn.C = json.list[index];
            isChangedMap = 1;
        }

        private void SelecteButtons(IList<ToolButton> list)
        {
            foreach (ToolButton item in list)
            {
                ButtonSelecte(item);
            }
        }

        private void ResetTBoxImage()
        {
            if (BackgroundImage.Source == null) return;
            TBoxImageWidth.Text = BackgroundImage.Source.Width.ToString("0");
            TBoxImageHeight.Text = BackgroundImage.Source.Height.ToString("0");
        }

        private double lastWindowWidth;
        private double lastWindowHeight;
        private void Maxmize()
        {
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            Topmost = true;//最大化后总是在最上面 
                           //获取窗口句柄 
            var handle = new WindowInteropHelper(this).Handle;
            //获取当前显示器屏幕
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(handle);

            //调整窗口最大化,全屏的关键代码就是下面3句 
            this.MaxWidth = screen.Bounds.Width;
            this.MaxHeight = screen.Bounds.Height;
            WindowState = WindowState.Maximized;

            //调整区域最大化
            WindowArea.Width = MaxWidth;
            WindowArea.Height = MaxWidth;
        }
    }
}
