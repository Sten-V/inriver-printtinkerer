using StarRepublic.Ipmc.PrintTinkerer.Core.CcrService;

namespace StarRepublic.Ipmc.PrintTinkerer.Application
{
    public class ObjectTemplateViewModel : PrintObject
    {
        public ObjectTemplateLight ObjectTemplate { get; }
        
        public long Id => ObjectTemplate.id.Value;
        public string Name => ObjectTemplate.name;

        public ObjectTemplateViewModel(ObjectTemplateLight objectTemplate)
        {
            ObjectTemplate = objectTemplate;
        }
    }
}