using Exceptionless.Logging;
using System;
using System.Threading.Tasks;

namespace PipeAndFilter.Logging.Interfaces
{
    public interface ILogger
    {
        void SubmitException(Exception exception);

        void AddMessageDetail(string item);

        void AddErrorDetail(string message);

        void SubmitLog(string orderDetailMessage);

        void SetMessageTitle(string title);

        void SetLogLevel(LogLevel logLevel);

        Task CommitAsync();

        void ClearDetails();
    }
}