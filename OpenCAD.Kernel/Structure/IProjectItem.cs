using System.Collections.Generic;

namespace OpenCAD.Kernel.Structure
{
    public interface IProjectItem
    {
        string Name { get; }
        string Directory { get; }
        string Path { get; }
        IEnumerable<IProjectItem> Items { get; }
    }
}