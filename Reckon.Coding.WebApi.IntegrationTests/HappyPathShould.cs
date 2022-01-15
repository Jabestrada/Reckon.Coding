using Microsoft.AspNetCore.Mvc.Testing;
using Reckon.Coding.Services.FindOccurrences;
using Reckon.Coding.Services.Helpers;
using Reckon.Coding.Services.ResultsConsumer.Models;
using Reckon.Coding.WebApi.IntegrationTests.Fixtures;
using Reckon.Coding.WebApi.IntegrationTests.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using WireMock.ResponseBuilders;
using Xunit;

namespace Reckon.Coding.WebApi.IntegrationTests {
    public class HappyPathShould : ReckonCodingWebApiTextFixture {
        public HappyPathShould(WebApplicationFactory<Startup> factory) : base(factory) {

        }

        [Fact]
        public async Task PassSmokeTest() {
            var mockTextToSearchResponse = JsonSerializer.Serialize(new {
                text = "This is misty!"
            });
            MockReckonServer.Given(ReckonWireMockHelper.MakeTextToSearchServiceRequest())
                            .RespondWith(ReckonWireMockHelper.MakeResponseBody(mockTextToSearchResponse)
                                                             .WithStatusCode(HttpStatusCode.OK));

            var mocksubTextResponse = JsonSerializer.Serialize(new {
                subTexts = new string[] {
                    "is",
                    "zzzz"
                }
            });
            MockReckonServer.Given(ReckonWireMockHelper.MakeSubTextServiceRequest())
                            .RespondWith(ReckonWireMockHelper.MakeResponseBody(mocksubTextResponse)
                                                             .WithStatusCode(HttpStatusCode.OK));
            MockReckonServer.Given(ReckonWireMockHelper.MakeResultsConsumerRequest())
                            .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK));

            var request = FindOccurrencesRequestBuilder.BuildFindOccurencesRequest();
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = JsonHelper.ToObject<FindOccurrencesResult>(response.Content.ReadAsStringAsync().Result);
            Assert.Equal("3, 6, 10", result.Results.First(r => r.Subtext == "is").Result);
            Assert.Equal(FindOccurrencesService.NO_MATCH, result.Results.First(r => r.Subtext == "zzzz").Result);

            Assert.Equal(3, MockReckonServer.LogEntries.Count());
            foreach (var log in MockReckonServer.LogEntries) {
                Assert.Equal((int)HttpStatusCode.OK, log.ResponseMessage.StatusCode);
            }
        }
    }
}
