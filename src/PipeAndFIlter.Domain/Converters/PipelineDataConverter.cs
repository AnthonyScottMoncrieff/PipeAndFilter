using PipeAndFilter.Models;
using PipeAndFilter.Models.Entities;
using PipeAndFilter.Models.Recieved;
using PipeAndFIlter.Domain.Converters.Interfaces;

namespace PipeAndFIlter.Domain.Converters
{
    public class PipelineDataConverter : IModelConverter<RecievedOrder, PipelineData>
    {
        public PipelineData Convert(RecievedOrder model)
        {
            return new PipelineData
            {
                Address = new Address
                {
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    City = model.City,
                    Postcode = model.Postcode
                },
                Order = new Order
                {
                    Amount = model.OrderAmount,
                    Description = model.OrderDescription,
                    Quantity = model.OrderQuantity,
                    Subtotal = model.OrderSubtotal
                },
                Person = new Person
                {
                    Age = model.Age,
                    DateJoined = model.DateJoined,
                    Name = model.Name,
                }
            };
        }
    }
}