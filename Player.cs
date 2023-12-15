using System;
using SplashKitSDK;

namespace _3D_Projection
{
    class Player
    {
        Camera cam;

        public Point3D Position;
        private Point3D _rotation;
        public Point3D Velocity;

        private float _moveSpd = 0.02f;
        private float _mouseSens = 5.0f;
        private float _gravity = 0.015f;
        private float _jumpSpd = 0.3f;
        private bool _onGround = false;

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

        public Player(Camera c)
        {
            cam = c;
            Rotation = new Point3D(0, 0, 0);
            Velocity = new Point3D(0, 0, 0);
        }

        private void MoveCameraToPlayer()
        {
            cam.Position.X = Position.X;
            cam.Position.Y = Position.Y + 1;
            cam.Position.Z = Position.Z;
        }

        private void Movement()
        {
            Vector2D dir = new Vector2D();
            dir.X = SplashKit.KeyDown(KeyCode.DKey).ToInt() - SplashKit.KeyDown(KeyCode.AKey).ToInt();
            dir.Y = SplashKit.KeyDown(KeyCode.WKey).ToInt() - SplashKit.KeyDown(KeyCode.SKey).ToInt();

            if (dir.MagSquared() > 0)
            {
                double rad = cam.Rotation.Y.ToRadians() + dir.AngleRadians();
                Accelerate(
                    x: Math.Cos(rad) * _moveSpd,
                    z: Math.Sin(rad) * _moveSpd
                );
            }

            Velocity.X *= 0.9;
            Velocity.Z *= 0.9;

            double y = Map.getYAt(Position.X, Position.Z);
            if (Position.Y <= y || _onGround)
            {
                Position.Y = y;
                Velocity.Y = 0;
                _onGround = true;
            }
            else
            {
                Velocity.Y -= _gravity;
            }

            if (_onGround && SplashKit.KeyTyped(KeyCode.SpaceKey))
            {
                Velocity.Y = _jumpSpd;
                _onGround = false;
            }

            Move(Velocity);
        }

        private void Aiming()
        {
            if (!SplashKit.MouseDown(MouseButton.LeftButton))
            {
                Point2D center = SplashKit.ScreenCenter();
                Point2D mouse = SplashKit.MouseClicked(MouseButton.LeftButton) ? center : SplashKit.MousePosition();

                Vector2D movement = new Vector2D
                {
                    X = mouse.X - center.X,
                    Y = mouse.Y - center.Y
                };

                Rotate(-movement.Y / _mouseSens, -movement.X / _mouseSens, 0);

                SplashKit.HideMouse();
                SplashKit.MoveMouse(SplashKit.ScreenCenter());
            }
            else
            {
                SplashKit.ShowMouse();
            }

            cam.Rotation = _rotation;
        }

        public void Update()
        {
            Movement();
            Aiming();
            MoveCameraToPlayer();
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
    }
}
