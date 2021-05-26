using StarRepublic.Ipmc.PrintTinkerer.Core.CcrService;

namespace StarRepublic.Ipmc.PrintTinkerer.Application.Model
{
    public class PublicationViewModel : HierarchicalEntity
    {
        public StructureObject StructureObject { get; }
        public int Id => StructureObject.Id;
        public override string Name => StructureObject.Name;

        public PublicationViewModel(StructureObject publication)
        {
            StructureObject = publication;
            Children = new[] { new LoadingDummy() };
        }
    }
}