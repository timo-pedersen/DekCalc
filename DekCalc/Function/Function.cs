using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DekCalc.Functions
{
    internal class Function<T, TResult> where T : struct
    {
        public String Name { get; init; }
        private Func<T, double, double, double, double, double, TResult> f { get; init; }

        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
        public double D { get; set; }
        public double E { get; set; }

        public Function(Func<T, double, double, double, double, double, TResult> f, string name)
        {
            Name = name;
            this.f = f;
        }

        public TResult Calculate(T t) => f(t, A, B, C, D, E);
    }
}
