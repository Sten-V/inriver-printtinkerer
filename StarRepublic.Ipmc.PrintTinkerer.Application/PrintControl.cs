using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using StarRepublic.Ipmc.PrintTinkerer.Application.Model;
using StarRepublic.Ipmc.PrintTinkerer.Core;
using StarRepublic.Ipmc.PrintTinkerer.Core.CcrService;

namespace StarRepublic.Ipmc.PrintTinkerer.Application
{
    internal class PrintControl
    {
        private readonly ViewModel _viewModel;
        private CCRServiceClient _ccrService;

        public PrintControl(ViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public async Task ConnectAndInitialize(string url, string userName, string password, string environment)
        {
            _ccrService = await CcrServiceHelper.GetCcrServiceAsync(url, userName, password, environment);
            await PopulateViewModelAsync();
        }

        internal async Task PopulateViewModelAsync()
        {
            await LoadPublications();
            await LoadAttributeSets();
            await LoadObjectTemplates();
            await LoadEditions();
        }

        private async Task LoadPublications()
        {
            var publications = await _ccrService.getSectionsAsync(0);

            _viewModel.Publications = publications
                .Select(p => new PublicationViewModel(p));
        }

        private async Task LoadAttributeSets()
        {
            var attributeSets = await _ccrService.getAllAttributeSetsAsync();

            _viewModel.AttributeSets = attributeSets
                .OrderBy(a => a.type)
                .ThenBy(a => a.name)
                .Select(a => new AttributeSetViewModel(a));
        }

        private async Task LoadObjectTemplates()
        {
            var objectTemplates = await _ccrService.getAllObjectTemplatesAsync();

            _viewModel.ObjectTemplates = objectTemplates
                .OrderBy(ot => ot.name)
                .Select(ot => new ObjectTemplateViewModel(ot));
        }

        private async Task LoadEditions()
        {
            var editions = await _ccrService.getAllEditionsAsync();

            _viewModel.Editions = editions
                .Select(e => new EditionViewModel(e));
        }

        private async Task<IEnumerable<HierarchicalEntity>> GetSections(int entityId)
        {
            var articles = await _ccrService.getSectionsAsync(entityId);
            return articles.Select(so => (HierarchicalEntity)new SectionViewModel(so));
        }

        private async Task<IEnumerable<HierarchicalEntity>> GetArticles(string sectionId, string identifier)
        {
            var articles = await _ccrService.getArticlesAsync(sectionId, identifier);
            return articles.Select(a => (HierarchicalEntity)new ArticleViewModel(a, sectionId));
        }

        public async Task LoadChildren(object hierarchicalEntity)
        {
            switch (hierarchicalEntity)
            {
                case PublicationViewModel publication:
                    publication.Children = await GetSections(publication.Id);
                    break;
                case SectionViewModel section:
                    var sections = await GetSections(section.Id);
                    var articles = await GetArticles(section.SectionId, null);
                    section.Children = sections.Union(articles);
                    break;
                case ArticleViewModel article:
                    article.Children = await GetArticles(article.SectionId, article.Identifier);
                    break;
            }
        }

        public async Task<string> GetPrintObjectText(PrintObject printObject, EditionViewModel edition, ArticleViewModel article)
        {
            if (printObject is AttributeSetViewModel attributeSet)
            {
                return await GetAttributeSetText(attributeSet, edition, article);
            }
            if (printObject is ObjectTemplateViewModel objectTemplate)
            {
                return await GetObjectTemplateText(objectTemplate, edition, article);
            }

            throw new ArgumentException("Unknown type", nameof(printObject));
        }

        private async Task<string> GetAttributeSetText(AttributeSetViewModel attributeSet, EditionViewModel edition, ArticleViewModel article)
        {
            var attributes = await _ccrService.getAttributesAsync(
                new[] { article.Article },
                edition.Id,
                attributeSet.AttributeSet,
                true,
                true,
                false,
                new ArticleIdResourceFileId[0],
                true);

            var result = new StringBuilder();

            foreach (var attribute in attributes.getAttributesReturn[0].attributes)
            {
                result.Append(attribute.textValue?.asString);
            }

            return result.ToString();
        }

        private async Task<string> GetObjectTemplateText(ObjectTemplateViewModel objectTemplate, EditionViewModel edition, ArticleViewModel article)
        {
            var ot = await _ccrService.getObjectTemplateAsync(objectTemplate.Id);

            var objectTemplateData = Encoding.UTF8.GetString(ot.data);
            var objectTemplateXml = XDocument.Parse(objectTemplateData);

            var attributeSets = objectTemplateXml
                .Descendants("XMLElement")
                .Where(e => e.Attribute("MarkupTag").Value == "XMLTag/CCR_Article");

            var result = new StringBuilder();

            foreach (var attributeSet in attributeSets)
            {
                await HandleObjectTemplateAttributeSet(edition, article, attributeSet, result);
            }

            result.AppendLine();
            result.AppendLine("--- Template XML");
            result.Append(objectTemplateXml);

            return result.ToString();
        }

        // TODO: Is the edition from the template xml or the UI selected edition to be used?
        private async Task HandleObjectTemplateAttributeSet(
            EditionViewModel edition,
            ArticleViewModel article,
            XElement attributeSet,
            StringBuilder result)
        {
            var attributeSetId = long.Parse(attributeSet
                .Elements("XMLAttribute")
                .First(e => e.Attribute("Name").Value == "AttributeSet")
                .Attribute("Value").Value);

            var attributes = await _ccrService.getAttributesAsync(
                new[] { article.Article },
                edition.Id,
                new AttributeSet { id = attributeSetId },
                true,
                true,
                false,
                new ArticleIdResourceFileId[0],
                true);

            var attributeSetName = _viewModel.AttributeSets
                                       .FirstOrDefault(a => a.Id == attributeSetId)?.Name
                                   ?? "WARNING: AttributeSet no longer exists in iPMC";

            result.AppendLine($"--- AttributeSet {attributeSetId}: {attributeSetName}");

            foreach (var attribute in attributes.getAttributesReturn[0].attributes)
            {
                result.Append(attribute.textValue?.asString);
            }

            result.AppendLine();
            result.AppendLine();
        }
    }
}