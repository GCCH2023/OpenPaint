using OpenPaint.Utility;
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

namespace OpenPaint
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private DrawingBoard CurrentDrawingBoard
        {
            get 
            {
                return (DrawingBoard)((TabItem)this.tabBoard.SelectedItem).Content;
            }
        }

        private void AddLayer_Click(object sender, RoutedEventArgs e)
        {
            CurrentDrawingBoard.AddLayer();
        }

        private void DeleteLayer_Click(object sender, RoutedEventArgs e)
        {
            var index = this.layerList.SelectedIndex;
            CurrentDrawingBoard.DeleteLayer((Layer)this.layerList.SelectedItem);
            this.layerList.SelectedIndex = index == this.layerList.Items.Count ? index - 1 : index;
        }

        private void Debug_Click(object sender, RoutedEventArgs e)
        {
            //this.drawingBoard.Debug();
        }
        /// <summary>
        /// 往TabControl中添加一个TabItem及一个画板
        /// </summary>
        private void NewDrawingBoard(BitmapDescription bitmapDescription)
        {
            DrawingBoard drawingBoard = new DrawingBoard(bitmapDescription);
            // 为画板创建数据绑定
            Binding binding = new Binding();        // 当前图层
            binding.Source = this.layerList;
            binding.Path = new PropertyPath("SelectedItem");
            drawingBoard.SetBinding(DrawingBoard.CurrentLayerProperty, binding);

            binding = new Binding();        // 绘画模式
            binding.Source = this.toolList;
            binding.Path = new PropertyPath("SelectedItem");
            drawingBoard.SetBinding(DrawingBoard.DrawingModeProperty, binding);

            binding = new Binding();        // 绘画颜色
            binding.Source = this.colorPicker;
            binding.Path = new PropertyPath("SelectedColor");
            drawingBoard.SetBinding(DrawingBoard.ColorProperty, binding);

            binding = new Binding();        // 画笔大小
            binding.Source = this.penSize;
            binding.Path = new PropertyPath("Value");
            drawingBoard.SetBinding(DrawingBoard.PenThicknessProperty, binding);

            TabItem tabItem = new TabItem();
            tabItem.Header = bitmapDescription.Name;
            tabItem.Content = drawingBoard;

            this.tabBoard.Items.Add(tabItem);
            this.tabBoard.SelectedItem = tabItem;

            this.layerList.SelectedIndex = 0;
        }
        /// <summary>
        /// 新建文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NewFileDialog dialog = new NewFileDialog();
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                NewDrawingBoard(dialog.BitmapDescription);
            }
        }
        /// <summary>
        /// 打开图像文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string filename = BitmapHelper.Open();
            if (filename != null)
            {
                BitmapImage imgSource = BitmapHelper.GetBitmapImage(filename);
                // 创建位图描述信息
                BitmapDescription bitmapDescription = new BitmapDescription();
                bitmapDescription.Name = filename;
                bitmapDescription.Width = imgSource.PixelWidth;
                bitmapDescription.Height = imgSource.PixelHeight;
                bitmapDescription.DPI_X = imgSource.DpiX;
                bitmapDescription.DPI_Y = imgSource.DpiY;
                // 创建画布
                NewDrawingBoard(bitmapDescription);
                // 将图像绘制到画布的图层上
                CurrentDrawingBoard.AddBitmap(imgSource);
            }
        }
        /// <summary>
        ///  保存位图文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var filename = CurrentDrawingBoard.Save();
            if (filename != null)
                ((TabItem)tabBoard.SelectedItem).Header = filename;
        }
        /// <summary>
        /// 是否能保存位图文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanSaveCommandExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (tabBoard.SelectedItem != null && CurrentDrawingBoard != null);
        }
        /// <summary>
        /// 文件另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentDrawingBoard.SaveAs();
        }
        /// <summary>
        /// 是否能另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanSaveAsCommandExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (tabBoard.SelectedItem != null && CurrentDrawingBoard != null);
        }
    }
}
