using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPaint.Utility
{
    public class BitmapDescription
    {
        /// <summary>
        /// 图像文件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图像宽度（像素）
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 图像高度（像素）
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 图像水平DPI
        /// </summary>
        public double DPI_X { get; set; }
        /// <summary>
        /// 图像垂直DPI
        /// </summary>
        public double DPI_Y { get; set; }

        public BitmapDescription()
        {
            this.Name = "新建图像";
            this.Width = 800;
            this.Height = 600;
            this.DPI_X = 96;
            this.DPI_Y = 96;
        }
    }
}
