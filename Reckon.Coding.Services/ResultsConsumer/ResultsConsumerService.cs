using Reckon.Coding.Services.ResultsConsumer.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Reckon.Coding.Services.ResultsConsumer {
    public class ResultsConsumerService : IResultsConsumerService {
        public const string SUBMIT_URL = "/test2/submitResults";

        private readonly HttpClient _httpClient;
        public ResultsConsumerService(HttpClient httpClient) {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task ConsumeResultAsync(FindOccurrencesResult result) {
            var request = new HttpRequestMessage(HttpMethod.Post, SUBMIT_URL);
            var payload = JsonSerializer.Serialize(result,
                                new JsonSerializerOptions {
                                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                                });
            request.Content = new StringContent(payload);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
