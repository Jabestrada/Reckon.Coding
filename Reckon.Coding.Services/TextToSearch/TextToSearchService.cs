using Reckon.Coding.Services.Helpers;
using Reckon.Coding.Services.TextToSearch.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reckon.Coding.Services.TextToSearch {
    public class TextToSearchService : ITextToSearchService {

        public const string GET_TEXT_URL = "/test2/textToSearch";
        private readonly HttpClient _httpClient;

        public TextToSearchService(HttpClient httpClient) {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> GetTextAsync() {
            var request = new HttpRequestMessage(HttpMethod.Get, GET_TEXT_URL);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content?.ReadAsStringAsync();
            return JsonHelper.ToObject<GetTextToSearchResponse>(responseString)
                             .Text;
        }
    }
}
