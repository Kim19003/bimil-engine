using Microsoft.Xna.Framework;

namespace Bimil.Engine.Models
{
    public class Circle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Radius { get; set; }
        public int Left => X;
        public int Right => X + Radius * 2;
        public int Top => Y;
        public int Bottom => Y + Radius * 2;
        public Point Center => new(X + Radius, Y + Radius);

        public Point Location
        {
            get
            {
                return new Point(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Point Size
        {
            get
            {
                return new Point(Radius * 2, Radius * 2);
            }
            set
            {
                Radius = value.X / 2;
            }
        }

        public Circle(int x, int y, int radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        public Circle(Point point, int radius)
        {
            X = point.X;
            Y = point.Y;
            Radius = radius;
        }
    }
}