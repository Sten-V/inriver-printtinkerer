using StarRepublic.Ipmc.PrintTinkerer.Core.CcrService;

namespace StarRepublic.Ipmc.PrintTinkerer.Application.Model
{
    public class AttributeSetViewModel : PrintObject
    {
        public AttributeSet AttributeSet { get; }

        public long Id => AttributeSet.id.Value;
        public string Name => AttributeSet.name;
        public string AttributeSetType => AttributeSet.type;

        public AttributeSetViewModel(AttributeSet attributeSet)
        {
            AttributeSet = attributeSet;
        }
    }
}