using System.Collections.Generic;

namespace OpenCAD.Kernel.Structure
{
    public abstract class BaseProjectItem : IProjectItem
    {
        public string Name { get { return System.IO.Path.GetFileName(Path); } }
        public string Directory { get { return System.IO.Path.GetDirectoryName(Path); } }
        public string Path { get; protected set; }
        public IEnumerable<IProjectItem> Items { get; protected set; }

        protected BaseProjectItem(string path)
        {
            Path = path;
            Items = new List<IProjectItem>();
        }
    }
}