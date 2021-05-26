using System;
using System.Linq;
using StarRepublic.Ipmc.PrintTinkerer.Core.CcrService;

namespace StarRepublic.Ipmc.PrintTinkerer.Application.Model
{
    public class ArticleViewModel : HierarchicalEntity
    {
        public Article Article { get; }
        public string Identifier => Article.identifier;
        public override string Name => Article.name;
        public string SectionId { get; }

        public bool IsLinkType
        {
            get
            {
                var parts = Identifier.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                return parts.Last().StartsWith("Link_");
            }
        }

        public ArticleViewModel(Article article, string sectionId)
        {
            Article = article;
            SectionId = sectionId;
            Children = new[] { new LoadingDummy() };
        }
    }
}