using Microsoft.AspNetCore.Mvc;
using Reckon.Coding.Services.FindOccurrences;
using Reckon.Coding.Services.ResultsConsumer.Models;
using System.Threading.Tasks;

namespace Reckon.Coding.WebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FindOccurrencesController : ControllerBase {
        private readonly IFindOccurrencesService _findOccurencesService;
        public FindOccurrencesController(IFindOccurrencesService findOccurencesService ) {
            _findOccurencesService = findOccurencesService;
        }

        [HttpGet]
        public async Task<FindOccurrencesResult> Get() {
            return await _findOccurencesService.SendIndicesOfAsync();
        }
    }
}
