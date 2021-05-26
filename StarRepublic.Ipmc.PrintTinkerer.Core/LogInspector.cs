using System.ServiceModel.Dispatcher;
using log4net;
using StarRepublic.Ipmc.PrintTinkerer.Core.CcrService;

namespace StarRepublic.Ipmc.PrintTinkerer.Core
{
    public class LogInspector : IParameterInspector
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CCRServiceClient));

        public object BeforeCall(string operationName, object[] inputs)
        {
            _log.Info($"Calling {operationName}({string.Join(", ", inputs)})");
            return null;
        }

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            _log.Info($"Finished {operationName}({string.Join(", ", outputs)})");
        }
    }
}