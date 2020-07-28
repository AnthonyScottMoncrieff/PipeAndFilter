using System.Collections.Generic;

namespace PipeAndFilter.Models
{
    public class PipelineResult
    {
        public IList<ResultCode> Warnings { get; set; }
        public IList<ResultCode> Errors { get; set; }

        public PipelineResult()
        {
            Warnings = new List<ResultCode>();
            Errors = new List<ResultCode>();
        }
    }
}