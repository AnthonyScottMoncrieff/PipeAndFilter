using NUnit.Framework;
using PipeAndFilter.Tests.Helpers.TestEntities;
using PipeAndFIlter.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PipeAndFilter.Tests.Unit
{
    [TestFixture]
    public class CollectionExtensionsTests
    {
        [Test]
        public void SortBy_Should_Sort_Collection_Successfully()
        {
            //Arrange
            var numberSortedList = new int[] { 1, 2, 3, 4, 5, 6 };
            var testEntity2 = new TestEntity { Id = 2, Name = "test2" };
            var testEntity3 = new TestEntity { Id = 3, Name = "test3" };
            var testEntity1 = new TestEntity { Id = 1, Name = "test1" };
            var entityCollection = new TestEntity[] { testEntity2, testEntity3, testEntity1 };

            //Act
            var sortedEntityCollection = entityCollection.SortBy(numberSortedList, x => x.Id);

            //Assert
            Assert.AreEqual(sortedEntityCollection.First().Id, 1);
            Assert.AreEqual(sortedEntityCollection.ElementAt(1).Id, 2);
            Assert.AreEqual(sortedEntityCollection.Last().Id, 3);
        }
    }
}
