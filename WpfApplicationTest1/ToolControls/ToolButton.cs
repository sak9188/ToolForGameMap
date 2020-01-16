using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApplicationTest1.ToolControls
{
    public class ToolButton : Button
    {
        public int Index { get; set; }

        public ToolButton()
        {
            base.Background = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num">序号</param>
        public ToolButton(int num, double cellLen)
            : this()
        {
            Index = num;
            base.Content = num;
            this.SetCellLen(cellLen);
        }

        public void SetCellLen(double cellLen)
        {
            base.Width = cellLen;
            base.Height = cellLen;
        }
    }
}
