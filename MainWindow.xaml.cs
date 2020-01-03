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

        private void AddLayer_Click(object sender, RoutedEventArgs e)
        {
            this.drawingBoard.AddLayer();
        }

        private void DeleteLayer_Click(object sender, RoutedEventArgs e)
        {
            var index = this.layerList.SelectedIndex;
            this.drawingBoard.DeleteLayer((Layer)this.layerList.SelectedItem);
            this.layerList.SelectedIndex = index == this.layerList.Items.Count ? index - 1 : index;
        }

        private void Debug_Click(object sender, RoutedEventArgs e)
        {
            this.drawingBoard.Debug();
        }
    }
}
