using PipeAndFilter.Models.Entities;

namespace PipeAndFilter.Models
{
    public class PipelineData
    {
        public Person Person { get; set; }
        public Order Order { get; set; }
        public Address Address { get; set; }
    }
}