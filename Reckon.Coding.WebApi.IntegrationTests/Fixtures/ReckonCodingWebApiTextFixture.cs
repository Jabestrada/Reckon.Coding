using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Reckon.Coding.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using WireMock.Server;
using Xunit;

namespace Reckon.Coding.WebApi.IntegrationTests.Fixtures {
    public class ReckonCodingWebApiTextFixture : IDisposable, IClassFixture<WebApplicationFactory<Startup>> {
        public const int RetryPolicyCount = 2;
        protected HttpClient HttpClient;
        protected WireMockServer MockReckonServer;

        public ReckonCodingWebApiTextFixture(WebApplicationFactory<Startup> factory) : this(factory, false) {

        }
        public ReckonCodingWebApiTextFixture(WebApplicationFactory<Startup> factory, bool useHttps) {
            var extraConfiguration = GetExtraConfiguration();
            var afterHttp = useHttps ? "s" : "";
            HttpClient = factory.WithWebHostBuilder(whb => {
                whb.ConfigureAppConfiguration((context, configBuilder) => {
                    configBuilder.AddInMemoryCollection(extraConfiguration);
                });
            }).CreateClient(new WebApplicationFactoryClientOptions {
                BaseAddress = new Uri($"http://localhost:5000/")
            });
        }

        private Dictionary<string, string> GetExtraConfiguration() {
            MockReckonServer = WireMockServer.Start();

            var mockReckonServerUri = MockReckonServer.Urls.Single();
            return new Dictionary<string, string> {
                { AppConfigKeys.SUBTEXT_URI, mockReckonServerUri},
                { AppConfigKeys.TEXT_TO_SEARCH_URI, mockReckonServerUri},
                { AppConfigKeys.RESULTS_CONSUMER_URI, mockReckonServerUri },
                { AppConfigKeys.RETRY_POLICY_COUNT, $"{RetryPolicyCount}" },
                { AppConfigKeys.RETRY_WAIT_MS, $"{10}" },
            };
        }

        #region IDisposable
        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                HttpClient?.Dispose();
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion IDisposable
    }
}
