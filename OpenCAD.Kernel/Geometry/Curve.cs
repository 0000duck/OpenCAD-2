using System;
using OpenCAD.Kernel.Maths;

namespace OpenCAD.Kernel.Geometry
{
    public interface ICurve
    {
        
    }

    public delegate Vect3 CurveEquation();

    public class Curve
    {
        private readonly CurveEquation _equation;
        public Curve(CurveEquation equation)
        {
            _equation = equation;
        }
    }

    public class StraightCurve:Curve
    {
        public StraightCurve() : base(() => Vect3.Zero)
        {

        }
    }

}