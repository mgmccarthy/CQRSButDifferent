using System;
using System.Linq;
using CQRSButDifferent.Data;
using CQRSButDifferent.Messages.Commands;
using CQRSButDifferent.Messages.Events;
using NServiceBus;
using NServiceBus.Logging;

namespace CQRSButDifferent.OrderEndpoint
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        private readonly IBus bus;
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlaceOrderHandler));

        public PlaceOrderHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(PlaceOrder message)
        {
            using (var context = new CqrsButDifferentContext())
            {
                //sum(delta) here to determine if there is enough quantity of the product in to place the order
                var productQuantity = context.ProductQuantity.Where(x => x.ProductId == message.ProductId).Sum(x => x.Delta);

                if ((productQuantity - 10) < message.Quantity)
                    Log.Warn("We have less than 10 of product 1 remaining. Please resupply the vendor.");

                if (productQuantity < message.Quantity)
                {
                    //don't allow the order to proceed, publish InsuffcientProductQuantityForOrder
                    bus.Publish(new InsuffcientProductQuantityForOrder { OrderId = message.OrderId, ProductId = message.ProductId, Quantity = message.Quantity });
                    Log.Error("We are out of product 1. Please resupply the vendor.");
                }
                else
                {
                    //every time someone makes a purchase, we insert a new record with the negative of the quantity of the product they purchased
                    //Math.Abs(message.Quantity) * (-1) negates the Quantity on PlaceOrder
                    var negativeQuantityForOrder = Math.Abs(message.Quantity) * (-1);
                    context.ProductQuantity.Add(new ProductQuantity { ProductId = message.ProductId, Delta = negativeQuantityForOrder, TimeStamp = DateTime.Now });
                    Log.Info("Order placed.");
                }
                context.SaveChanges();
            }
        }
    }
}
