using AutoFixture;
using Moq;
using NUnit.Framework;
using PipeAndFilter.Logging.Interfaces;
using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Pipelines.Director;
using PipeAndFIlter.Domain.Pipelines.Factory.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PipeAndFilter.Tests.Unit
{
    [TestFixture]
    public class FulfilmentDirectorPipelineTests
    {
        private PipelineData _pipelineData;
        private PipelineResult _pipelineResult;
        private Mock<IPipeline<PipelineData, PipelineResult>> _pipeline1Mock;
        private Mock<IPipeline<PipelineData, PipelineResult>> _pipeline2Mock;
        private Mock<IPipeline<PipelineData, PipelineResult>> _pipeline3Mock;
        private Mock<ILogger> _loggerMock;
        private IPipeline<PipelineData, PipelineResult> _sut;
        private Mock<IPipelineFactory> _pipelineSectorFactory;
        private List<IPipeline<PipelineData, PipelineResult>> _pipelines;
        private Fixture _fixture;

        private const string ErrorMessage = "error";
        private const string ExternalId = "SFXd33434!RT0";

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _pipelineData = _fixture.Create<PipelineData>();
            _pipelineResult = new PipelineResult();

            _pipeline1Mock = new Mock<IPipeline<PipelineData, PipelineResult>>();
            _pipeline1Mock.Setup(x => x.Name).Returns("Step 1");
            _pipeline2Mock = new Mock<IPipeline<PipelineData, PipelineResult>>();
            _pipeline2Mock.Setup(x => x.Name).Returns("Step 2");
            _pipeline3Mock = new Mock<IPipeline<PipelineData, PipelineResult>>();
            _pipeline3Mock.Setup(x => x.Name).Returns("Step 3");
            _loggerMock = new Mock<ILogger>();
            _pipelineSectorFactory = new Mock<IPipelineFactory>();

            _pipelines = new List<IPipeline<PipelineData, PipelineResult>>();
            _pipelines.AddRange(new[] { _pipeline1Mock.Object, _pipeline2Mock.Object, _pipeline3Mock.Object });
        }

        private void GivenFulfilmentPipeline(IEnumerable<IPipeline<PipelineData, PipelineResult>> pipelines)
        {
            _pipelineSectorFactory.Setup(x => x.GetOrderedPipelines()).Returns(pipelines);
            _sut = new FulfilmentDirectorPipeline(_loggerMock.Object, _pipelineSectorFactory.Object);
        }

        [Test]
        public void Name_should_be_class_name()
        {
            GivenFulfilmentPipeline(_pipelines);

            var result = _sut.Name;
            Assert.That(result, Is.EqualTo(nameof(FulfilmentDirectorPipeline)));
        }

        [Test]
        public void Should_Do_All_Steps_When_Do_Called()
        {
            GivenFulfilmentPipeline(_pipelines);

            _sut.Do(_pipelineData, _pipelineResult);

            _pipeline1Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipeline2Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipeline3Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipeline1Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Never);
            _pipeline2Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Never);
            _pipeline3Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Never);
        }

        [Test]
        public void Should_Do_And_Undo_All_Undoable_Steps_When_Do_And_Undo_Called()
        {
            GivenFulfilmentPipeline(_pipelines);

            _sut.Do(_pipelineData, _pipelineResult);
            _sut.Undo(_pipelineData, _pipelineResult);

            _pipeline1Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipeline2Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipeline3Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipeline1Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
            _pipeline2Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
            _pipeline3Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
        }

        [Test]
        public void Should_Undo_Nothing_When_Do_Not_Called()
        {
            GivenFulfilmentPipeline(_pipelines);

            _sut.Undo(_pipelineData, _pipelineResult);

            _pipeline1Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Never);
            _pipeline2Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Never);
            _pipeline3Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Never);
            _pipeline1Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Never);
            _pipeline2Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Never);
            _pipeline3Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Never);
        }

        [Test]
        public async Task Should_Stop_Calling_Do_When_Exception_Raised()
        {
            GivenFulfilmentPipeline(_pipelines);
            _pipeline2Mock.Setup(p => p.Do(_pipelineData, _pipelineResult))
                .Throws(new Exception(ErrorMessage));

            try
            {
                await _sut.Do(_pipelineData, _pipelineResult);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message == ErrorMessage);
            }

            _pipeline1Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipeline2Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipeline3Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Never);
        }

        [Test]
        public async Task Should_Stop_Calling_Undo_When_Exception_Raised()
        {
            GivenFulfilmentPipeline(_pipelines);
            _pipeline2Mock.Setup(p => p.Undo(_pipelineData, _pipelineResult))
                .Throws(new Exception(ErrorMessage));

            await _sut.Do(_pipelineData, _pipelineResult);
            _pipeline1Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipeline2Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipeline3Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);

            try
            {
                await _sut.Undo(_pipelineData, _pipelineResult);
            }
            catch (Exception e)
            {
                Assert.That(e.Message == ErrorMessage);
            }

            _pipeline3Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
            _pipeline2Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
            _pipeline1Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Never);
        }

        [Test]
        public void Should_Start_With_First_Step_When_Do_Called()
        {
            GivenFulfilmentPipeline(_pipelines);
            var callOrder = 0;
            _pipeline1Mock.Setup(x => x.Do(_pipelineData, _pipelineResult))
                .Returns(Task.FromResult(new PipelineResult())).Callback(() => Assert.That(++callOrder, Is.EqualTo(1)));
            _pipeline2Mock.Setup(x => x.Do(_pipelineData, _pipelineResult))
                .Returns(Task.FromResult(new PipelineResult())).Callback(() => Assert.That(++callOrder, Is.EqualTo(2)));
            _pipeline3Mock.Setup(x => x.Do(_pipelineData, _pipelineResult))
                .Returns(Task.FromResult(new PipelineResult())).Callback(() => Assert.That(++callOrder, Is.EqualTo(3)));

            _sut.Do(_pipelineData, _pipelineResult);

            _pipeline1Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipeline2Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipeline3Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
        }

        [Test]
        public void Should_Start_With_Last_Step_When_Undo_Called()
        {
            GivenFulfilmentPipeline(_pipelines);
            _sut.Do(_pipelineData, _pipelineResult);

            var callOrder = 0;
            _pipeline3Mock.Setup(x => x.Undo(_pipelineData, _pipelineResult))
                .Returns(Task.FromResult(new PipelineResult())).Callback(() => Assert.That(++callOrder, Is.EqualTo(1)));
            _pipeline2Mock.Setup(x => x.Undo(_pipelineData, _pipelineResult))
                .Returns(Task.FromResult(new PipelineResult())).Callback(() => Assert.That(++callOrder, Is.EqualTo(2)));
            _pipeline1Mock.Setup(x => x.Undo(_pipelineData, _pipelineResult))
                .Returns(Task.FromResult(new PipelineResult())).Callback(() => Assert.That(++callOrder, Is.EqualTo(3)));

            _sut.Undo(_pipelineData, _pipelineResult);

            _pipeline1Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
            _pipeline2Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
            _pipeline3Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
        }
    }
}