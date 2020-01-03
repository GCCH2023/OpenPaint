using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPaint.Controls
{
    struct HsvColor
    {

        public double H;
        public double S;
        public double V;

        public HsvColor(double h, double s, double v)
        {
            this.H = h;
            this.S = s;
            this.V = v;
        }
    }
}
