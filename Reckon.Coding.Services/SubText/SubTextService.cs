using Reckon.Coding.Services.Helpers;
using Reckon.Coding.Services.SubText.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reckon.Coding.Services.SubText {
    public class SubTextService : ISubTextService {

        public const string GET_SUBTEXT_URL = "/test2/subTexts";

        private readonly HttpClient _httpClient;
        public SubTextService(HttpClient httpClient) {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<string>> GetSubTextsAsync() {
            var request = new HttpRequestMessage(HttpMethod.Get, GET_SUBTEXT_URL);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content?.ReadAsStringAsync();
            return JsonHelper.ToObject<GetSubTextsResponse>(responseString)
                             .SubTexts;
        }
    }
}
