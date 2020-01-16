using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        private List<ToolButton> buttonlist = new List<ToolButton>();
        // 生成button
        private void GenerateButton()
        {
            if (BackgroundImage.Source != null && this.cellLen != 0)
            {
                var iter = buttonlist.GetEnumerator();
                int index = 0;
                double dCellLen = this.cellLen * this.scale;
                // 清理 
                ImageCanvas.Children.RemoveRange(1, ImageCanvas.Children.Count - 1);
                for (double i = 0; i < BackgroundImage.Source.Width * scale; i += dCellLen)
                {
                    for (double j = 0; j < BackgroundImage.Source.Height * scale; j += dCellLen)
                    {
                        if (iter.Current != null)
                        {
                            iter.Current.SetCellLen(dCellLen);
                            // 放置
                            ImageCanvas.Children.Add(iter.Current);
                            Canvas.SetLeft(iter.Current, i + BackgroundImage.Margin.Left);
                            Canvas.SetTop(iter.Current, j + BackgroundImage.Margin.Top);
                            iter.MoveNext();
                        }
                        else
                        {
                            ToolButton button = new ToolButton(index, dCellLen);
                            buttonlist.Add(button);
                            // 放置
                            ImageCanvas.Children.Add(button);
                            Canvas.SetLeft(button, i + BackgroundImage.Margin.Left);
                            Canvas.SetTop(button, j + BackgroundImage.Margin.Top);
                        }
                        index++;
                    }                    
                }
            }
        }
    }
}
