using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPaint.Shapes
{

    public enum DrawingMode
    {
        None = 0,
        Pen,        // 对应Ink
        //GestureOnly,
        //InkAndGesture,
        //Select,
        //EraseByPoint,
        //EraseByStroke,


        /// <summary>
        /// 画直线
        /// </summary>
        Line,
        /// <summary>
        /// 画矩形
        /// </summary>
        Rectangle,
        /// <summary>
        /// 画椭圆
        /// </summary>
        Ellipse,
    }
}
