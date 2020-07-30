using NUnit.Framework;
using PipeAndFilter.Tests.Helpers.TestEntities;
using PipeAndFIlter.Domain.Extensions;
using System.ComponentModel.DataAnnotations;

namespace PipeAndFilter.Tests.Unit
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        [Test]
        public void GetValue_Should_Successfully_Get_Object_Value()
        {
            //Arrange
            var testEntity = new TestEntity() { Id = 1, Name = "test" };
            var castedEntity = (object)testEntity;

            //Act
            var id = castedEntity.GetValue<int>("Id");

            //Assert
            Assert.AreEqual(id, testEntity.Id);
        }
    }
}