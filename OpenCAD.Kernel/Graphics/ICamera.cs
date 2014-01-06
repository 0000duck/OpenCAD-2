using OpenCAD.Kernel.Maths;

namespace OpenCAD.Kernel.Graphics
{
    public interface ICamera
    {
        Mat4 Model { get; set; }
        Mat4 View { get; set; }
        Mat4 Projection { get; set; }
        Mat4 MVP { get; }
        void Resize(int width, int height);
    }

    public abstract class BaseCamera:ICamera
    {
        public Mat4 Model { get; set; }
        public Mat4 View { get; set; }
        public Mat4 Projection { get; set; }
        public Mat4 MVP
        {
            get { return Projection * View * Model; }
        }
        public abstract void Resize(int width, int height);

        public double Near { get; protected set; }
        public double Far { get; protected set; }
    }

    public class NullCamera:BaseCamera
    {
        public override void Resize(int width, int height)
        {
            
        }
    }
}