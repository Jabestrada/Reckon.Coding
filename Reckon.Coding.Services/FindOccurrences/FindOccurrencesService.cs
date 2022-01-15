using Reckon.Coding.Services.Extensions;
using Reckon.Coding.Services.ResultsConsumer;
using Reckon.Coding.Services.ResultsConsumer.Models;
using Reckon.Coding.Services.SubText;
using Reckon.Coding.Services.TextToSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reckon.Coding.Services.FindOccurrences {
    public class FindOccurrencesService : IFindOccurrencesService {
        public const string NO_MATCH = "<No Output>";
        private readonly ITextToSearchService _textToSearchService;
        private readonly ISubTextService _subTextService;
        private readonly IResultsConsumerService _resultsConsumerService;

        public FindOccurrencesService(ITextToSearchService textToSearchService, 
                                      ISubTextService subTextService,
                                      IResultsConsumerService resultsConsumerService) 
        {
            _textToSearchService = textToSearchService ?? throw new ArgumentNullException(nameof(textToSearchService));
            _subTextService = subTextService ?? throw new ArgumentNullException(nameof(subTextService));
            _resultsConsumerService = resultsConsumerService ?? throw new ArgumentNullException(nameof(_resultsConsumerService));
        }

        public async Task<FindOccurrencesResult> SendIndicesOfAsync() {
            try {
                var mainText = await _textToSearchService.GetTextAsync();
                var subTexts = await _subTextService.GetSubTextsAsync();
                var result = GetIndicesOf(mainText, subTexts);
                var findOccurencesResult = new FindOccurrencesResult {
                    Candidate = "Julius Estrada",
                    Text = mainText,
                    Results = result
                };
                await _resultsConsumerService.ConsumeResultAsync(findOccurencesResult);
                return findOccurencesResult;
            } catch (Exception exc) {
                // TODO: Log exception here.
                throw;
            }
        }

        internal IEnumerable<FindMatchResult> GetIndicesOf(string mainText, IEnumerable<string> subTexts) {
            var output = new List<FindMatchResult>();
            foreach (var text in subTexts) {
                var result = mainText.IndicesOf(text);
                output.Add(new FindMatchResult { 
                    Subtext = text,
                    Result = result.Any() ? string.Join(", ", result) : NO_MATCH
                });
            }
            return output;
        }
    }
}
