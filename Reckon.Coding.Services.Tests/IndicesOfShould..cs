using Reckon.Coding.Services.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Reckon.Coding.Services.Tests {
    public class IndicesOfShould {

        private const string INPUT_1 = "Peter told me (actually he slurrred) that peter the pickle piper piped a pitted pickle before he petered out. Phew!";

        [Theory]
        [InlineData(INPUT_1, "Peter", new int[] { 1, 43, 98 })]
        [InlineData(INPUT_1, "Pick", new int[] { 53, 81 })]
        [InlineData(INPUT_1, "Pi", new int[] { 53, 60, 66, 74, 81 })]
        [InlineData("memememe me", "meme", new int[] { 1, 3, 5 })]
        public void ReturnSubstringIndices(string mainText, string substring, IEnumerable<int> expected) {
            var result = mainText.IndicesOf(substring);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(INPUT_1, "Peter", new int[] { 1, 43, 98 })]
        [InlineData(INPUT_1, "peter", new int[] { 1, 43, 98 })]
        public void BeCaseInsensitive(string mainText, string substring, IEnumerable<int> expected) {
            var result = mainText.IndicesOf(substring);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(INPUT_1, "Z", new int[] { })]
        public void ReturnEmptyIfNotFound(string mainText, string substring, IEnumerable<int> expected) {
            var result = mainText.IndicesOf(substring);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, "a", new int[] { })]
        [InlineData("", "a", new int[] { })]
        [InlineData("a", null, new int[] { })]
        [InlineData("a", "", new int[] { })]
        public void ReturnEmptyIfAnyArgIsNullOrEmpty(string mainText, string substring, IEnumerable<int> expected) {
            var result = mainText.IndicesOf(substring);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ReturnEmptyIfSubstringIsLongerThanMainText() {
            var result = "long".IndicesOf("longer").ToArray();
            Assert.Empty(result);
        }
    }
}
