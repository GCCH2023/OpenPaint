using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OpenPaint.Shapes
{
    public abstract class Shape : DrawingVisual
    {
        /// <summary>
        /// 渲染图形
        /// </summary>
        protected abstract void OnRender(DrawingContext dc);

        public void Render()
        {
            using (var dc = this.RenderOpen())
                this.OnRender(dc);
        }
    }
}
