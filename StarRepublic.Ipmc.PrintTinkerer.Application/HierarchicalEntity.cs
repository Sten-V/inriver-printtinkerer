using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StarRepublic.Ipmc.PrintTinkerer.Application
{
    public class HierarchicalEntity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IEnumerable<HierarchicalEntity> _children;

        public IEnumerable<HierarchicalEntity> Children
        {
            get => _children;
            set
            {
                _children = value;
                OnPropertyChanged();
            }
        }

        public virtual string Name => string.Empty;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}