namespace PipeAndFilter.Models.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int PersonId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }
}