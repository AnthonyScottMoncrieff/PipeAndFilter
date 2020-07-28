using AutoFixture;
using NUnit.Framework;
using PipeAndFilter.Models.Recieved;
using PipeAndFIlter.Domain.Converters;

namespace PipeAndFilter.Tests.Unit
{
    [TestFixture]
    public class PipelineDataConverterTests
    {
        private Fixture _fixture;
        private PipelineDataConverter _pipelineDataConverter;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _pipelineDataConverter = new PipelineDataConverter();
        }

        [Test]
        public void Converter_Should_Convert_Order_Correctly()
        {
            //Arrange
            var recievedOrder = _fixture.Create<RecievedOrder>();

            //Act
            var result = _pipelineDataConverter.Convert(recievedOrder);

            //Assert
            Assert.AreEqual(result.Address.AddressLine1, recievedOrder.AddressLine1);
            Assert.AreEqual(result.Address.AddressLine2, recievedOrder.AddressLine2);
            Assert.AreEqual(result.Address.City, recievedOrder.City);
            Assert.AreEqual(result.Address.Postcode, recievedOrder.Postcode);

            Assert.AreEqual(result.Order.Amount, recievedOrder.OrderAmount);
            Assert.AreEqual(result.Order.Description, recievedOrder.OrderDescription);
            Assert.AreEqual(result.Order.Quantity, recievedOrder.OrderQuantity);
            Assert.AreEqual(result.Order.Subtotal, recievedOrder.OrderSubtotal);

            Assert.AreEqual(result.Person.Age, recievedOrder.Age);
            Assert.AreEqual(result.Person.DateJoined, recievedOrder.DateJoined);
            Assert.AreEqual(result.Person.Name, recievedOrder.Name);
        }
    }
}