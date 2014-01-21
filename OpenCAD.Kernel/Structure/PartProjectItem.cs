using System.IO;
using OpenCAD.Kernel.Scripting;

namespace OpenCAD.Kernel.Structure
{
    public class PartProjectItem : BaseProjectItem, IProjectCompilableItem
    {
        public PartProjectItem(string path) : base(path)
        {
            
        }

        public string Load()
        {
            return File.ReadAllText(Path);
        }

        public void Save(string contents)
        {
            File.WriteAllText(Path, contents);
        }
    }
}