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
using System.Windows.Shapes;

namespace OpenPaint
{
    /// <summary>
    /// NewFileDialog.xaml 的交互逻辑
    /// </summary>
    public partial class NewFileDialog : Window
    {
        public NewFileDialog()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 位图描述
        /// </summary>
        public Utility.BitmapDescription BitmapDescription
        {
            get { return (Utility.BitmapDescription)this.DataContext; }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
