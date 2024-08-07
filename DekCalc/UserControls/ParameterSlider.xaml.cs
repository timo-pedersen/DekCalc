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
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(string),
            typeof(ParameterSlider),
            new FrameworkPropertyMetadata("---", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MinProperty = DependencyProperty.Register(
            "Min", typeof(double),
            typeof(ParameterSlider),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCoerceValue));

        public static readonly DependencyProperty MaxProperty = DependencyProperty.Register(
            "Max", typeof(double),
            typeof(ParameterSlider),
            new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCoerceValue));

        public static readonly DependencyProperty DefaultSliderValueProperty = DependencyProperty.Register(
            "DefaultSliderValue", typeof(double),
            typeof(ParameterSlider),
            new FrameworkPropertyMetadata(1.0));

        private MinMaxWindow The_MinMaxWindow;

        public double DefaultSliderValue
        {
            get { return (double)GetValue(DefaultSliderValueProperty); }
            set { SetValue(DefaultSliderValueProperty, value); }
        }

        private static void OnCoerceValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double newValue = Math.Round((double)e.NewValue, 2);
            d.SetValue(e.Property, newValue);
        }

        private static bool OnValidateDouble(object value)
        {
            return true;
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public double Min
        {
            get { return (double)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        public ParameterSlider()
        {
            InitializeComponent();
        }

        public event EventHandler<double> ValueChanged;

        public static readonly DependencyProperty ValueProperty = DependencyProperty
            .Register("Value", typeof(double), typeof(ParameterSlider),
        new FrameworkPropertyMetadata(1.0,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnValueChanged));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ParameterSlider? control = d as ParameterSlider;
            if (control != null)
            {
                control.ValueChanged?.Invoke(control, (double)e.NewValue);
            }
        }

        private void Btn_MinMax_Click(object sender, RoutedEventArgs e)
        {
            if(The_MinMaxWindow is null)
                The_MinMaxWindow = new MinMaxWindow();
            Point p = PointToScreen(Mouse.GetPosition(this));
            The_MinMaxWindow.Min = Min;
            The_MinMaxWindow.Max = Max;
            The_MinMaxWindow.Left = p.X;
            The_MinMaxWindow.Top = p.Y;
            The_MinMaxWindow.ShowDialog();
            Min = The_MinMaxWindow.Min;
            Max = The_MinMaxWindow.Max;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (The_MinMaxWindow is null)
                The_MinMaxWindow = new MinMaxWindow();
            The_MinMaxWindow.Min = Min;
            The_MinMaxWindow.Max = Max;
        }

        private void Slider_Value_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Slider slider)
            {
                Value = DefaultSliderValue;
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            The_MinMaxWindow.Close();
            The_MinMaxWindow = null;
        }
    }
}
