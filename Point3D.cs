using System;
using SplashKitSDK;

namespace _3D_Projection
{
    class Point3D
    {
        public double X;
        public double Y;
        public double Z;

        public Point3D() : this(0, 0, 0)
        {

        }

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3D Set(double x = 0, double y = 0, double z = 0)
        {
            X = x;
            Y = y;
            Z = z;
            return this;
        }

        public void Print()
        {
            Console.WriteLine(this);
        }

        public override string ToString()
        {
            return $"{X} {Y} {Z}";
        }

        public Point3D Mult(double val)
        {
            Point3D p = new Point3D
            {
                X = X * val,
                Y = Y * val,
                Z = Z * val
            };
            return p;
        }

        public Point3D Sub(Point3D p)
        {
            Point3D p2 = new Point3D();
            p2.X = X - p.X;
            p2.Y = Y - p.Y;
            p2.Z = Z - p.Z;
            return p2;
        }

        public Point3D Add(Point3D p)
        {
            Point3D p2 = new Point3D();
            p2.X = X + p.X;
            p2.Y = Y + p.Y;
            p2.Z = Z + p.Z;
            return p2;
        }

        public Point2D To2d()
        {
            Point2D center = SplashKit.ScreenCenter();
            return new Point2D { X = center.X + X, Y = center.Y + Y };
        }

        public Matrix3D ToMatrix()
        {
            Matrix3D m = new Matrix3D();
            m.Matrix = new double[,] {
                { X },
                { Y },
                { Z },
                { 1 }
            };
            return m;
        }

        public double MagSquared()
        {
            return X * X + Y * Y + Z * Z;
        }
    }
}
