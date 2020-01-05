using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpenPaint.Utility
{
    class BitmapHelper
    {
        /// <summary>
        /// 将指定位图保存到指定路径中
        /// </summary>
        /// <param name="bitmap">要保存的位图</param>
        /// <param name="path">要保存到的路径（包含文件名和扩展卡名）</param>
        public static void Save(BitmapSource bitmap, string path)
        {
            BitmapEncoder encoder = null;
            string fileExtension = System.IO.Path.GetExtension(path).ToUpper();
            //选取编码器
            switch (fileExtension)
            {
                case ".BMP":
                    encoder = new BmpBitmapEncoder();
                    break;
                case ".GIF":
                    encoder = new GifBitmapEncoder();
                    break;
                case ".JPEG":
                    encoder = new JpegBitmapEncoder();
                    break;
                case ".PNG":
                    encoder = new PngBitmapEncoder();
                    break;
                case ".TIFF":
                    encoder = new TiffBitmapEncoder();
                    break;
                default:
                    throw new Exception("无法识别的图像格式！");
            }
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using (var file = File.Create(path))
                encoder.Save(file);
        }
        /// <summary>
        /// 从控件表面获取图像
        /// </summary>
        /// <param name="element"></param>
        /// <param name="type"></param>
        /// <param name="outputPath"></param>
        public static void GetPicFromControl(FrameworkElement element, String type, String outputPath)
        {
            //96为显示器DPI
            var bitmapRender = new RenderTargetBitmap((int)element.ActualWidth, (int)element.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            //控件内容渲染RenderTargetBitmap
            bitmapRender.Render(element);
            BitmapEncoder encoder = null;
            //选取编码器
            switch (type.ToUpper())
            {
                case ".BMP":
                    encoder = new BmpBitmapEncoder();
                    break;
                case ".GIF":
                    encoder = new GifBitmapEncoder();
                    break;
                case ".JPEG":
                    encoder = new JpegBitmapEncoder();
                    break;
                case ".PNG":
                    encoder = new PngBitmapEncoder();
                    break;
                case ".TIFF":
                    encoder = new TiffBitmapEncoder();
                    break;
                default:
                    break;
            }
            //对于一般的图片，只有一帧，动态图片是有多帧的。
            encoder.Frames.Add(BitmapFrame.Create(bitmapRender));
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(outputPath)))
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));
            using (var file = File.Create(outputPath))
                encoder.Save(file);
        }
        /// <summary>
        /// 另存为，返回保存后的文件名
        /// </summary>
        /// <param name="element"></param>
        public static string SaveAs(BitmapSource bitmap, string path)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "PNG文件(*.png)|*.png|JPG文件(*.jpg)|*.jpg|BMP文件(*.bmp)|*.bmp|GIF文件(*.gif)|*.gif|TIF文件(*.tif)|*.tif";
            saveFileDialog.FileName = path;
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Save(bitmap, path);
                return saveFileDialog.FileName;
            }
            return null;
        }
        /// <summary>
        /// 打开图片，返回图片路径
        /// </summary>
        /// <returns></returns>
        public static string Open()
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "PNG文件(*.png)|*.png|JPG文件(*.jpg)|*.jpg|BMP文件(*.bmp)|*.bmp|GIF文件(*.gif)|*.gif|TIF文件(*.tif)|*.tif";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return ofd.FileName;
            }
            return null;
        }
        /// <summary>
        /// 创建图像文件的副本，解决文件被占用的问题
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static BitmapImage GetBitmapImage(string imagePath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(imagePath);
            bitmap.EndInit();
            return bitmap.Clone();
        }
    }
}
