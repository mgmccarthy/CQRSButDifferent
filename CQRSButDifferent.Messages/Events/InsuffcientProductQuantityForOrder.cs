namespace CQRSButDifferent.Messages.Events
{
    public class InsuffcientProductQuantityForOrder
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
