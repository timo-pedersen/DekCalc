using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DekCalc.Functions
{
    internal class Function<T, TResult> where T : struct
    {
        public String Name { get; init; }
        public Color Color { get; init; }
        public Func<T, double, double, double, double, double, TResult> TheFunction { get; init; }

        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
        public double D { get; set; }
        public double E { get; set; }

        public Function(Func<T, double, double, double, double, double, TResult> f, Color color, string name)
        {
            Color = color;
            Name = name;
            TheFunction = f;
        }

        public TResult Calculate(T t) => TheFunction(t, A, B, C, D, E);
    }
}
