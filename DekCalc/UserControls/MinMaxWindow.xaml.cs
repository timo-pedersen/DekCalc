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

namespace DekCalc.UserControls
{
    /// <summary>
    /// Interaction logic for MinMaxWindow.xaml
    /// </summary>
    public partial class MinMaxWindow : Window
    {
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(double), typeof(MinMaxWindow));

        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(double), typeof(MinMaxWindow));

        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        public double Min
        {
            get { return (double)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        public MinMaxWindow()
        {
            try
            {
                InitializeComponent();

                Binding binding1 = new Binding("Max");
                binding1.Source = this;
                binding1.Mode = BindingMode.TwoWay;
                Textb_Max.SetBinding(TextBox.TextProperty, binding1);

                Binding binding2 = new Binding("Min");
                binding2.Source = this;
                binding2.Mode = BindingMode.TwoWay;
                Textb_Min.SetBinding(TextBox.TextProperty, binding2);
            }catch (Exception ex) {
                Width = 10;
                Height = 10;
            }
        }

        private void Btn_Done_Click(object sender, RoutedEventArgs e)
        {
            //if (double.TryParse(Textb_Max.Text, out double max))
            //    Max = max;
            //if (double.TryParse(Textb_Min.Text, out double min))
            //    Min = min;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Textb_Min.Text = Min.ToString();
            Textb_Max.Text = Max.ToString();
        }
    }
}
