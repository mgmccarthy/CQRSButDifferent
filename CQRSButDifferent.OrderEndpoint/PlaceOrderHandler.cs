using System;
using System.Linq;
using CQRSButDifferent.Data;
using CQRSButDifferent.Messages.Commands;
using NServiceBus;

namespace CQRSButDifferent.OrderEndpoint
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        public void Handle(PlaceOrder message)
        {
            using (var context = new CqrsButDifferentContext())
            {
                //sum(delta) here to determine if there is enough quantity of the product in to place the order
                var productQuantity = context.ProductQuantity.Where(x => x.ProductId == message.ProductId).Sum(x => x.Delta);

                if ((productQuantity < 20) && (productQuantity > 0))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("We have less than 20 of product 1 remaining. Resupply the vendor soon.");
                }
                
                if (productQuantity < 0)
                {
                    //bus.Publish(new InsuffcientProductQuantityForOrder { OrderId = message.OrderId, ProductId = message.ProductId, Quantity = message.Quantity });
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("We are out of product 1. Resupply the vendor now.");
                }
                else
                {
                    //every time someone makes a purchase, we insert a new record with the negative of the quantity of the product they purchased
                    //Math.Abs(message.Quantity) * (-1) negates the Quantity on PlaceOrder
                    var negativeQuantityForOrder = Math.Abs(message.Quantity) * (-1);
                    context.ProductQuantity.Add(new ProductQuantity { ProductId = message.ProductId, Delta = negativeQuantityForOrder, TimeStamp = DateTime.Now });
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Order placed.");
                }
                context.SaveChanges();
            }
        }
    }
}
