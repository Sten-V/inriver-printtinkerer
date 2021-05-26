using StarRepublic.Ipmc.PrintTinkerer.Core.CcrService;

namespace StarRepublic.Ipmc.PrintTinkerer.Application.Model
{
    public class EditionViewModel
    {
        private readonly Edition _edition;

        public string Name => _edition.text;
        public long Id => _edition.id.Value;

        public EditionViewModel(Edition edition)
        {
            _edition = edition;
        }
    }
}