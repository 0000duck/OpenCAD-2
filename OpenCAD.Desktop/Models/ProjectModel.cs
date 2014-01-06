using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using OpenCAD.Desktop.Misc;

namespace OpenCAD.Desktop.Models
{
    public class ProjectModel : PropertyChangedBase, IProjectModel
    {
        public string Name { get; private set; }

        IReadOnlyObservableCollection<IPartModel> IProjectModel.Parts
        {
            get { return _readOnlyParts; }
        }

        private ObservableCollection<PartModel> _parts;
        private IReadOnlyObservableCollection<IPartModel> _readOnlyParts;

        public ObservableCollection<PartModel> Parts
        {
            get { return _parts; }
            set
            {
                if (Equals(value, _parts)) return;
                _parts = value;
                _readOnlyParts = value.WrapReadOnly<PartModel, IPartModel>();
                NotifyOfPropertyChange(() => Parts);
            }
        }
    }

    public class PartModel : PropertyChangedBase, IPartModel
    {
        public string Name { get; private set; }
    }

    public interface IProjectModel : INotifyPropertyChanged
    {
        string Name { get; }

        IReadOnlyObservableCollection<IPartModel> Parts { get; }
    }

    public interface IPartModel : INotifyPropertyChanged
    {
        string Name { get; }

    }
}
