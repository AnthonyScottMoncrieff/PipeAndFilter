using Exceptionless;
using Exceptionless.Logging;
using PipeAndFilter.Logging.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace PipeAndFilter.Logging
{
    public class Logger : ILogger
    {
        private readonly ExceptionlessClient _exceptionlessClient;
        private string _messageTitle;
        private LogLevel _logLevel = LogLevel.Info;
        private ConcurrentQueue<string> _messageDetails;

        public Logger(ExceptionlessClient exceptionlessClient)
        {
            _exceptionlessClient = exceptionlessClient;
            _messageDetails = new ConcurrentQueue<string>();
            SetMessageTitle("Performing Fulfilment");
        }

        public void SubmitException(Exception exception)
        {
            _exceptionlessClient.SubmitException(exception);
        }

        public void AddMessageDetail(string message)
        {
            _messageDetails.Enqueue($"{DateTime.Now} - {LogLevel.Info.Name.ToUpper()} - {message}");
        }

        public void AddErrorDetail(string message)
        {
            SetLogLevel(LogLevel.Error);
            _messageDetails.Enqueue($"{DateTime.Now} - {LogLevel.Error.Name.ToUpper()} - {message}");
        }

        public void SubmitLog(string orderDetailMessage)
        {
            _exceptionlessClient.SubmitLog($"{DateTime.Now} - {orderDetailMessage}", _logLevel);
        }

        public void SetMessageTitle(string title) => _messageTitle = $"{DateTime.Now} - {title}";

        public void SetLogLevel(LogLevel logLevel) => _logLevel = logLevel;

        public Task CommitAsync()
        {
            _exceptionlessClient.CreateLog(_messageTitle, _logLevel)
                .AddObject(string.Join(Environment.NewLine, _messageDetails), "Details")
                .Submit();

            return _exceptionlessClient.ProcessQueueAsync();
        }

        public void ClearDetails()
        {
            _messageDetails = new ConcurrentQueue<string>();
        }
    }
}