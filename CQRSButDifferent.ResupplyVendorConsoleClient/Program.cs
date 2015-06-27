using System;
using CQRSButDifferent.Messages.Commands;

namespace CQRSButDifferent.ResupplyVendorConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBus.Init();

            Console.WriteLine("Press 'Enter' to resupply the vendor with 100 of product 1. To exit, Ctrl + C");

            while (Console.ReadLine() != null)
            {
                ServiceBus.Bus.Send(new ResupplyVendor { ProductId = 1, Quantity = 100 });
                Console.WriteLine("Vendor resupplied.");
            }
        }
    }
}
