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

namespace OpenPaint.Controls
{
    /// <summary>
    /// SpectrumSlider.xaml 的交互逻辑
    /// </summary>
    public partial class SpectrumSlider : Slider
    {
        public SpectrumSlider()
        {
            InitializeComponent();
        }

        static SpectrumSlider()
        {
            // 重载原数据
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpectrumSlider),
                new FrameworkPropertyMetadata(typeof(SpectrumSlider)));
        }


        #region Public Properties
        public Color SelectedColor
        {
            get
            {
                return (Color)GetValue(SelectedColorProperty);
            }
            set
            {
                SetValue(SelectedColorProperty, value);
            }
        }
        #endregion


        #region Dependency Property Fields
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register
            ("SelectedColor", typeof(Color), typeof(SpectrumSlider),
            new PropertyMetadata(System.Windows.Media.Colors.Transparent));

        #endregion


        #region Public Methods

        public override void OnApplyTemplate()
        {

            base.OnApplyTemplate();
            m_spectrumDisplay = GetTemplateChild(SpectrumDisplayName) as Rectangle;
            updateColorSpectrum();
            OnValueChanged(Double.NaN, Value);

        }

        #endregion


        #region Protected Methods
        protected override void OnValueChanged(double oldValue, double newValue)
        {

            base.OnValueChanged(oldValue, newValue);
            Color theColor = ColorUtilities.ConvertHsvToRgb(360 - newValue, 1, 1);
            SetValue(SelectedColorProperty, theColor);
        }
        #endregion


        #region Private Methods

        private void updateColorSpectrum()
        {
            if (m_spectrumDisplay != null)
            {
                createSpectrum();
            }
        }



        private void createSpectrum()
        {

            pickerBrush = new LinearGradientBrush();
            pickerBrush.StartPoint = new Point(0.5, 0);
            pickerBrush.EndPoint = new Point(0.5, 1);
            pickerBrush.ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation;


            List<Color> colorsList = ColorUtilities.GenerateHsvSpectrum();
            double stopIncrement = (double)1 / colorsList.Count;

            int i;
            for (i = 0; i < colorsList.Count; i++)
            {
                pickerBrush.GradientStops.Add(new GradientStop(colorsList[i], i * stopIncrement));
            }

            pickerBrush.GradientStops[i - 1].Offset = 1.0;
            m_spectrumDisplay.Fill = pickerBrush;

        }
        #endregion


        #region Private Fields
        private static string SpectrumDisplayName = "PART_SpectrumDisplay";
        private Rectangle m_spectrumDisplay;
        private LinearGradientBrush pickerBrush;
        #endregion
    }
}
