using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCAD.Kernel.Modeling;
using OpenCAD.Kernel.Modeling.Octree;

namespace OpenCAD.Kernel.Scripting
{
    public interface IPartScript
    {
        IEnumerable<IPartScriptOption> Options();
        IModel Generate();
    }

    public interface IPartScriptOption
    {
        
    }

    public class TestPart:IPartScript
    {
        public IEnumerable<IPartScriptOption> Options()
        {
            throw new NotImplementedException();
        }

        public IModel Generate()
        {
            throw new NotImplementedException();
        }
    }
}
