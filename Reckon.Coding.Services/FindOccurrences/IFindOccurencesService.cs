using Reckon.Coding.Services.ResultsConsumer.Models;
using System.Threading.Tasks;

namespace Reckon.Coding.Services.FindOccurrences {
    public interface IFindOccurrencesService {
        Task<FindOccurrencesResult> SendIndicesOfAsync();
    }
}
