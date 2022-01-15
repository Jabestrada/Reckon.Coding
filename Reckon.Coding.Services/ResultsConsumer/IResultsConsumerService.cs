using Reckon.Coding.Services.ResultsConsumer.Models;
using System.Threading.Tasks;

namespace Reckon.Coding.Services.ResultsConsumer{
    public interface IResultsConsumerService {
        Task ConsumeResultAsync(FindOccurrencesResult result);
    }
}
