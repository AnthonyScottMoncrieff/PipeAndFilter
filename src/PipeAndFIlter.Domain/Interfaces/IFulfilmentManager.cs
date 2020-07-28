using PipeAndFilter.Models;
using PipeAndFilter.Models.Recieved;
using System.Threading.Tasks;

namespace PipeAndFIlter.Domain.Interfaces
{
    public interface IFulfilmentManager
    {
        Task<PipelineResult> Manage(RecievedOrder order);
    }
}