﻿using PipeAndFilter.Models;
using PipeAndFIlter.Domain.Pipelines.Interfaces;
using System;
using System.Threading.Tasks;

namespace PipeAndFIlter.Domain.Pipelines
{
    public class PersonPipeline : IPipeline<PipelineData, PipelineResult>
    {
        public string Name => nameof(PersonPipeline);

        public async Task Do(PipelineData pipelineData, PipelineResult pipelineResult)
        {
            throw new NotImplementedException();
        }

        public async Task Undo(PipelineData pipelineData, PipelineResult pipelineResult)
        {
            throw new NotImplementedException();
        }
    }
}