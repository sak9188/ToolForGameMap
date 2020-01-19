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
using WpfApplicationTest1.Entity;
using Xceed.Wpf.Toolkit;

namespace WpfApplicationTest1.ToolControls
{
    /// <summary>
    /// ToolListItem.xaml 的交互逻辑
    /// </summary>
    public partial class ToolListItem : ListBoxItem
    {
        private string name;
        public string NAME 
        {
            get { return name; }
            set { lName.Content = value; name = value; }
        }

        private int avo;
        public int AVO 
        {
            get { return avo;}
            set { lAvo.Content = value; avo = value; } 
        }

        private int def;
        public int DEF 
        {
            get { return def; }
            set { lDef.Content = value; def = value; } 
        }

        private int alt;
        public int ALT 
        {
            get { return alt;}
            set { lAlt.Content = value; alt = value; } 
        }

        private int index;
        public int INDEX
        {
            get { return index; }
            set { indexer.Content = value; index = value; }
        }

        public ToolListItem()
        {
            InitializeComponent();
        }

        public ToolListItem(Cell cell, int idx) : this()
        {
            NAME = cell.name;
            AVO = cell.avo;
            DEF = cell.def;
            ALT = cell.alt;
            INDEX = idx;
        }

        public ToolListItem(string name, int avo, int def, int alt, int index) : this()
        {
            NAME = name;
            AVO = avo;
            DEF = def;
            ALT = alt;
        }

        private void Button_Indexer_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ToolColorPicker pi = new ToolColorPicker(btn);
            pi.ShowDialog();
        }
    }
}
