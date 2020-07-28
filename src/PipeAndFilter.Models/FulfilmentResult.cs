using System.Collections.Generic;

namespace PipeAndFilter.Models
{
    public class FulfilmentResult
    {
        public IEnumerable<ResultCode> Errors { get; set; }
        public IEnumerable<ResultCode> Warnings { get; set; }

        public FulfilmentResult()
        {
            Errors = new List<ResultCode>();
            Warnings = new List<ResultCode>();
        }
    }
}