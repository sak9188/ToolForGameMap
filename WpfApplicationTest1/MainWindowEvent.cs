using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        }
    }
}
