using System;

namespace _3D_Projection
{
    class Matrix3D
    {
        public double[,] Matrix = new double[,] {
            { 1, 0, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 }
        };

        public void Print()
        {
            int cols = Matrix.GetLength(0);
            int rows = Matrix.GetLength(1);

            Console.WriteLine($"{cols}x{rows}");
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {                    
                    Console.Write(Matrix[i, j] + " ");
                }
                Console.WriteLine("");
            }
        }

        public Point3D ToPoint()
        {
            return new Point3D(
                Matrix[0, 0],
                Matrix[1, 0],
                Matrix[2, 0]
            );
        }

        public Point3D Multiply(Point3D p)
        {
            Matrix3D m = p.ToMatrix();
            Matrix3D r = Multiply(m);
            return r.ToPoint();
        }

        public Matrix3D Multiply(Matrix3D m)
        {
            int rowsA = Matrix.GetLength(0);
            int colsA = Matrix.GetLength(1);
            int rowsB = m.Matrix.GetLength(0);
            int colsB = m.Matrix.GetLength(1);

            if (colsA != rowsB)
            {
                Console.WriteLine("Columns of this matrix must match rows of m");
                return new Matrix3D();
            }

            Matrix3D result = new Matrix3D();
            for (int j = 0; j < rowsA; j++)
            {
                for (int i = 0; i < colsB; i++)
                {
                    double sum = 0;
                    for (int n = 0; n < colsA; n++)
                    {
                        sum += Matrix[j, n] * m.Matrix[n, i];
                    }
                    result.Matrix[j, i] = sum;
                }
            }
            return result;
        }

        public static Matrix3D GetRotationX(double angle)
        {
            double rad = angle.ToRadians();
            Matrix3D m = new Matrix3D();
            m.Matrix = new double[,] {
                { 1, 0, 0, 0 },
                { 0, Math.Cos(rad), -Math.Sin(rad), 0 },
                { 0, Math.Sin(rad), Math.Cos(rad), 0 },
                { 0, 0, 0, 1 }
            };
            return m;
        }

        public static Matrix3D GetRotationY(double angle)
        {
            double rad = angle.ToRadians();
            Matrix3D m = new Matrix3D();
            m.Matrix = new double[,] {
                { Math.Cos(rad), 0, Math.Sin(rad), 0 },
                { 0, 1, 0, 0 },
                { -Math.Sin(rad), 0, Math.Cos(rad), 0 },
                { 0, 0, 0, 1 }
            };
            return m;
        }

        public static Matrix3D GetRotationZ(double angle)
        {
            double rad = angle.ToRadians();
            Matrix3D m = new Matrix3D();
            m.Matrix = new double[,] {
                { Math.Cos(rad), -Math.Sin(rad), 0, 0 },
                { Math.Sin(rad), Math.Cos(rad), 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
            };
            return m;
        }

        public static Matrix3D GetRotation(Point3D p)
        {
            Matrix3D rotationX = GetRotationX(p.X);
            Matrix3D rotationY = GetRotationY(p.Y);
            Matrix3D rotationZ = GetRotationZ(p.Z);

            Matrix3D m = rotationY;
            m = rotationX.Multiply(m);
            m = rotationZ.Multiply(m);

            return m;
        }

        public static Matrix3D GetTranslation(Point3D t)
        {
            Matrix3D m = new Matrix3D();
            m.Matrix = new double[,] {
                { 1, 0, 0, t.X },
                { 0, 1, 0, t.Y },
                { 0, 0, 1, t.Z },
                { 0, 0, 0, 1 }
            };
            return m;
        }

        public static Matrix3D GetProjection(Point3D t)
        {
            double z = 1 / -t.Z;
            Matrix3D m = new Matrix3D();
            m.Matrix = new double[,] {
                { -z, 0, 0, 0 },
                { 0, z, 0, 0 },
                { 0, 0, -1, 0 }
            };
            return m;
        }
    }
}
