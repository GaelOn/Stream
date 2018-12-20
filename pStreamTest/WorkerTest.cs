using FluentAssertions;
using NUnit.Framework;
using pStream.Messages;
using pStream.Pipeline;
using pStream.Workers;
using StreamTest.MockObject;
using System;

namespace StreamTest
{
    [TestFixture]
    public class WorkerTest
    {
        [Test]
        public void InputMessagehouldProduceInputMessageIfNoIssue()
        {
            using (var reader   = new ReadableMock<IMessage>())
            using (var pipeMock = new PipeableMock<IMessage>())
            using (var worker   = new Worker<int, int>(new MessageVisitorFactory(), Identity, SharedPipeMockFactory, false))
            {
                worker.PipeFrom(reader);
                pipeMock.PipeFrom(worker);
                reader.Writer.Push(new InputMessage<int>(3));
                worker.Start();
                pipeMock.Read();
                pipeMock.Results[0].Should().BeOfType<InputMessage<int>>();
                ((InputMessage<int>)pipeMock.Results[0]).Value.Should().Be(3);
            }
        }

        [Test]
        public void InputMessagehouldProduceIErrorMessageIfIssue()
        {
            using (var reader = new ReadableMock<IMessage>())
            using (var pipeMock = new PipeableMock<IMessage>())
            using (var worker = new Worker<int, int>(new MessageVisitorFactory(), ThrowError, SharedPipeMockFactory, false))
            {
                worker.PipeFrom(reader);
                pipeMock.PipeFrom(worker);
                reader.Writer.Push(new InputMessage<int>(3));
                worker.Start();
                pipeMock.Read();
                pipeMock.Results[0].Should().BeOfType<ErrorMessage<Exception>>();
                ((ErrorMessage<Exception>)pipeMock.Results[0]).Error.Should().BeOfType<TestException>();
                ((ErrorMessage<Exception>)pipeMock.Results[0]).Error.Message.Should().Be("test");
            }
        }

        [Test]
        public void ErrorMessagehouldProduceErrorMessage()
        {
            using (var reader = new ReadableMock<IMessage>())
            using (var pipeMock = new PipeableMock<IMessage>())
            using (var worker = new Worker<int, int>(new MessageVisitorFactory(), Identity, SharedPipeMockFactory, false))
            {
                worker.PipeFrom(reader);
                pipeMock.PipeFrom(worker);
                reader.Writer.Push(new ErrorMessage<Exception>(new TestException()));
                worker.Start();
                pipeMock.Read();
                pipeMock.Results[0].Should().BeOfType<ErrorMessage<Exception>>();
                ((ErrorMessage<Exception>)pipeMock.Results[0]).Error.Should().BeOfType<TestException>();
                ((ErrorMessage<Exception>)pipeMock.Results[0]).Error.Message.Should().Be("test");
            }
        }

        [Test]
        public void EndOfStreamShouldProduceEndOfStream()
        {
            using (var reader = new ReadableMock<IMessage>())
            using (var pipeMock = new PipeableMock<IMessage>())
            using (var worker = new Worker<int, int>(new MessageVisitorFactory(), Identity, SharedPipeMockFactory, false))
            {
                worker.PipeFrom(reader);
                pipeMock.PipeFrom(worker);
                reader.Writer.Push(EndOfStreamMessage.Value);
                worker.Start();
                pipeMock.Read();
                pipeMock.Results[0].Should().BeOfType<EndOfStreamMessage>();
            }
        }

        private class TestException : Exception
        {
            public TestException() : base("test") { }
        }

        private int Identity(int x) => x;

        private int ThrowError(int x) => throw new TestException();
        
        private ISharedPipe<IMessage> SharedPipeMockFactory() => new SharedPipeMock<IMessage>();
    }
}