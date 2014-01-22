using System;
using OpenCAD.Kernel.Maths;

namespace OpenCAD.Kernel.Graphics
{
    public class OrthographicCamera:BaseCamera
    {
        public double Scale { get; set; }
        public Vect3 Eye { get; set; }
        public Vect3 Target { get; set; }
        public Vect3 Up { get; set; }
        private float _dist = 10;
        public OrthographicCamera()
        {
            Near = 1;
            Far = 40.0;
            Target = Vect3.Zero;
            Up = Vect3.UnitY;
            Eye = new Vect3(0, 0, -_dist);
            Model = Mat4.Identity;

            View = Mat4.LookAt(Eye, Target, Up);

            Projection = Mat4.Identity;
            Scale = 1;
        }
        public override void Resize(int width, int height)
        {
            Projection = Mat4.CreatePerspective(Math.PI / 4, width / (float)height, 1f, 2 * _dist);
            //Projection = Mat4.CreateOrthographic(-width / 2.0, width / 2.0, -height / 2.0, height / 2.0, Near, Far) * Mat4.Scale(Scale);
        }
    }
}