using Microsoft.AspNetCore.Mvc.Testing;
using Reckon.Coding.WebApi.IntegrationTests.Fixtures;
using Reckon.Coding.WebApi.IntegrationTests.Helpers;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WireMock.ResponseBuilders;
using Xunit;

namespace Reckon.Coding.WebApi.IntegrationTests {
    public class HttpRetryPolicyShould : ReckonCodingWebApiTextFixture {
        public HttpRetryPolicyShould(WebApplicationFactory<Startup> factory) : base(factory) {
        }

        [Fact]
        public async Task BeWiredCorrectly() {
            MockReckonServer.Given(ReckonWireMockHelper.MakeTextToSearchServiceRequest())
                            .WithTitle("TextToSearchServiceRequest")
                            .RespondWith(Response.Create().WithStatusCode(500));
            MockReckonServer.Given(ReckonWireMockHelper.MakeSubTextServiceRequest())
                            .WithTitle("SubTextServiceRequest")
                            .RespondWith(Response.Create().WithStatusCode(500));
            var request = FindOccurrencesRequestBuilder.BuildFindOccurencesRequest();
            
            _ = await HttpClient.SendAsync(request);

            // Total expected request count = (first attempt + configured retry count) * 2 URLs
            Assert.Equal((1 + RetryPolicyCount) * 2, MockReckonServer.LogEntries.Count());
            foreach (var log in MockReckonServer.LogEntries) {
                Assert.Equal((int)HttpStatusCode.InternalServerError, log.ResponseMessage.StatusCode);
            }
        }
    }
}
