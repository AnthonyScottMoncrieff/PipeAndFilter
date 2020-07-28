using Microsoft.AspNetCore.Mvc;
using PipeAndFilter.Models.Recieved;
using PipeAndFIlter.Domain.Interfaces;
using System.Threading.Tasks;

namespace PipeAndFilter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FulfilmentController : ControllerBase
    {
        private readonly IFulfilmentManager _fulfilmentManager;

        public FulfilmentController(IFulfilmentManager fulfilmentManager)
        {
            _fulfilmentManager = fulfilmentManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RecievedOrder recievedOrder)
        {
            var result = await _fulfilmentManager.Manage(recievedOrder);
            return Ok(result);
        }
    }
}