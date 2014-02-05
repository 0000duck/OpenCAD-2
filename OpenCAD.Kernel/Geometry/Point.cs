using OpenCAD.Kernel.Maths;

namespace OpenCAD.Kernel.Geometry
{
    public class Point
    {
        public Vect3 Position { get; private set; }
        public Point(Vect3 position)
        {
            Position = position;
        }
    }
}