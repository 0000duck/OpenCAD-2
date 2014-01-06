namespace OpenCAD.Desktop.Commands
{
    public class OpenProjectCommand
    {
        public string FileName { get; private set; }
        public OpenProjectCommand(string fileName)
        {
            FileName = fileName;
        }
        public override string ToString()
        {
            return "OpenProjectCommand: " + FileName;
        }
    }
}