using System;

namespace Geometry
{
    public class Triple
    {
        protected int a, b, c;

        public Triple(int a, int b, int c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public Triple(Triple source) : this(source.a, source.b, source.c)
        {
        }

        public int GetMinLastDigit()
        {
            int lastA = Math.Abs(a % 10);
            int lastB = Math.Abs(b % 10);
            int lastC = Math.Abs(c % 10);
            return Math.Min(Math.Min(lastA, lastB), lastC);
        }

        public override string ToString() => $"Triple [a: {a}, b: {b}, c: {c}]";
    }
}
