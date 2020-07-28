namespace PipeAndFilter.Models.Entities
{
    public class Address
    {
        public int AddressId { get; set; }
        public int PersonId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
    }
}