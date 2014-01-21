using System.Collections.Generic;

namespace OpenCAD.Kernel.Structure
{
    public interface IProject
    {
        string Name { get; }
        string Directory { get; }
        IEnumerable<IProjectItem> Items { get; }
        IEnumerable<string> References { get; }
        void Save();
    }
}