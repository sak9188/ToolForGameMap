﻿using System;
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
using System.Windows.Shapes;

namespace WpfApplicationTest1.ToolControls
{
    /// <summary>
    /// ToolColorPicker.xaml 的交互逻辑
    /// </summary>
    public partial class ToolColorPicker : Window
    {
        private Button btn;
        public ToolColorPicker()
        {
            InitializeComponent();
        }

        public ToolColorPicker(Button btn):this()
        {
            this.btn = btn;
            SolidColorBrush brush = btn.BorderBrush as SolidColorBrush;
            cCanvas.SelectedColor = brush.Color;
        }

        private void cCanvas_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            SolidColorBrush brush = btn.Background as SolidColorBrush;
            brush.Color = (Color)cCanvas.SelectedColor;
        }


    }
}