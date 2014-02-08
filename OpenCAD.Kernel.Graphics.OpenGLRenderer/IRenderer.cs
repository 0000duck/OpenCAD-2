namespace OpenCAD.Kernel.Graphics.OpenGLRenderer
{
    public interface IOpenGLRenderer
    {
        void Update(ICamera camera);
        void Render();
    }
}