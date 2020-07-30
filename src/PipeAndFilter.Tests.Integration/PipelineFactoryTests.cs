using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Pipelines;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System;
using System.Collections.Generic;
using PipeAndFilter.Logging.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Factory;
using System.Linq;
using PipeAndFilter.Tests.Helpers;

namespace PipeAndFilter.Tests.Integration
{
    [TestFixture]
    public class PipelineFactoryTests
    {
        private ServiceProviderMock _serviceProvider;
        private Mock<ILogger> _logger;
        private PipelineFactory _pipelineFactory;

        [SetUp]
        public void Setup()
        {
            _serviceProvider = new ServiceProviderMock();
            _logger = new Mock<ILogger>();
            _pipelineFactory = new PipelineFactory(_serviceProvider, _logger.Object);
        }

        [Test]
        public void GetOrderedPipelines_Should_Get_Pipelines_In_Correct_Order()
        {
            //Act
            var pipelines = _pipelineFactory.GetOrderedPipelines();

            //Assert
            Assert.AreEqual(2, pipelines.Count());
            Assert.IsTrue(pipelines.First().Name == nameof(DuplicatePersonCheckerPipeline));
            Assert.IsTrue(pipelines.Skip(1).First().Name == nameof(PersonPipeline));
        }
    }
}