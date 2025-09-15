using System;

namespace Geometry
{
    public class Point3D : Triple
    {
        private string label;

        public Point3D(int x, int y, int z, string label) : base(x, y, z)
        {
            this.label = label;
        }

        public Point3D(Point3D source) : base(source)
        {
            label = source.label + "_copy";
        }

        public double GetMagnitude()
        {
            return Math.Sqrt(a * a + b * b + c * c);
        }

        public Point3D CrossProduct(Point3D other)
        {
            return new Point3D(
                b * other.c - c * other.b,
                c * other.a - a * other.c,
                a * other.b - b * other.a,
                $"{label}_cross_{other.label}"
            );
        }

        public bool IsZeroVector()
        {
            return a == 0 && b == 0 && c == 0;
        }

        public override string ToString() => $"{label} {base.ToString()}";
    }
}
