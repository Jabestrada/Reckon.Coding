using System.Collections.Generic;

namespace Reckon.Coding.Services.Extensions {
    public static class StringExtensions {
        public static IEnumerable<int> IndicesOf(this string mainString, string substring) {
            var output = new List<int>();
            if (string.IsNullOrEmpty(mainString) || 
                string.IsNullOrEmpty(substring) ||
                mainString.Length < substring.Length) 
            {
                return output;
            }

            var substrLen = substring.Length;
            for (var mainIdx = 0; mainIdx < mainString.Length; mainIdx++) {
                int substrIdx = 0;
                for (; substrIdx < substrLen && mainIdx + substrIdx < mainString.Length; substrIdx++) {
                    if (char.ToLower(mainString[mainIdx + substrIdx]) != char.ToLower(substring[substrIdx])) {
                        // Match failed, try next char.
                        break;
                    }
                }
                if (substrIdx == substrLen) {
                    // Found match; store as 1-based per reqs.
                    output.Add(1 + ((substrIdx + mainIdx) - substrLen));
                }
            }
            return output;
        }
    }
}
