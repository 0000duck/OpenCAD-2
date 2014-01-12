using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Newtonsoft.Json;
using OpenCAD.Desktop.Commands;
using OpenCAD.Desktop.Models;

namespace OpenCAD.Desktop.Misc
{
    public class ProjectManager : IHandle<OpenProjectCommand>, IHandle<AddPartCommand>
    {
        private readonly IEventAggregator _eventAggregator;

        public IProjectModel Project { get; private set; }

        public ProjectManager(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void Handle(OpenProjectCommand message)
        {
            var json = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(message.FileName));
            var dir = Path.GetDirectoryName(message.FileName);

            var t = json.Items;
            Project = new ProjectModel()
                {
                    Name = json.Name,
                    Directory = dir,
                    References = new ObservableCollection<string>( ((object)json.References).Select(p => p.ToString()).Cast<string>()),
                    Items = new ObservableCollection<ItemModel>(
                        ((object)json.Items).Select(p => new ItemModel
                        {
                            Name = Path.GetFileName(p.FilePath.ToString()),
                            FilePath = Path.GetFullPath(Path.Combine(dir, p.FilePath.ToString())),
                            Contents = File.ReadAllText(Path.GetFullPath(Path.Combine(dir, p.FilePath.ToString())))
                        }).Cast<ItemModel>())
                };



            _eventAggregator.Publish(new ProjectOpenedEvent(Project));
        }

        public void Handle(AddPartCommand message)
        {

        }
    }
}
