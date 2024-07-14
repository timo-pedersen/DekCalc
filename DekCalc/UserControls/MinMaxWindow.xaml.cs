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
        public double Max { get; set; }
        public double Min { get; set; }

        public MinMaxWindow()
        {
            InitializeComponent();
        }

        private void Btn_Done_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Textb_Max.Text, out double max))
                Max = max;
            if (double.TryParse(Textb_Min.Text, out double min))
                Min = min;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Textb_Min.Text = Min.ToString();
            Textb_Max.Text = Max.ToString();
        }
    }
}
