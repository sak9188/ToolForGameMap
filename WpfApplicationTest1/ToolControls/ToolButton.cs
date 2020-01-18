using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApplicationTest1.ToolControls
{
    public class ToolButton : Button
    {
        private readonly string borderColor = "#FF707070";
        private readonly static ResourceDictionary rd = new ResourceDictionary();
        private static ControlTemplate TP;

        static ToolButton()
        {
            rd.Source = new Uri("ToolControls/ToolControls.xaml", UriKind.Relative);
            TP = (ControlTemplate)rd["ToolButton"];
        }

        private int index;
        public int Index
        {
            get { return index; }
            set { index = value; base.Content = value; }
        }


        public ToolButton()
        {
            base.Background = Brushes.Transparent;
            base.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(borderColor));
            base.Template = TP;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num">序号</param>
        public ToolButton(int num, double cellLen)
            : this()
        {
            Index = num;
            this.SetCellLen(cellLen);
        }

        public void SetCellLen(double cellLen)
        {
            base.Width = cellLen;
            base.Height = cellLen;
        }
    }
}
