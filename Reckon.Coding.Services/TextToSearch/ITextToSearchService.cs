using System.Threading.Tasks;

namespace Reckon.Coding.Services.TextToSearch {
    public interface ITextToSearchService {
        Task<string> GetTextAsync();
    }
}
