﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApplicationTest1.Entity;
using WpfApplicationTest1.Help;
using WpfApplicationTest1.ToolControls;

namespace WpfApplicationTest1
{
    public partial class MainWindow : Window
    {
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
        }

        private int maxWidth = 0;
        private int maxHeight = 0;
        private List<ToolButton> buttonlist = new List<ToolButton>();
        // 生成button
        private void GenerateButton()
        {
            // cellLen 不能太小否则内存爆炸，并且卡炸
            if (BackgroundImage.Source != null && this.cellLen >=32)
            {
                var iter = buttonlist.GetEnumerator();
                int index = 0;
                double dCellLen = this.cellLen * this.scale;
                // 清理 
                ImageCanvas.Children.RemoveRange(buttonStartIndex, ImageCanvas.Children.Count - buttonStartIndex);
                maxWidth = 0;
                maxHeight = 0;
                int x = 0;
                for (double i = 0; i < BackgroundImage.Source.Height * scale; i += dCellLen)
                {
                    int y = 0;
                    for (double j = 0; j < BackgroundImage.Source.Width * scale; j += dCellLen)
                    {                        
                        if (iter.Current != null)
                        {
                            iter.Current.SetCellLen(dCellLen);
                            // 放置
                            ImageCanvas.Children.Add(iter.Current);
                            Canvas.SetLeft(iter.Current, j + BackgroundImage.Margin.Left);
                            Canvas.SetTop(iter.Current, i + BackgroundImage.Margin.Top);
                            // 设置xy
                            iter.Current.X = x;
                            iter.Current.Y = y;
        
                            iter.MoveNext();
                        }
                        else
                        {
                            ToolButton button = new ToolButton(index, dCellLen);
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
                        index++;
                        y++;
                        if(maxWidth < y)
                        {
                            maxWidth = y;
                        }
                    }
                    x++;
                    if (maxHeight < x)
                    {
                        maxHeight = x;
                    }
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
            UIElementCollection list = ImageCanvas.Children;
            for (int i = buttonStartIndex; i < list.Count; i++)
            {
                var btn = list[i] as ToolButton;
                if (btn.C != null && btn.C.delete)
                {
                    btn.Background = Brushes.Transparent;
                    btn.C = null;
                }
            }
        }
    }
}
