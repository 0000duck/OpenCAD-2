using System;
using OpenCAD.Kernel.Geometry;
using OpenCAD.Kernel.Maths;

namespace OpenCAD.Kernel.Topology
{
    public class Edge
    {
        private readonly ICurve _curve;
        public Vect3 Start { get { return Eval(0); } }
        public Vect3 End { get { return Eval(1); } }

        public Edge(ICurve curve)
        {
            _curve = curve;
        }

        public Vect3 Eval(double t)
        {
            throw new NotImplementedException();
        }
    }
}