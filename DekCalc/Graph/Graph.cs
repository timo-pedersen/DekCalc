using DekCalc.Dek;
using DekCalc.Functions;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static DekCalc.Functions.Compiler;

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

        public void PlotFunctions()
        {
            if (G is null)
                throw new ArgumentNullException("G in Graph must be initialized.");

            foreach (Function<Complex, Complex> f in _functions)
            {
                PlotFunction(f.TheFunction, f.Color);
            }
        }

        public bool ValidateFunctions { get
            {
                if(_functions.Count < 1)
                    return false;

                foreach(Function<Complex, Complex> f in _functions)
                    if (f.TheFunction is null)
                        return false;

                return true;
            }
        }

        private void PlotFunction(Func<Complex, double, double, double, double, double, Complex> f, System.Drawing.Color? color = null)
        {
            if (G is null)
                throw new ArgumentNullException("G in Graph must be initialized.");

            color ??= System.Drawing.Color.Red;

            double step = 1 / GPixelsPerXUnit;
            double[] pars = new double[] { A, B, C, D, E };
            for (double x = Xmin; x < Xmax; x += step / 4)
            {
                Complex z = new Complex(x, 0);
                Line(x, f(z.Real, A, B, C, D, E).Real, x + step, f(z.Real + step, A, B, C, D, E).Real, color);
            }
        }

        private void PlotFunction(Func<double, double, double, double, double, double, double> f, Color? color = null)
        {
            if (G is null)
                throw new ArgumentNullException("G in Graph must be initialized.");

            color ??= Color.Red;

            double step = 1 / GPixelsPerXUnit;
            double[] pars = new double[]{ A, B, C, D, E };
            for (double x = Xmin; x < Xmax; x += step / 4)
            {
                Line(x, f(x, A, B, C, D, E), x + step, f(x + step, A, B, C, D, E), color);
            }
        }

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
            Point[] gPoints = new Point[points.Length];
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

            Line(0, Ymin, 0, Ymax);
            Line(Xmin, 0, Xmax, 0);
        }

        #region Utils ==========================================================

        private (float x, float y) ToGCoords(double x, double y)
        {
            Point gp = ToGPoint(x, y);
            return (gp.X, gp.Y);
        }

        private Point ToGPoint(DekPoint point)
        {
            return ToGPoint(point.X, point.Y);
        }

        /// <summary>
        /// The main converter from Graph coordinates to Graphics coordinates (i.e. the bitmaps coords)
        /// </summary>
        private Point ToGPoint(double x, double y)
        {
            if(x < Xmin || x > Xmax || y < Ymin || y > Ymax )
                return new Point(int.MaxValue, int.MaxValue);

            int gx = 0;
            int gy = 0;

            // X,Y distance from left, top
            double xLeft = Math.Abs(Xmin - x);
            double yTop = (Ymax - y);

            return new Point((int)(xLeft * GPixelsPerXUnit), (int)(yTop * GPixelsPerYUnit));
        }

        #endregion --------

    }
}
