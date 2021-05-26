using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using StarRepublic.Ipmc.PrintTinkerer.Core.CcrService;

namespace StarRepublic.Ipmc.PrintTinkerer.Core
{
    public static class CcrServiceHelper
    {
        public static async Task<CCRServiceClient> GetCcrServiceAsync(string url, string username, string password, string environment)
        {
            var ticket = await GetTicketAsync(url, username, password, environment);

            var eab = new EndpointAddressBuilder(new EndpointAddress(url));
            eab.Headers.Add(
                AddressHeader.CreateAddressHeader(
                    "inriverAuthTicket",
                    string.Empty,
                    ticket.ticketSignature));

            var ccrService = new CCRServiceClient("inRiver_ICCRService");
            ccrService.Endpoint.Address = eab.ToEndpointAddress();
            ccrService.Endpoint.EndpointBehaviors.Add(new LogBehavior());
            return ccrService;
        }

        private static async Task<getAuthenticationTicketResponse> GetTicketAsync(string url, string username, string password, string environment)
        {
            var authenticationTicketRequest = new getAuthenticationTicketRequest(username, password, environment);

            using (var ccrService = new CCRServiceClient("inRiver_ICCRService"))
            {
                ccrService.Endpoint.Address = new EndpointAddress(url);
                ccrService.Endpoint.EndpointBehaviors.Add(new LogBehavior());
                return await ccrService.GetAuthenticationTicketAsync(authenticationTicketRequest);
            }
        }
    }
}
