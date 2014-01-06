using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCAD.Kernel.Modeling;
using OpenCAD.Kernel.Modeling.Octree;

namespace OpenCAD.Kernel.Scripting
{
    public interface IPartScript<T> where T : IModel
    {
        IEnumerable<IPartScriptOption> Options();
        T Generate();
    }

    public interface IPartScriptOption
    {
        
    }

    public class TestPart:IPartScript<OctreeModel>
    {
        public IEnumerable<IPartScriptOption> Options()
        {
            throw new NotImplementedException();
        }

        public OctreeModel Generate()
        {
            throw new NotImplementedException();
        }
    }
}
