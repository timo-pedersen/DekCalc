//using static System.Math;
using System.Numerics;
using static System.Numerics.Complex;

namespace DekCalcDummyNameSpace
{
    public class Functions
    {
        public static double PI => System.Math.PI;

        public System.Func<Complex, double, double, double, double, double, Complex> CreateComplexFunction()
        {
            return (x, A, B, C, D, E) => 
            Sin(x);

        }
    }
}

