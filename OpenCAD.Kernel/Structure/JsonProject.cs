using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace OpenCAD.Kernel.Structure
{
    public class JsonProject:BaseProject
    {
        public JsonProject(string path)
        {
            var json = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(path));
            if (json == null) throw new Exception("Error Loading JSON Project");
            Directory = Path.GetDirectoryName(path);
            Name = json.Name;
            Items = ((object)json.Items).DynamicSelect(item => new PartProjectItem(Path.Combine(Directory,item.FilePath.ToString()))).Cast<IProjectItem>();
            References = ((object)json.References).DynamicSelect(p => p.ToString()).Cast<string>();
        }

        public override void Save()
        {
            throw new Exception();
        }
    }
}