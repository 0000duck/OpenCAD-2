using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;
using SharpGL.SceneGraph.Shaders;

namespace OpenCAD.Kernel.Graphics.OpenGLRenderer.Buffers
{
   

    public interface IBuffer:IBindable
    {

    }

    public class ShaderBindable:IBindable
    {
        protected readonly OpenGL _gl;
        protected ShaderProgram _program;

        public ShaderBindable(OpenGL gl)
        {
            _gl = gl;
        }


        public void Bind()
        {
            _program.Push(_gl, null);
        }

        public void UnBind()
        {
            _program.Pop(_gl, null);
        }
    }
}
