using Reckon.Coding.Services.ResultsConsumer;
using Reckon.Coding.Services.SubText;
using Reckon.Coding.Services.TextToSearch;
using System;
using System.Text;
using WireMock.Matchers;
using WireMock.Matchers.Request;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Reckon.Coding.WebApi.IntegrationTests.Helpers {
    public class ReckonWireMockHelper {

        public static IRequestBuilder MakeTextToSearchServiceRequest() {
            return Request.Create()
                          .WithPath(new WildcardMatcher(TextToSearchService.GET_TEXT_URL, ignoreCase: true));
        }

        public static IResponseBuilder MakeResponseBody(string body) {
            return Response.Create().WithBody(body, encoding: Encoding.UTF8);
        }

        public static IRequestBuilder MakeSubTextServiceRequest() {
            return Request.Create()
                          .WithPath(new WildcardMatcher(SubTextService.GET_SUBTEXT_URL, ignoreCase: true));
        }

        public static IRequestBuilder MakeResultsConsumerRequest() {
            return Request.Create()
                          .UsingPost()
                          .WithPath(new WildcardMatcher(ResultsConsumerService.SUBMIT_URL, ignoreCase: true));
        }

    }
}
