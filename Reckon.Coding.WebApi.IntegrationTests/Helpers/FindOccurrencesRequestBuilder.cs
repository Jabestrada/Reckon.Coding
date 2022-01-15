using System.Net.Http;

namespace Reckon.Coding.WebApi.IntegrationTests.Helpers {
    public class FindOccurrencesRequestBuilder {
        public static HttpRequestMessage BuildFindOccurencesRequest() {
            return new HttpRequestMessage(HttpMethod.Get, "api/FindOccurrences");

        }
    }
}
