using System;

namespace _3D_Projection
{
    class Camera
    {
        public Point3D Position;
        private Point3D _rotation;
        public Point3D Velocity;

        public Camera() : this(0, 0, 0)
        {
            
        }

        public Camera(double x, double y, double z)
        {
            Position = new Point3D(x, y, z);
            Rotation = new Point3D(0, 0, 0);
            Velocity = new Point3D(0, 0, 0);
        }

        public Point3D Rotation
        {
            get => _rotation;
            set
            {
                Point3D p = value;
                p.X = Math.Clamp(p.X, -90, 90);
                p.Y = p.Y % 360;
                p.Z = p.Z % 360;

                if (p.Y < 0)
                    p.Y += 360;
                if (p.Z < 0)
                    p.Z += 360;
                
                _rotation = p;
            }
        }

        public void Accelerate(Point3D p)
        {
            Velocity = Velocity.Add(p);
        }

        public void Accelerate(double x = 0, double y = 0, double z = 0)
        {
            Accelerate(new Point3D(x, y, z));
        }

        public void Move(Point3D p)
        {
            Position = Position.Add(p);
        }

        public void Move(double x = 0, double y = 0, double z = 0)
        {
            Move(new Point3D(x, y, z));
        }

        public void Rotate(Point3D p)
        {
            Rotation = Rotation.Add(p);
        }

        public void Rotate(double x = 0, double y = 0, double z = 0)
        {
            Rotate(new Point3D(x, y, z));
        }

        public void Update()
        {
            Velocity = Velocity.Mult(0.9);
            Move(Velocity);
            Position.Y = Map.getYAt(Position.X, Position.Z) + 1;

            //Point3D b = new Point3D();
            //for (int i = 0; i < 360; i++)
            //{
            //    double rad = ((double)i).ToRadians();
            //    double x = Position.X + Math.Sin(rad) / 1000;
            //    double z = Position.Z + Math.Cos(rad) / 1000;
            //    Point3D p = new Point3D(x, Map.getYAt(x, z), z);
            //    Point3D diff = p.Sub(Position);
            //    if (diff.Y < b.Y)
            //    {
            //        b = diff;
            //    }
            //}
            //b.Print();
            //Accelerate(b.X * Math.Pow(1, b.Y * 5) * 1, 0, b.Z * Math.Pow(1, b.Y * 5) * 1);
            //Velocity = Velocity.Mult(0.999);
            //Move(Velocity);
        }
    }
}
