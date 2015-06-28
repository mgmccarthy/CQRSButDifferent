using System;
using System.Linq;
using CQRSButDifferent.Data;
using CQRSButDifferent.Messages.Commands;
using NServiceBus;
using NServiceBus.Logging;

namespace CQRSButDifferent.SnapshotEndpoint
{
    public class CreateSnapshotForProductQuantityHandler : IHandleMessages<CreateSnapshotForProductQuantity>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CreateSnapshotForProductQuantityHandler));

        public void Handle(CreateSnapshotForProductQuantity message)
        {
            using (var context = new CqrsButDifferentContext())
            {
                Log.Warn("Creating snapshot.");

                //stamp out "one minute ago"
                var oneMinuteAgo = DateTime.Now.AddMinutes(-1);

                //get the prouduct quantity as of one minute ago
                var productQuantity = context.ProductQuantity.Where(x => x.ProductId == 1 && x.TimeStamp <= oneMinuteAgo).Sum(x => x.Delta);

                //delete all records less than or equal to one minute ago
                context.ProductQuantity.RemoveRange(context.ProductQuantity.Where(x => x.ProductId == 1 && x.TimeStamp <= oneMinuteAgo).ToList());

                //insert the new delta based on the product quantity at one minute ago
                context.ProductQuantity.Add(new ProductQuantity { ProductId = 1, Delta = productQuantity, TimeStamp = DateTime.Now });

                context.SaveChanges();
                Log.Warn("Snapshot Created.");
            }
        }
    }
}
