using Moq;
using NUnit.Framework;
using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Pipelines.Director;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PipeAndFilter.Logging.Interfaces;
using PipeAndFIlter.Domain.Pipelines.Factory.Interfaces;
using AutoFixture;

namespace PipeAndFilter.Tests.Unit
{
    [TestFixture]
    public class FulfilmentDirectorPipelineTests
    {
        private PipelineData _pipelineData;
        private PipelineResult _pipelineResult;
        private Mock<IPipeline<PipelineData, PipelineResult>> _pipelineStep1Mock;
        private Mock<IPipeline<PipelineData, PipelineResult>> _pipelineStep2Mock;
        private Mock<IPipeline<PipelineData, PipelineResult>> _pipelineStep3Mock;
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

            _pipelineStep1Mock = new Mock<IPipeline<PipelineData, PipelineResult>>();
            _pipelineStep1Mock.Setup(x => x.Name).Returns("Step 1");
            _pipelineStep2Mock = new Mock<IPipeline<PipelineData, PipelineResult>>();
            _pipelineStep2Mock.Setup(x => x.Name).Returns("Step 2");
            _pipelineStep3Mock = new Mock<IPipeline<PipelineData, PipelineResult>>();
            _pipelineStep3Mock.Setup(x => x.Name).Returns("Step 3");
            _loggerMock = new Mock<ILogger>();
            _pipelineSectorFactory = new Mock<IPipelineFactory>();

            _pipelines = new List<IPipeline<PipelineData, PipelineResult>>();
            _pipelines.AddRange(new[] { _pipelineStep1Mock.Object, _pipelineStep2Mock.Object, _pipelineStep3Mock.Object });
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

            _pipelineStep1Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep2Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep3Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep1Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Never);
            _pipelineStep2Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Never);
            _pipelineStep3Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Never);
        }

        [Test]
        public void Should_Do_And_Undo_All_Undoable_Steps_When_Do_And_Undo_Called()
        {
            GivenFulfilmentPipeline(_pipelines);

            _sut.Do(_pipelineData, _pipelineResult);
            _sut.Undo(_pipelineData, _pipelineResult);

            _pipelineStep1Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep2Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep3Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep1Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep2Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep3Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
        }

        [Test]
        public void Should_Undo_Nothing_When_Do_Not_Called()
        {
            GivenFulfilmentPipeline(_pipelines);

            _sut.Undo(_pipelineData, _pipelineResult);

            _pipelineStep1Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Never);
            _pipelineStep2Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Never);
            _pipelineStep3Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Never);
            _pipelineStep1Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Never);
            _pipelineStep2Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Never);
            _pipelineStep3Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Never);
        }

        [Test]
        public async Task Should_Stop_Calling_Do_When_Exception_RaisedAsync()
        {
            GivenFulfilmentPipeline(_pipelines);
            _pipelineStep2Mock.Setup(p => p.Do(_pipelineData, _pipelineResult))
                .Throws(new Exception(ErrorMessage));

            try
            {
                await _sut.Do(_pipelineData, _pipelineResult);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message == ErrorMessage);
            }


            _pipelineStep1Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep2Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep3Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Never);
        }

        [Test]
        public async Task Should_Stop_Calling_Undo_When_Exception_Raised()
        {
            GivenFulfilmentPipeline(_pipelines);
            _pipelineStep2Mock.Setup(p => p.Undo(_pipelineData, _pipelineResult))
                .Throws(new Exception(ErrorMessage));

            await _sut.Do(_pipelineData, _pipelineResult);
            _pipelineStep1Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep2Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep3Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);

            try
            {
                await _sut.Undo(_pipelineData, _pipelineResult);
            }
            catch (Exception e)
            {
                Assert.That(e.Message == ErrorMessage);
            }

            _pipelineStep3Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep2Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep1Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Never);
        }

        [Test]
        public void Should_Start_With_First_Step_When_Do_Called()
        {
            GivenFulfilmentPipeline(_pipelines);
            var callOrder = 0;
            _pipelineStep1Mock.Setup(x => x.Do(_pipelineData, _pipelineResult))
                .Returns(Task.FromResult(new PipelineResult())).Callback(() => Assert.That(++callOrder, Is.EqualTo(1)));
            _pipelineStep2Mock.Setup(x => x.Do(_pipelineData, _pipelineResult))
                .Returns(Task.FromResult(new PipelineResult())).Callback(() => Assert.That(++callOrder, Is.EqualTo(2)));
            _pipelineStep3Mock.Setup(x => x.Do(_pipelineData, _pipelineResult))
                .Returns(Task.FromResult(new PipelineResult())).Callback(() => Assert.That(++callOrder, Is.EqualTo(3)));

            _sut.Do(_pipelineData, _pipelineResult);

            _pipelineStep1Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep2Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep3Mock.Verify(p => p.Do(_pipelineData, _pipelineResult), Times.Once);
        }

        [Test]
        public void Should_Start_With_Last_Step_When_Undo_Called()
        {
            GivenFulfilmentPipeline(_pipelines);
            _sut.Do(_pipelineData, _pipelineResult);

            var callOrder = 0;
            _pipelineStep3Mock.Setup(x => x.Undo(_pipelineData, _pipelineResult))
                .Returns(Task.FromResult(new PipelineResult())).Callback(() => Assert.That(++callOrder, Is.EqualTo(1)));
            _pipelineStep2Mock.Setup(x => x.Undo(_pipelineData, _pipelineResult))
                .Returns(Task.FromResult(new PipelineResult())).Callback(() => Assert.That(++callOrder, Is.EqualTo(2)));
            _pipelineStep1Mock.Setup(x => x.Undo(_pipelineData, _pipelineResult))
                .Returns(Task.FromResult(new PipelineResult())).Callback(() => Assert.That(++callOrder, Is.EqualTo(3)));

            _sut.Undo(_pipelineData, _pipelineResult);

            _pipelineStep1Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep2Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
            _pipelineStep3Mock.Verify(p => p.Undo(_pipelineData, _pipelineResult), Times.Once);
        }
    }
}
