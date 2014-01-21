using System.Collections.Generic;

namespace OpenCAD.Kernel.Structure
{
    public abstract class BaseProject:IProject
    {
        public string Name { get; protected set; }
        public string Directory { get; protected set; }
        public IEnumerable<IProjectItem> Items { get; protected set; }
        public IEnumerable<string> References { get; protected set; }
        public abstract void Save();
    }
}