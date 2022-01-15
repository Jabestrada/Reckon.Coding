using Moq;
using Reckon.Coding.Services.FindOccurrences;
using Reckon.Coding.Services.ResultsConsumer;
using Reckon.Coding.Services.ResultsConsumer.Models;
using Reckon.Coding.Services.SubText;
using Reckon.Coding.Services.TextToSearch;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Reckon.Coding.Services.Tests {
    public class FindOccurrencesServiceShould {
        private const string INPUT_1 = "Peter told me (actually he slurrred) that peter the pickle piper piped a pitted pickle before he petered out. Phew!";

        [Theory]
        [InlineData(INPUT_1,
                    new string[] {
                        "Peter",
                        "peter",
                        "Pick",
                        "Pi",
                        "Z"
                    },
                    new string[] {
                        "1, 43, 98",
                        "1, 43, 98",
                        "53, 81",
                        "53, 60, 66, 74, 81",
                         FindOccurrencesService.NO_MATCH
                    }
        )]
        public void FormatResultsCorrectly(string mainText, string[] substrings, string[] expected) {
            var sut = new FindOccurrencesService(new Mock<ITextToSearchService>().Object,
                                                 new Mock<ISubTextService>().Object,
                                                 new Mock<IResultsConsumerService>().Object);

            var results = sut.GetIndicesOf(mainText, substrings)
                             .ToList();

            for (var j = 0; j < substrings.Length; j++) {
                Assert.Equal(results.First(r => r.Subtext == substrings[j]).Result, 
                             expected[j]);
            }
        }

        [Fact]
        public async Task SendsResultsToConsumer() { 
           #region Arrange
            var mockTextToSearchService = new Mock<ITextToSearchService>();
            mockTextToSearchService.Setup(m => m.GetTextAsync())
                                   .Returns(Task.FromResult("testing"));
            
            var mockSubTextService = new Mock<ISubTextService>();
            mockSubTextService.Setup(m => m.GetSubTextsAsync())
                              .Returns(Task.FromResult(new List<string> { "substr1", "test"}.AsEnumerable()));
          
            var mockResultConsumerService = new Mock<IResultsConsumerService>();
            mockResultConsumerService.Setup(m => m.ConsumeResultAsync(It.IsAny<FindOccurrencesResult>()));


            var sut = new FindOccurrencesService(mockTextToSearchService.Object,
                                                 mockSubTextService.Object,
                                                 mockResultConsumerService.Object);
            #endregion Arrange

            // Act
            await sut.SendIndicesOfAsync();

            // Assert
            mockResultConsumerService.Verify(m => m.ConsumeResultAsync(It.IsAny<FindOccurrencesResult>()), Times.Once());
        }

    }
}
