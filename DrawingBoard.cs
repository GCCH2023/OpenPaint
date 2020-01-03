using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using OpenPaint.Collections;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Ink;
using OpenPaint.Shapes;

namespace OpenPaint
{
    using Sys = System.Windows.Controls;
    using System.Reflection;
    /// <summary>
    /// 画板，一个画板可以有很多图层
    /// 实际上一个画板对应一副图像
    /// </summary>
    class DrawingBoard : Label
    {
        // 画布，图层容器
        Canvas inkCanvas;

        /// <summary>
        /// 当前绘制的图形
        /// </summary>
        DrawingVisual shape = null;

        /// <summary>
        /// 画板至少拥有一个图层
        /// </summary>
        public DrawingBoard()
        {
            this.HorizontalContentAlignment = HorizontalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Background = new SolidColorBrush(Colors.Gray);

            inkCanvas = new Canvas();
            inkCanvas.Width = 800;
            inkCanvas.Height = 600;
            inkCanvas.Background = (Brush)FindResource("CheckerBrush");
            this.Content = inkCanvas;

            this.AddLayer();
        }

        #region 图层
/// <summary>
        /// 图层容器，用来通知更改
        /// </summary>
        ObservableCollection<Layer> layers = new ObservableCollection<Layer>();
        public ObservableCollection<Layer> Layers { get { return layers; } }

        /// <summary>
        /// 添加一个图层
        /// </summary>
        public void AddLayer()
        {
            Layer layer = new Layer((int)inkCanvas.Width, (int)inkCanvas.Height);
            // 新建的图层显示在上面，所以是插入到0位置
            inkCanvas.Children.Add(layer);
            this.layers.Insert(0, layer);
        }

        internal void DeleteLayer(Layer layer)
        {
            // 至少要有1个图层
            if (this.layers.Count > 1)
            {
                inkCanvas.Children.Remove(layer);
                this.layers.Remove(layer);
            }
        }
#endregion

        #region 图层相关事件

        Point startPoint;
        /// <summary>
        /// 在鼠标左键按下时创建图形
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // 只有规则图形才创建DrawingVisual对象
            if ((int)DrawingMode >= (int)Shapes.DrawingMode.Line)
            {
                // 利用反射创建特定实例
                shape = new DrawingVisual();
                // 添加到图层中
                CurrentLayer.AddVisual(shape);

                startPoint = e.GetPosition(inkCanvas);
            }
        }

        /// <summary>
        /// 在鼠标移动时，重新渲染图形
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (shape != null)
            {
                using (var dc = shape.RenderOpen())
                {
                    Pen pen = new Pen(new SolidColorBrush(Color), PenThickness);
                    Point endPoint = e.GetPosition(inkCanvas);
                    switch (DrawingMode)
                    {
                        case Shapes.DrawingMode.Line:
                            dc.DrawLine(pen, startPoint, endPoint);
                            break;
                        case Shapes.DrawingMode.Rectangle:
                            dc.DrawRectangle(Brushes.Transparent, pen, new Rect(startPoint, endPoint));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 鼠标松开时，完成一次绘图，将图形移动到当前图层
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            // 如果不是规则图形则需要添加到图层中
            if (DrawingMode == Shapes.DrawingMode.Pen)
            {
                InkPresenter inkPresenter = new InkPresenter();
                inkPresenter.Strokes = inkCanvas.Strokes;
                CurrentLayer.AddUIElement(inkPresenter);
                // 重新创建Stroke对象
                inkCanvas.Strokes = new StrokeCollection();
            }
            else
                shape = null;
        }

        public void Debug()
        {
            
        }

        #endregion

        /// <summary>
        /// 绘画模式
        /// </summary>
        public DrawingMode DrawingMode
        {
            get { return (DrawingMode)GetValue(DrawingModeProperty); }
            set { SetValue(DrawingModeProperty, value); }
        }

        public static readonly DependencyProperty DrawingModeProperty =
            DependencyProperty.Register("DrawingMode", typeof(DrawingMode), typeof(DrawingBoard),
            new PropertyMetadata((DrawingMode)0, new PropertyChangedCallback(DrawingModePropertyChanged)));

        private static void DrawingModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DrawingMode drawingMode = (DrawingMode)e.NewValue;
            var drawingBoard = d as DrawingBoard;
            // 绘制规则图形
            if (drawingMode != DrawingMode.Pen)
                drawingBoard.inkCanvas.EditingMode = InkCanvasEditingMode.None;
            else
            {
                // 设置绘画模式
                drawingBoard.inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            }
        }

        /// <summary>
        /// 当前图层
        /// </summary>
        public Layer CurrentLayer
        {
            get { return (Layer)GetValue(CurrentLayerProperty); }
            set { SetValue(CurrentLayerProperty, value); }
        }

        public static readonly DependencyProperty CurrentLayerProperty =
            DependencyProperty.Register("CurrentLayer", typeof(Layer), typeof(DrawingBoard),
            new PropertyMetadata(null));


        /// <summary>
        /// 绘画颜色
        /// </summary>
        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(DrawingBoard),
            new PropertyMetadata(Colors.Black, new PropertyChangedCallback(ColorPropertyChanged)));

        private static void ColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var drawingBoard = d as DrawingBoard;
            var drawingAttributes = drawingBoard.inkCanvas.DefaultDrawingAttributes;
            drawingAttributes.Color = drawingBoard.Color;
        }


        /// <summary>
        /// 画笔大小
        /// </summary>
        public double PenThickness
        {
            get { return (double)GetValue(PenThicknessProperty); }
            set { SetValue(PenThicknessProperty, value); }
        }

        public static readonly DependencyProperty PenThicknessProperty =
            DependencyProperty.Register("PenThickness", typeof(double), typeof(DrawingBoard),
            new PropertyMetadata(1.0, new PropertyChangedCallback(PenThicknessPropertyChanged)));

        private static void PenThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var drawingBoard = d as DrawingBoard;
            var drawingAttributes = drawingBoard.inkCanvas.DefaultDrawingAttributes;
            drawingAttributes.Width = drawingAttributes.Height = drawingBoard.PenThickness;
        }
    }
}
