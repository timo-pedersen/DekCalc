using DekCalc.UserControls;
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

namespace DekCalc
{
    /// <summary>
    /// Interaction logic for ParameterSlider.xaml
    /// </summary>
    public partial class ParameterSlider : UserControl
    {
        private double Min, Max;
        public event EventHandler<double> ValueChanged;
        public double Value { get; set; }

        public ParameterSlider()
        {
            InitializeComponent();
            Min = 0;
            Max = 100;
        }

        private void Btn_MinMax_Click(object sender, RoutedEventArgs e)
        {
            MinMaxWindow minMaxWindow = new MinMaxWindow();
            Point p = PointToScreen(Mouse.GetPosition(this));
            minMaxWindow.Min = Min;
            minMaxWindow.Max = Max;
            minMaxWindow.Left = p.X;
            minMaxWindow.Top = p.Y;
            minMaxWindow.ShowDialog();
            Min = minMaxWindow.Min;
            Max = minMaxWindow.Max;
            //ShowValue();
        }

        private void Slider_Value_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Value = (double)e.NewValue;
            ValueChanged?.Invoke(this, e.NewValue);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Textb_Value_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(double.TryParse(Textb_Value.Text, out double value))
            {
                Value = value;
                //ValueChanged?.Invoke(this, value);
            }
        }

        //private void ShowValue()
        //{
        //    double pos = Slider_Value.Value;

        //    double delta = Max - Min;

        //    double newValue = Min + delta * pos / 10;

        //    Textb_Value.Text = newValue.ToString("F2");
        //}
    }
}
