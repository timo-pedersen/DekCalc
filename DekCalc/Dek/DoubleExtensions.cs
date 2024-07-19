using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DekCalc.Dek;

public static class DoubleExtensions
{
    //https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-double
    public static bool IsApproximatelyEqual(double value1, double value2, double epsilon)
    {
        // If they are equal anyway, just return True.
        if (value1.Equals(value2))
            return true;

        // Handle NaN, Infinity.
        if (Double.IsInfinity(value1) | Double.IsNaN(value1))
            return value1.Equals(value2);
        else if (Double.IsInfinity(value2) | Double.IsNaN(value2))
            return value1.Equals(value2);

        // Handle zero to avoid division by zero
        double divisor = Math.Max(value1, value2);
        if (divisor.Equals(0))
            divisor = Math.Min(value1, value2);

        return Math.Abs((value1 - value2) / divisor) <= epsilon;
    }

    public static bool Equals(this double me, double value2, double epsilon) => IsApproximatelyEqual(me, value2, epsilon);
    public static bool Equals(this double me, float value2, double epsilon) => IsApproximatelyEqual(me, (double)value2, epsilon);
    public static bool Equals(this double me, Half value2, double epsilon) => IsApproximatelyEqual(me, (double)value2, epsilon);
    public static bool Equals(this double me, Decimal value2, double epsilon) => IsApproximatelyEqual(me, (double)value2, epsilon);
}
