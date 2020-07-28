using System;

namespace PipeAndFilter.Models.Recieved
{
    public class RecievedOrder
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime DateJoined { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string OrderDescription { get; set; }
        public decimal OrderAmount { get; set; }
        public int OrderQuantity { get; set; }
        public decimal OrderSubtotal { get; set; }
    }
}