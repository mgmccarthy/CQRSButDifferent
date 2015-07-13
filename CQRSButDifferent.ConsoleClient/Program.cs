using System;
using System.Threading;
using CQRSButDifferent.Messages.Commands;

namespace CQRSButDifferent.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBus.Init();

            Console.WriteLine("Press 'Enter' to start placing orders. To exit, Ctrl + C");

            while (Console.ReadLine() != null)
            {
                var random = new Random();
                for (var i = 1; i <= int.MaxValue; i++)
                {
                    var productQuantity = random.Next(1, 6);
                    ServiceBus.Bus.Send(new PlaceOrder { OrderId = i, ProductId = 1, Quantity = productQuantity });
                    Console.WriteLine("An order with {0} of product 1 has been placed.", productQuantity);
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
