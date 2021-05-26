using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using StarRepublic.Ipmc.PrintTinkerer.Proxy.CcrService;

namespace StarRepublic.Ipmc.PrintTinkerer.Proxy
{
    public class CcrProxyService : ICCRService
    {
        public Article getArticle(string articleIdentifier)
        {
            Console.WriteLine($"{nameof(getArticle)}({articleIdentifier})");

            using (var client = GetClient())
            {
                var article = client.getArticle(articleIdentifier);
                return article;
            }
        }

        public getAttributeResponse getAttribute(getAttributeRequest request)
        {
            Console.WriteLine($"{nameof(getAttribute)}({request.attrSet.id}, {request.article.identifier})");

            using (var client = GetClient())
            {
                var attribute = client.getAttribute(
                    request.article,
                    request.editionId,
                    request.attrSet,
                    request.attributeIdentifier,
                    request.attributeValueIdentifier,
                    request.preview,
                    request.applyFormatters,
                    request.isHighRes,
                    request.resourceType,
                    request.resourceFileId,
                    request.onlyAttributeData);

                return new getAttributeResponse(attribute);
            }
        }

        public getAttributesResponse getAttributes(getAttributesRequest request)
        {
            Console.WriteLine($"{nameof(getAttributes)}({request.articles[0].identifier}, {request.editionId}, {request.attrSet.id}, {request.preview}, {request.applyFormatters}, {request.isHighRes}, {request.articleIdAndResourceFileId.FirstOrDefault()?.resourceFileId}, {request.onlyAttributeData})");

            using (var client = GetClient())
            {
                var attributes = client.getAttributes(
                    request.articles,
                    request.editionId,
                    request.attrSet,
                    request.preview,
                    request.applyFormatters,
                    request.isHighRes,
                    request.articleIdAndResourceFileId,
                    request.onlyAttributeData);

                return new getAttributesResponse(attributes);
            }
        }

        public AttributeSetImpl getAttributeSetImpl(AttributeSet attributeSet, Edition edition, Article article)
        {
            Console.WriteLine($"{nameof(getAttributeSetImpl)}({attributeSet.id}, {attributeSet.name}, {edition.text}, {article.name})");

            using (var client = GetClient())
            {
                var attributeSetImpl = client.getAttributeSetImpl(attributeSet, edition, article);
                return attributeSetImpl;
            }
        }

        public Edition[] getAllEditions()
        {
            Console.WriteLine($"{nameof(getAllEditions)}()");

            using (var client = GetClient())
            {
                var editions = client.getAllEditions();
                foreach (var edition in editions)
                {
                    Console.WriteLine($"\t{edition.id}, {edition.text}, {edition.language.code}");
                }

                return editions;
            }
        }

        public Article[] getArticles(string documentId, string parentArticleIdentifier)
        {
            Console.WriteLine($"{nameof(getArticles)}({documentId}, {parentArticleIdentifier})");

            using (var client = GetClient())
            {
                var articles = client.getArticles(documentId, parentArticleIdentifier);
                foreach (var article in articles)
                {
                    Console.WriteLine($"\t{article.identifier}, {article.name}, {article.hasChildren}");
                }

                return articles;
            }
        }

        public StructureObject[] searchSection(string searchString)
        {
            Console.WriteLine($"{nameof(searchSection)}({searchString})");

            using (var client = GetClient())
            {
                var structureObjects = client.searchSection(searchString);
                return structureObjects;
            }
        }

        public StructureObject[] getSections(int entityId)
        {
            Console.WriteLine($"{nameof(getSections)}({entityId})");

            using (var client = GetClient())
            {
                var sections = client.getSections(entityId);
                foreach (var section in sections)
                {
                    Console.WriteLine($"\t{section.Id}, {section.Name}, {section.SectionId}");
                }

                return sections;
            }
        }

        public DocumentEdition[] getDocumentEditions(string documentId)
        {
            throw new System.NotImplementedException();
        }

        public void updateLocalEditions()
        {
            throw new System.NotImplementedException();
        }

        public AttributeSet[] getAllAttributeSets()
        {
            Console.WriteLine($"{nameof(getAllAttributeSets)}()");

            using (var client = GetClient())
            {
                var attributeSets = client.getAllAttributeSets();
                foreach (var attributeSet in attributeSets)
                {
                    Console.WriteLine($"\t{attributeSet.id}, {attributeSet.name}, {attributeSet.type}");
                }

                return attributeSets;
            }
        }

        public searchArticlesResponse searchArticles(searchArticlesRequest request)
        {
            throw new System.NotImplementedException();
        }

        public ObjectTemplateLight[] getAllObjectTemplates()
        {
            Console.WriteLine($"{nameof(getAllObjectTemplates)}()");

            using (var client = GetClient())
            {
                var objectTemplates = client.getAllObjectTemplates();
                foreach (var objectTemplate in objectTemplates)
                {
                    Console.WriteLine($"\t{objectTemplate.id}, {objectTemplate.name}");
                }

                return objectTemplates;
            }
        }

        public ObjectTemplate getObjectTemplate(long templateId)
        {
            Console.WriteLine($"{nameof(getObjectTemplate)}({templateId})");

            using (var client = GetClient())
            {
                return client.getObjectTemplate(templateId);
            }
        }

        public string getPublicationStructure()
        {
            throw new System.NotImplementedException();
        }

        public getResourceFromFileIdResponse getResourceFromFileId(getResourceFromFileIdRequest request)
        {
            throw new System.NotImplementedException();
        }

        public addObjectTemplateResponse addObjectTemplate(addObjectTemplateRequest request)
        {
            throw new System.NotImplementedException();
        }

        public updateObjectTemplateResponse updateObjectTemplate(updateObjectTemplateRequest request)
        {
            throw new System.NotImplementedException();
        }

        public getAuthenticationEnabledStatusResponse GetAuthenticationEnabledStatus(GetAuthenticationEnabledStatusRequest request)
        {
            throw new System.NotImplementedException();
        }

        public getAuthenticationTicketResponse GetAuthenticationTicket(getAuthenticationTicketRequest request)
        {
            Console.WriteLine($"{nameof(GetAuthenticationTicket)}(\"{request.username}\", \"{request.password}\", \"{request.environment})\"");

            using (var client = GetClient())
            {
                var ticketSignature = client.GetAuthenticationTicket(request.username, request.password, request.environment,
                    out var environmentId, out var ticketSignature2, out var validTo);

                return new getAuthenticationTicketResponse(ticketSignature, environmentId, ticketSignature2, validTo);
            }
        }

        public long[] getUpdatedArticleAttributeTimestamps(string atricleId, string[] attributeIds)
        {
            throw new System.NotImplementedException();
        }

        private CCRServiceClient GetClient()
        {
            var authTicket = WebOperationContext.Current.IncomingRequest.Headers["inriverAuthTicket"];

            var client = new CCRServiceClient("inRiver_ICCRService");

            var eab = new EndpointAddressBuilder(client.Endpoint.Address);
            eab.Headers.Add(
                AddressHeader.CreateAddressHeader(
                    "inriverAuthTicket",
                    string.Empty,
                    authTicket));

            client.Endpoint.Address = eab.ToEndpointAddress();

            return client;
        }
    }
}