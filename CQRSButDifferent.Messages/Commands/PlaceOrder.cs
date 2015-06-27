namespace CQRSButDifferent.Messages.Commands
{
    public class PlaceOrder
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
