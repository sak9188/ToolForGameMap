﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApplicationTest1.Entity;
using WpfApplicationTest1.ToolControls;

namespace WpfApplicationTest1
{
    public partial class MainWindow : Window
    {
        private int currentIdx = -1;
        private void ToolButton_Click(object sender, RoutedEventArgs e)
        {
            ToolButton btn = sender as ToolButton;
            currentIdx = btn.Index;
            // 显示层
            if (CellListBox.SelectedIndex == -1) return;
            int index = CellListBox.SelectedIndex;
            ToolListItem item = CellListBox.SelectedItem as ToolListItem;
            // 处理一下颜色问题
            btn.Background = item.indexer.BorderBrush;
        }

        private void ToolListButton_Click(object sender, RoutedEventArgs e)
        {
            ToolListItem item = sender as ToolListItem;                
            if (json != null && json.list != null)
            {
                Cell c = json.list[item.INDEX-1];
                TBoxName.Text = c.name;
                priviousName = c.name;
                TBoxAVO.Text = c.avo.ToString();
                TBoxDEF.Text = c.def.ToString();
                TBoxALT.Text = c.alt.ToString();
                TBoxOTHER.Text = c.other;
            }
        }
    }
}
