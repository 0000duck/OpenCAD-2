using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCAD.Kernel.Modeling;

namespace OpenCAD.Kernel.Graphics
{
    public interface IModelRenderManager
    {
        //void Add<TPart>(IModelRenderer<TPart> render);
        //Image Render(IModel model);
        IRenderer Fetch<T>();
        IRenderer Fetch(Type type);
        IRenderer Fetch(IModel model);
    }
    public abstract class BaseModelRenderManager:IModelRenderManager
    {
        public IRenderer Fetch<T>()
        {
            return Fetch(typeof (T));
        }

        public abstract IRenderer Fetch(Type type);
        public IRenderer Fetch(IModel model)
        {
            return Fetch(model.GetType());
        }
    }
}
