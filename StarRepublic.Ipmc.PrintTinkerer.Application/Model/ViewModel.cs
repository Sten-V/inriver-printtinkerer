using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StarRepublic.Ipmc.PrintTinkerer.Application.Model
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IEnumerable<PublicationViewModel> _publications;
        private IEnumerable<AttributeSetViewModel> _attributeSets;
        private IEnumerable<ObjectTemplateViewModel> _objectTemplates;
        private IEnumerable<EditionViewModel> _editions;

        public IEnumerable<PublicationViewModel> Publications
        {
            get => _publications;
            set
            {
                _publications = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<AttributeSetViewModel> AttributeSets
        {
            get => _attributeSets;
            set
            {
                _attributeSets = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PrintObjects));
            }
        }

        public IEnumerable<ObjectTemplateViewModel> ObjectTemplates
        {
            get => _objectTemplates;
            set
            {
                _objectTemplates = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PrintObjects));
            }
        }

        public IEnumerable<EditionViewModel> Editions
        {
            get => _editions;
            set
            {
                _editions = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<PrintObject> PrintObjects
        {
            get
            {
                if (AttributeSets == null && ObjectTemplates == null)
                    return null;
                if (AttributeSets == null)
                    return ObjectTemplates;
                if (ObjectTemplates == null)
                    return AttributeSets;

                return ObjectTemplates.Union((IEnumerable<PrintObject>)AttributeSets);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}