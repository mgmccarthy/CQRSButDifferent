using System;
using CQRSButDifferent.Data;
using CQRSButDifferent.Messages.Commands;
using NServiceBus;

namespace CQRSButDifferent.SupplierEndpoint
{
    public class ResupplyVendorHandler : IHandleMessages<ResupplyVendor>
    {
        public void Handle(ResupplyVendor message)
        {
            using (var context = new CqrsButDifferentContext())
            {
                context.ProductQuantity.Add(new ProductQuantity { ProductId = message.ProductId, Delta = message.Quantity, TimeStamp = DateTime.Now });
                context.SaveChanges();
                Console.WriteLine("Vendor Resupplied.");
            }
        }
    }
}
