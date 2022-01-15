using System.Collections.Generic;

namespace Reckon.Coding.Services.ResultsConsumer.Models {
    public class FindOccurrencesResult {
        public string Candidate { get; set; }
        public string Text { get; set; }
        public IEnumerable<FindMatchResult> Results{ get; set; }
    }

    public class FindMatchResult {
        public string Subtext { get; set; }
        public string Result { get; set; }
    }
}
