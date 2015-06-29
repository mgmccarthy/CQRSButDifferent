using System;
using System.Linq;
using CQRSButDifferent.Data;
using CQRSButDifferent.Messages.Commands;
using NServiceBus;

namespace CQRSButDifferent.SnapshotEndpoint
{
    public class CreateSnapshotOfProductQuantityHandler : IHandleMessages<CreateSnapshotOfProductQuantity>
    {
        public void Handle(CreateSnapshotOfProductQuantity message)
        {
            using (var context = new CqrsButDifferentContext())
            {
                //stamp out "one minute ago"
                var oneMinuteAgo = DateTime.Now.AddMinutes(-1);

                //get the prouduct quantity as of one minute ago
                var productQuantity = context.ProductQuantity.Where(x => x.ProductId == 1 && x.TimeStamp <= oneMinuteAgo).Sum(x => x.Delta);

                //delete all records less than or equal to one minute ago
                context.ProductQuantity.RemoveRange(context.ProductQuantity.Where(x => x.ProductId == 1 && x.TimeStamp <= oneMinuteAgo).ToList());

                //insert the new delta based on the product quantity at one minute ago
                context.ProductQuantity.Add(new ProductQuantity { ProductId = 1, Delta = productQuantity, TimeStamp = DateTime.Now });

                context.SaveChanges();
                Console.WriteLine("Snapshot Created.");
            }
        }
    }
}
