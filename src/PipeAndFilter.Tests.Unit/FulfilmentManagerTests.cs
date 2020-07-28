using Moq;
using NUnit.Framework;
using PipeAndFilter.Models;
using PipeAndFilter.Models.Recieved;
using PipeAndFIlter.Domain;
using PipeAndFIlter.Domain.Converters.Interfaces;
using PipeAndFilter.Logging.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Director.Interfaces;

namespace PipeAndFilter.Tests.Unit
{
    [TestFixture]
    public class FulfilmentManagerTests
    {
        private Mock<IPipelineDirector<PipelineData, PipelineResult>> _director;
        private Mock<IModelConverter<RecievedOrder, PipelineData>> _converter;
        private Mock<ILogger> _logger;
        private FulfilmentManager _fulfilmentManager;

        [SetUp]
        public void Setup()
        {
            _director = new Mock<IPipelineDirector<PipelineData, PipelineResult>>();
            _converter = new Mock<IModelConverter<RecievedOrder, PipelineData>>();
            _logger = new Mock<ILogger>();
            _fulfilmentManager = new FulfilmentManager(_converter.Object, _director.Object, _logger.Object);
        }
    }
}