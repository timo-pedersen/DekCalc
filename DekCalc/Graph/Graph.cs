using DekCalc.Dek;
using DekCalc.Functions;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DekCalc.Graphing
{
    internal class Graph
    {
        private List<Function<Complex, Complex>> _functions { get; } = new();

        public double A { get; set; } = 1;
        public double B { get; set; } = 1;
        public double C { get; set; } = 1;
        public double D { get; set; } = 1;
        public double E { get; set; } = 1;
        public double Img { get; set; } = 0;

        // Screen metrics
        public double Xmin { get; set; } = -10;
        public double Xmax { get; set; } = 10;
        public double Ymin { get; set; } = -10;
        public double Ymax { get; set; } = 10;
        public double Xwidth => Xmax - Xmin;
        public double Yheight => Ymax - Ymin;
        public double Xstep { get; set; }
        public double Ystep { get; set; }
        public int Gwidth { get; set; }
        public int Gheight { get; set; }

        public double GPixelsPerXUnit { get; set; }
        public double GPixelsPerYUnit { get; set; }

        public Color BgColor { get; set; }
        public Color AxisColor { get; set; } = Color.Black;
        public Color GridColor { get; set; } = Color.LightGray;

        // Remember to set G
        Graphics _g;
        public Graphics G
        {
            get
            {
                return _g;
            }
            set
            {
                _g = value;
                Gheight = (int)_g.VisibleClipBounds.Height;
                Gwidth = (int)_g.VisibleClipBounds.Width;
                GPixelsPerXUnit = Gwidth / Xwidth;
                GPixelsPerYUnit = Gheight / Yheight;
            }
        }

        public void AddFunction(Function<Complex, Complex> f) => _functions.Add(f);

        public void ClearFunctions() => _functions.Clear();

        public void ClearGraph()
        {
            if (G is null)
                throw new ArgumentNullException($"G in Graph must be initialized.");

            _g.Clear(BgColor);
            DrawGrid();
            DrawAxes();
        }

        public void PlotFunctions()
        {
            if (G is null)
                throw new ArgumentNullException("G in Graph must be initialized.");

            DrawGrid();
            DrawAxes();

            foreach (Function<Complex, Complex> f in _functions)
            {
                PlotFunction(f.TheFunction, f.Color);
            }
        }

        public bool ValidateFunctions { 
            get {
                if(_functions.Count < 1)
                    return false;

                foreach(Function<Complex, Complex> f in _functions)
                    if (f.TheFunction is null)
                        return false;

                return true;
            }
        }

        double LastValue;
        private void PlotFunction(Func<Complex, double, double, double, double, double, Complex> f, System.Drawing.Color? color = null)
        {
            LastValue = double.NaN;
            if (G is null)
                throw new ArgumentNullException("G in Graph must be initialized.");

            color ??= Color.Red;

            double step = 1 / GPixelsPerXUnit;
            double[] pars = new double[] { A, B, C, D, E };
            for (double x = Xmin; x < Xmax; x += step / 4)
            {
                Complex z = new Complex(x, Img);
                Complex zy = f(z, A, B, C, D, E);
                //double y = Complex.Abs(zy);
                //double y = Math.Sqrt(zy.Real * zy.Real + zy.Imaginary * zy.Imaginary);
                //double y = Complex.Sqrt(zy * Complex.Conjugate(zy)).Real * Math.Sign(zy.Real);
                double y = zy.Real;
                if (!double.IsNaN(LastValue))
                    Line(x-step, LastValue, x, y, color);

                LastValue = y;
            }
        }

        //private void PlotFunction(Func<double, double, double, double, double, double, double> f, Color? color = null)
        //{
        //    if (G is null)
        //        throw new ArgumentNullException("G in Graph must be initialized.");

        //    color ??= Color.Red;

        //    double step = 1 / GPixelsPerXUnit;
        //    double[] pars = new double[]{ A, B, C, D, E };
        //    for (double x = Xmin; x < Xmax; x += step / 4)
        //    {
        //        Line(x, f(x, A, B, C, D, E), x + step, f(x + step, A, B, C, D, E), color);
        //    }
        //}

        private void Line(double x0, double y0, double x1, double y1, System.Drawing.Color? color = null)
        {
            if (G is null)
                throw new ArgumentNullException("G in Graph must be initialized.");

            if (x0 < Xmin || x0 > Xmax ||
                y0 < Ymin || y0 > Ymax ||
                x1 < Xmin || x1 > Xmax ||
                y1 < Ymin || y1 > Ymax)
                return;

            color ??= Color.Black;
            (float gx0, float gy0) = ToGCoords(x0, y0);
            (float gx1, float gy1) = ToGCoords(x1, y1);
            G.DrawLine(new Pen(color.Value), gx0, gy0, gx1, gy1);
        }

        private void Lines(DekPoint[] points, Color? color = null)
        {
            if (G is null)
                throw new ArgumentNullException("G in Graph must be initialized.");

            color ??= Color.Black;
            PointF[] gPoints = new PointF[points.Length];
            int i = 0;
            foreach (var point in points)
            {
                gPoints[i++] = ToGPoint(point);
            }
            G.DrawLines(new Pen(color.Value), gPoints);
        }

        public void DrawAxes()
        {
            if (G is null)
                throw new ArgumentNullException("G in Graph must be initialized.");

            Line(0, Ymin, 0, Ymax, AxisColor);
            Line(Xmin, 0, Xmax, 0, AxisColor);
        }
        public void DrawGrid()
        {
            if (G is null)
                throw new ArgumentNullException("G in Graph must be initialized.");

            for (int x = 0; x < Xmax; x += 1)
                Line(x, Ymin, x, Ymax, GridColor);
            for (int x = 0; x > Xmin; x -= 1)
                Line(x, Ymin, x, Ymax, GridColor);

            for (int y = 0; y < Ymax; y += 1)
                Line(Xmin, y, Xmax, y, GridColor);
            for (int y = 0; y > Ymin; y -= 1)
                Line(Xmin, y, Xmax, y, GridColor);
        }

        #region Utils ==========================================================

        private (float x, float y) ToGCoords(double x, double y)
        {
            PointF gp = ToGPoint(x, y);
            return (gp.X, gp.Y);
        }

        private PointF ToGPoint(DekPoint point)
        {
            return ToGPoint(point.X, point.Y);
        }

        /// <summary>
        /// The main converter from Graph coordinates to Graphics coordinates (i.e. the bitmaps coords)
        /// </summary>
        private PointF ToGPoint(double x, double y)
        {
            //if (x < Xmin || x > Xmax || y < Ymin || y > Ymax)
            //    return new PointF(float.NaN, float.NaN);

            float gx = 0;
            float gy = 0;

            // X,Y distance from left, top
            double xLeft = Math.Abs(Xmin - x);
            double yTop = (Ymax - y);

            return new PointF((float)(xLeft * GPixelsPerXUnit), (float)(yTop * GPixelsPerYUnit));
        }

        //private DekPoint FromGPoint(float gx, float gy)
        //{
        //    if (gx < Xmin || x > Xmax || y < Ymin || y > Ymax)
        //        return new PointF(float.NaN, float.NaN);

        //    float gx = 0;
        //    float gy = 0;

        //    // X,Y distance from left, top
        //    double xLeft = Math.Abs(Xmin - x);
        //    double yTop = (Ymax - y);

        //    return new PointF((float)(xLeft * GPixelsPerXUnit), (float)(yTop * GPixelsPerYUnit));
        //}

        #endregion --------

    }
}
