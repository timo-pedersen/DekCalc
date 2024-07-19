using System.Drawing;

namespace DekCalc.Dek
{
    public class DekLineSegment
    {
        public DekPoint A = DekPoint.Empty;
        public DekPoint B = DekPoint.Empty;

        public DekLineSegment(DekPoint a, DekPoint b)
        {
            A = a;
            B = b;
        }

        public DekLineSegment()
        {
        }

        public DekPoint StartPoint => A;
        public DekPoint EndPoint => B;

        public static DekPoint GetCrossingPoint(DekLineSegment segment1, DekLineSegment segment2)
        {
            DekPoint output = DekPoint.Empty;

            double x1, x2, x3, x4, y1, y2, y3, y4;

            x1 = segment1.StartPoint.X;
            x2 = segment1.EndPoint.X;
            x3 = segment2.StartPoint.X;
            x4 = segment2.EndPoint.X;

            y1 = segment1.StartPoint.Y;
            y2 = segment1.EndPoint.Y;
            y3 = segment2.StartPoint.Y;
            y4 = segment2.EndPoint.Y;

            double den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (den != 0)
            {
                double t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / den;
                double u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / den;

                if (t > 0 && t < 1 && u > 0 && u < 1)
                {
                    output = new DekPoint(x1 + t * (x2 - x1), y1 + t * (y2 - y1));
                }
            }

            return output;
        }

        public DekPoint GetCrossingPoint(DekLineSegment segment)
        {
            DekPoint output = DekPoint.Empty;

            double x1, x2, x3, x4, y1, y2, y3, y4;

            x1 = StartPoint.X;
            x2 = EndPoint.X;
            x3 = segment.StartPoint.X;
            x4 = segment.EndPoint.X;

            y1 = StartPoint.Y;
            y2 = EndPoint.Y;
            y3 = segment.StartPoint.Y;
            y4 = segment.EndPoint.Y;

            double den = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

            if (den != 0)
            {
                double t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / den;
                double u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / den;

                if (t > 0 && t < 1 && u > 0 && u < 1)
                {
                    output = new DekPoint(x1 + t * (x2 - x1), y1 + t * (y2 - y1));
                }
            }

            return output;
        }


    }
}