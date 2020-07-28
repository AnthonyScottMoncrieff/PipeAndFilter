using AutoFixture;
using Moq;
using NUnit.Framework;
using PipeAndFilter.Logging.Interfaces;
using PipeAndFilter.Models;
using PipeAndFilter.Models.Recieved;
using PipeAndFIlter.Domain;
using PipeAndFIlter.Domain.Converters.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Director.Interfaces;
using System;
using System.Threading.Tasks;

namespace PipeAndFilter.Tests.Unit
{
    [TestFixture]
    public class FulfilmentManagerTests
    {
        private Fixture _fixture;
        private Mock<IPipelineDirector<PipelineData, PipelineResult>> _director;
        private Mock<IModelConverter<RecievedOrder, PipelineData>> _converter;
        private Mock<ILogger> _logger;
        private FulfilmentManager _fulfilmentManager;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _director = new Mock<IPipelineDirector<PipelineData, PipelineResult>>();
            _converter = new Mock<IModelConverter<RecievedOrder, PipelineData>>();
            _logger = new Mock<ILogger>();
            _fulfilmentManager = new FulfilmentManager(_converter.Object, _director.Object, _logger.Object);
        }

        [Test]
        public async Task FulfilmentManager_Should_Call_Correct_Dependencies_On_Happy_Path()
        {
            //Arrange
            var recievedOrder = _fixture.Create<RecievedOrder>();

            //Act
            var resposne = await _fulfilmentManager.Manage(recievedOrder);

            //Assert
            _director.Verify(x => x.Do(It.IsAny<PipelineData>(), It.IsAny<PipelineResult>()), Times.Once);
            _director.Verify(x => x.Undo(It.IsAny<PipelineData>(), It.IsAny<PipelineResult>()), Times.Never);
            _converter.Verify(x => x.Convert(It.IsAny<RecievedOrder>()), Times.Once);
            _logger.Verify(x => x.AddMessageDetail(It.IsAny<string>()), Times.Once);
            _logger.Verify(x => x.AddErrorDetail(It.IsAny<string>()), Times.Never);
            _logger.Verify(x => x.SubmitException(It.IsAny<Exception>()), Times.Never);
        }

        [Test]
        public async Task FulfilmentManager_Should_Call_Correct_Dependencies_On_Director_Exception()
        {
            //Arrange
            var recievedOrder = _fixture.Create<RecievedOrder>();
            _director.Setup(x => x.Do(It.IsAny<PipelineData>(), It.IsAny<PipelineResult>())).ThrowsAsync(new Exception("Error"));

            //Act
            var resposne = await _fulfilmentManager.Manage(recievedOrder);

            //Assert
            _director.Verify(x => x.Do(It.IsAny<PipelineData>(), It.IsAny<PipelineResult>()), Times.Once);
            _director.Verify(x => x.Undo(It.IsAny<PipelineData>(), It.IsAny<PipelineResult>()), Times.Once);
            _converter.Verify(x => x.Convert(It.IsAny<RecievedOrder>()), Times.Once);
            _logger.Verify(x => x.AddMessageDetail(It.IsAny<string>()), Times.Once);
            _logger.Verify(x => x.AddErrorDetail(It.IsAny<string>()), Times.Once);
            _logger.Verify(x => x.SubmitException(It.IsAny<Exception>()), Times.Once);
        }
    }
}