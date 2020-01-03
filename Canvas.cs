using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace OpenPaint
{
    /// <summary>
    /// 画布，画布是用来绘制图形的，永远在最上方
    /// </summary>
    class Canvas : System.Windows.Controls.InkCanvas
    {
        public Canvas()
        {
            this.Background = Brushes.Transparent;
            this.EditingMode = InkCanvasEditingMode.None;
        }

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
        }
    }
}
