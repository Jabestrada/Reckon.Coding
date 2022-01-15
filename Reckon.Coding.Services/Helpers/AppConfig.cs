using Microsoft.Extensions.Configuration;
using System;

namespace Reckon.Coding.Services.Helpers {
    public class AppConfig {
        #region Lazy readers
        private Lazy<HttpRetryPolicy> _httpRetryPolicy => new Lazy<HttpRetryPolicy>(() => {
            return _config.GetSection("ExternalServices:RetryPolicy").Get<HttpRetryPolicy>();
        });

        #region Service URIs lazy readers
        private Lazy<string> _textToSearchUri => new Lazy<string>(() => {
            return FirstNonEmpty(_config[AppConfigKeys.TEXT_TO_SEARCH_URI], "https://join.reckon.com");
        });

        private Lazy<string> _subTextUri => new Lazy<string>(() => {
            return FirstNonEmpty(_config[AppConfigKeys.SUBTEXT_URI], "https://join.reckon.com");
        });
        private Lazy<string> _resultsConsumerUri => new Lazy<string>(() => {
            return  FirstNonEmpty(_config[AppConfigKeys.RESULTS_CONSUMER_URI], "https://join.reckon.com");
        });
        #endregion Service URIs lazy readers

        #endregion Lazy readers

        private readonly IConfiguration _config;
        
        public string TextToSearchUri => _textToSearchUri.Value;
        public string SubTextUri => _subTextUri.Value;
        public string ResulsConsumerUri => _resultsConsumerUri.Value;
        public HttpRetryPolicy RetryPolicy => _httpRetryPolicy.Value;
        
        public AppConfig(IConfiguration config) {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        #region non-public
        private string FirstNonEmpty(params string[] args) {
            foreach (var s in args) {
                if (!string.IsNullOrWhiteSpace(s)) {
                    return s;
                }
            }
            return string.Empty;
        }
        #endregion
    }

    public class HttpRetryPolicy {
        public int RetryCount { get; set; }
        public int RetryWaitMs { get; set; }
    }

    public class AppConfigKeys {
        public const string TEXT_TO_SEARCH_URI = "ExternalServices:TextToSearchService:BaseUri";
        public const string SUBTEXT_URI = "ExternalServices:SubTextService:BaseUri";
        public const string RESULTS_CONSUMER_URI = "ExternalServices:ResultsConsumerService:BaseUri";
        
        public const string RETRY_POLICY_COUNT= "ExternalServices:RetryPolicy:RetryCount";
        public const string RETRY_WAIT_MS = "ExternalServices:RetryPolicy:RetryWaitMs";

    }
}
