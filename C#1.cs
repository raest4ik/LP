using System;
using Geometry; // Используем пространство имен Geometry

class Program
{
    static void Main()
    {
        var original = new Triple(123, -45, 67);
        var copy = new Triple(original);
        Console.WriteLine("Оригинал: " + original);
        Console.WriteLine("Копия: " + copy);
        Console.WriteLine("Минимальная последняя цифра: " + original.GetMinLastDigit());

        var point1 = new Point3D(1, 2, 3, "A");
        var point2 = new Point3D(point1);
        var point3 = new Point3D(4, 5, 6, "B");

        Console.WriteLine("\nТочка 1: " + point1);
        Console.WriteLine("Копия точки: " + point2);
        Console.WriteLine("Длина вектора: " + point1.GetMagnitude().ToString("F2"));
        Console.WriteLine("Нулевой вектор? " + new Point3D(0, 0, 0, "Zero").IsZeroVector());

        var cross = point1.CrossProduct(point3);
        Console.WriteLine("Векторное произведение: " + cross);
    }
}
