using System.Drawing;

namespace DekCalc.Dek
{
    public struct DekPoint
    {
        public static DekPoint Empty => new DekPoint(double.NaN, double.NaN);

        public DekPoint()
        {
            X = Y = double.NaN;
        }

        public DekPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; } = double.NaN;
        public double Y { get; set; } = double.NaN;

        public PointF ToPointF()
        {
            return new PointF((float)X, (float)Y);
        }
    }
}
