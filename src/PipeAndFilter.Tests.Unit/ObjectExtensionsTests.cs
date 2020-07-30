using NUnit.Framework;
using PipeAndFIlter.Domain.Extensions;
using PipeAndFIlter.Domain.Pipelines;
using System;
using System.Collections.Generic;
using System.Text;

namespace PipeAndFilter.Tests.Unit
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        [Test]
        public void GetValue_Should_Successfully_Get_Object_Value()
        {
            //Arrange
            var personPipeline = new PersonPipeline();

            //Act
            var name = personPipeline.GetValue<string>("Name");

            //Assert
            Assert.AreEqual(name, personPipeline.Name);
        }
    }
}
