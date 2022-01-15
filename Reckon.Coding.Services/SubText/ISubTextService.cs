using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reckon.Coding.Services.SubText {
    public interface ISubTextService {
        Task<IEnumerable<string>> GetSubTextsAsync();
    }
}
