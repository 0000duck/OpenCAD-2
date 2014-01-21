namespace OpenCAD.Kernel.Structure
{
    public interface IProjectCompilableItem:IProjectItem
    {
        string Load();
        void Save(string contents);
    }
}