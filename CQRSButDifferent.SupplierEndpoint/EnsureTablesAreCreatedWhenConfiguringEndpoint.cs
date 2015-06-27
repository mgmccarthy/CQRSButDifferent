using CQRSButDifferent.Data;
using NServiceBus;
using NServiceBus.Config;

namespace CQRSButDifferent.SupplierEndpoint
{
    public class EnsureTablesAreCreatedWhenConfiguringEndpoint : IWantToRunWhenConfigurationIsComplete
    {
        public void Run(Configure config)
        {
            using (var context = new CqrsButDifferentContext())
                context.Database.Initialize(false);
        }
    }
}
