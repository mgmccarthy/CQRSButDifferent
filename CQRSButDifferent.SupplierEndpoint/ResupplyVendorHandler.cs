using System;
using CQRSButDifferent.Data;
using CQRSButDifferent.Messages.Commands;
using NServiceBus;
using NServiceBus.Logging;

namespace CQRSButDifferent.SupplierEndpoint
{
    public class ResupplyVendorHandler : IHandleMessages<ResupplyVendor>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ResupplyVendorHandler));

        public void Handle(ResupplyVendor message)
        {
            using (var context = new CqrsButDifferentContext())
            {
                context.ProductQuantity.Add(new ProductQuantity { ProductId = message.ProductId, Delta = message.Quantity, TimeStamp = DateTime.Now });
                context.SaveChanges();
                Log.Warn("Vendor Resupplied.");
            }
        }
    }
}
