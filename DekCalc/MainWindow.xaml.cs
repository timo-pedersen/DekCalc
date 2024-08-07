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
using DekCalc.Functions;
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

        //Func<double, double, double, double, double, double, double>? Fx;
        //Func<Complex, double, double, double, double, double, Complex>? Fx;

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

            if(!_graph.ValidateFunctions)
            {
                Error("Error: Invalid Function");
                return;
            }
            else
            {
                ClearError();
            }

            _graph.PlotFunctions();

            ImageBox.Source = bmpStuff.ImageSource;
        }

        private void Error(string error = "Error")
        {
            Textb_Error.Text = error;
            Textb_Error.Foreground = Brushes.Red;
            Errors.Visibility = Visibility.Visible;
        }

        private void ClearError()
        {
            Textb_Error.Text = string.Empty;
            Textb_Error.Foreground = Brushes.Black;
            Errors.Visibility = Visibility.Collapsed;
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

            //if(_graph is null)
            //{
            //    _graph = new Graph();
            //    _graph.BgColor = Drawing.Color.Beige;
            //}

            //if(_graph.G is null)
            //    _graph.G = bmpStuff.G;

            //_graph.ClearGraph();

            HookupEvents();
        }

        private void HookupEvents()
        {
            Parameter_Img.ValueChanged += Parameter_ValueChanged;
            Parameter_A.ValueChanged += Parameter_ValueChanged;
            Parameter_B.ValueChanged += Parameter_ValueChanged;
            Parameter_C.ValueChanged += Parameter_ValueChanged;
            Parameter_D.ValueChanged += Parameter_ValueChanged;
            Parameter_E.ValueChanged += Parameter_ValueChanged;
        }

        private void Parameter_ValueChanged(object? sender, double e)
        {
            if (!(sender is ParameterSlider slider) || _graph is null)
                return;

            switch (slider.Name)
            {
                case "Parameter_Img":
                    _graph.Img = Parameter_Img.Value;
                    break;
                case "Parameter_A":
                    _graph.A = Parameter_A.Value;
                    break;
                case "Parameter_B":
                    _graph.B = Parameter_B.Value;
                    break;
                case "Parameter_C":
                    _graph.C = Parameter_C.Value;
                    break;
                case "Parameter_D":
                    _graph.D = Parameter_D.Value;
                    break;
                case "Parameter_E":
                    _graph.E = Parameter_E.Value;
                    break;
                default:
                    break;
            }

            Update();
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

        private void Button_Compile_Click(object sender, RoutedEventArgs e)
        {
            ClearError();
            _graph.ClearFunctions();  // For now

            Func<Complex, double, double, double, double, double, Complex> fx = Compiler.CompileSimpleR2Function(TextBox_Func.Text);
            Function<Complex, Complex> function = new Function<Complex, Complex>(fx, Drawing.Color.Blue, "A function");
            _graph.AddFunction(function);
            if (!string.IsNullOrWhiteSpace(Compiler.ErrorMessage))
                Error(Compiler.ErrorMessage);
            else
                Update();
        }

        private void ImageBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void ImageBox_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ImageBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double scaleFactor = e.Delta < 0 ? 1.25 : 0.9;

            _graph.Ymax = _graph.Ymax * scaleFactor;
            _graph.Ymin = _graph.Ymin * scaleFactor;
            _graph.Xmax = _graph.Xmax * scaleFactor;
            _graph.Xmin = _graph.Xmin * scaleFactor;

            Update();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //MessageBox.Show("Wooot");
        }
    }
}