using OpenCAD.Kernel.Maths;

namespace OpenCAD.Kernel.Modeling.FileFormats.StereoLithography
{
    public struct STLTriangle
    {
        public Vect3 P1;
        public Vect3 P2;
        public Vect3 P3;
        public Vect3 Normal;
    }
}