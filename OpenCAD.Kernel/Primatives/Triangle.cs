using OpenCAD.Kernel.Maths;

namespace OpenCAD.Kernel.Primatives
{
    public class Triangle
    {
        public Vect3 P1 { get; set; }
        public Vect3 P2 { get; set; }
        public Vect3 P3 { get; set; }
        public Vect3 Normal { get; set; }
    }
}
