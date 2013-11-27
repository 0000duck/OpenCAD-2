namespace OpenCAD.Kernel.Maths
{
    public class AxisAngle
    {
        public Vect3 Axis { get; set; }
        public Angle Angle { get; set; }
        public AxisAngle(Vect3 axis, Angle angle)
        {
            Axis = axis;
            Angle = angle;

        }

        public override string ToString()
        {
            return "AxisAngle(Angle:{0},{1})".Format(Angle, Axis);
        }
    }
}