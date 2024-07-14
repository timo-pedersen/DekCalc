using System.Text;
using System.Windows;
using Drawing = System.Drawing;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DekCalc.Bitmaps;
using Graph = DekCalc.Graphing.Graph;
using DekCalc.Function;
using System.Numerics;

namespace DekCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BitMapStuff bmpStuff;
        private Graph _graph = new Graph();

        Func<double, double[], double>? Fx;
        Func<Complex, double[], Complex>? Fz;

        public MainWindow()
        {
            InitializeComponent();
            Width+= 1;
        }

        private void Image_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            if(bmpStuff == null)
                return;

            _graph.G = bmpStuff.G;
            bmpStuff.Clear();

            if(Fx is null)
            {
                Error("Error: Invalid Function");
                return;
            }
            else
            {
                ClearError();
            }

            _graph.Line(0, _graph.Ymin, 0, _graph.Ymax);
            _graph.Line(_graph.Xmin, 0, _graph.Xmax, 0);

            _graph.PlotFunction(Fx);

            ImageBox.Source = bmpStuff.ImageSource;
        }

        private void Error(string error = "Error")
        {
            Textb_Error.Text = error;
            Textb_Error.Foreground = Brushes.Red;
        }

        private void ClearError()
        {
            Textb_Error.Text = string.Empty;
            Textb_Error.Foreground = Brushes.Black;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is null)
                return;

            if (bmpStuff == null)
            {
                //bmpStuff = new BitMapStuff((int)ImageBox.ActualWidth, (int)ImageBox.ActualHeight, Drawing.Color.Gray);
                //bmpStuff = new BitMapStuff((int)TheGrid.ColumnDefinitions[2].ActualWidth, (int)TheGrid.ActualHeight, Drawing.Color.Beige);
                //ImageBox.Source = bmpStuff.ImageSource;
                bmpStuff = new BitMapStuff((int)TheGrid.ColumnDefinitions[2].ActualWidth, (int)TheGrid.ActualHeight, Drawing.Color.Beige);
                ImageBox.Source = bmpStuff.ImageSource;
            }

        }

        private void ResizeBitmap()
        {
            if (bmpStuff == null) return;
            bmpStuff.Update((int)TheGrid.ColumnDefinitions[2].ActualWidth, (int)TheGrid.ActualHeight);
            ImageBox.Source = bmpStuff.ImageSource;
        }

        private void TheGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeBitmap();

            Update();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ClearError();
            Fx = Compiler.CompileSimpleR2Function(TextBox_Func.Text);
            if (!string.IsNullOrWhiteSpace(Compiler.ErrorMessage))
                Error(Compiler.ErrorMessage);
            else
                Update();
        }
    }
}