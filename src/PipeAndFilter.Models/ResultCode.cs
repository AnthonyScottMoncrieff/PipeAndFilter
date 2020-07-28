using PipeAndFilter.Models.Enums;

namespace PipeAndFilter.Models
{
    public class ResultCode
    {
        public FulfilmentResultType Code { get; set; }

        public string Message { get; set; }

        public string Id { get; set; }
    }
}