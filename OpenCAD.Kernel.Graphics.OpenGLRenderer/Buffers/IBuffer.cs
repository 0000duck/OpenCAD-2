using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCAD.Kernel.Graphics.OpenGLRenderer.Buffers
{
   

    public interface IBuffer:IBindable
    {

    }

    public class FBO:IBuffer
    {
        public FBO()
        {

        }

        public void Bind()
        {
            throw new System.NotImplementedException();
        }

        public void UnBind()
        {
            throw new System.NotImplementedException();
        }
    }
}
