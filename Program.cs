using System;
using SplashKitSDK;

namespace _3D_Projection
{
    public class Program
    {
        const int RENDER_DISTANCE = 25;

        public static void Main()
        {
            Camera cam = new Camera();
            Player player = new Player(cam);
            player.Position = new Point3D(0, Map.getYAt(0, 0), 0);
            player.Rotation.X = -30;

            Map.Init();

            new Window("3D Projection", 1280, 720);
            do
            {
                SplashKit.ProcessEvents();
                SplashKit.ClearScreen(Color.Black);

                player.Update();

                Matrix3D rotation = Matrix3D.GetRotation(cam.Rotation);
                Matrix3D translation = Matrix3D.GetTranslation(cam.Position.Mult(-1));

                Point2D[,] points = new Point2D[RENDER_DISTANCE * 2, RENDER_DISTANCE * 2];

                for (int i = 0; i < RENDER_DISTANCE * 2; i++)
                {
                    for (int j = 0; j < RENDER_DISTANCE * 2; j++)
                    {
                        Point3D p = new Point3D();
                        p.X = Math.Ceiling(cam.Position.X) + i - RENDER_DISTANCE;
                        p.Z = Math.Ceiling(cam.Position.Z) + j - RENDER_DISTANCE;
                        p.Y = Map.getYAt(p.X, p.Z);

                        Point3D rotated = translation.Multiply(p);
                        rotated = rotation.Multiply(rotated);

                        Matrix3D projection = Matrix3D.GetProjection(rotated);
                        Point3D projected3d = projection.Multiply(rotated);
                        projected3d = projected3d.Mult(400);

                        double z = projection.Matrix[0, 0];

                        if (z <= 0)
                        {
                            Point2D p2 = new Point2D();
                            p2.X = -9999;
                            points[i, j] = p2;
                            continue;
                        }

                        Point2D projected2d = projected3d.To2d();
                        points[i, j] = projected2d;

                        //double radius = 2; //projection.Matrix[0, 0] * 40;
                        //if (IsCircleOnScreen(projected2d, radius))
                        //{
                        //    SplashKit.FillCircle(Color.Black, projected2d.X, projected2d.Y, radius);
                        //}
                    }
                }

                for (int i = 0; i < RENDER_DISTANCE * 2 - 1; i++)
                {
                    for (int j = 0; j < RENDER_DISTANCE * 2 - 1; j++)
                    {
                        if (points[i, j].X == -9999 || points[i, j + 1].X == -9999 || points[i + 1, j].X == -9999 || points[i + 1, j + 1].X == -9999)
                        {
                            continue;
                        }

                        Quad q = new Quad();
                        q.Points = new Point2D[] {
                            points[i, j],
                            points[i, j + 1],
                            points[i + 1, j],
                            points[i + 1, j + 1]
                        };

                        Line l = new Line();
                        l.StartPoint = points[i, j + 1];
                        l.EndPoint = points[i + 1, j];

                        SplashKit.DrawQuad(Color.White, q);
                        SplashKit.DrawLine(Color.White, l);
                    }
                }

                SplashKit.RefreshScreen();
            } while (!SplashKit.WindowCloseRequested("3D Projection"));

            bool IsCircleOnScreen(Point2D p, double r)
            {
                Rectangle rect = new Rectangle();
                rect.X = p.X - r;
                rect.Y = p.Y - r;
                rect.Width = r * 2;
                rect.Height = r * 2;
                return SplashKit.RectOnScreen(rect);
            }
        }
    }

    public static class NumericExtensions
    {
        public static double ToRadians(this double val)
        {
            return (Math.PI / 180) * val;
        }

        public static int ToInt(this bool b)
        {
            return b ? 1 : 0;
        }
    }

    public static class Vector2DExtensions
    {
        public static void Normalize(this Vector2D v)
        {
            double mag = v.Mag();
            if (mag == 0)
            {
                v.Mult(0);
            }
            else
            {
                v.Div(mag);
            }
        }

        public static double Mag(this Vector2D p)
        {
            return Math.Sqrt(p.MagSquared());
        }

        public static double MagSquared(this Vector2D p)
        {
            return Math.Pow(p.X, 2) + Math.Pow(p.Y, 2);
        }

        public static void Div(this Vector2D v, double val)
        {
            if (val == 0)
            {
                v.Mult(0);
            }
            else
            {
                v.X /= val;
                v.Y /= val;
            }
        }

        public static void Mult(this Vector2D v, double val)
        {
            v.X *= val;
            v.Y *= val;
        }

        public static double AngleRadians(this Vector2D v)
        {
            if (v.Mag() == 0)
            {
                return 0;
            }
            return Math.Atan2(v.Y, v.X);
        }
    }

    public static class Map
    {
        private static FastNoise _noise = new FastNoise();

        public static void Init()
        {
            _noise.SetNoiseType(FastNoise.NoiseType.SimplexFractal);
        }

        public static double getYAt(double x, double z)
        {
            float freq = 3;
            float amp = 10;
            float y = _noise.GetNoise((float)x * freq, (float)z * freq);
            return y * amp;
        }
    }
}