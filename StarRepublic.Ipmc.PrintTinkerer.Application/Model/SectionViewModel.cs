using StarRepublic.Ipmc.PrintTinkerer.Core.CcrService;

namespace StarRepublic.Ipmc.PrintTinkerer.Application.Model
{
    public class SectionViewModel : HierarchicalEntity
    {
        public StructureObject StructureObject { get; }
        public string SectionId => StructureObject.SectionId;
        public int Id => StructureObject.Id;
        public override string Name => StructureObject.Name;

        public SectionViewModel(StructureObject section)
        {
            StructureObject = section;
            Children = new[] { new LoadingDummy() };
        }
    }
}