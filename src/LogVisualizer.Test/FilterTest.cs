using ReverseMarkdown.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogVisualizer.Test
{
    public class FilterTest
    {
        [Theory]
        [InlineData("fjhgASFDjhgfjhaghafsdhjfHJGSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "fhgFHSD", true, false, false, true)]
        [InlineData("fjhgASFDjhgfjhaghafsdhjfHJGSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "fhgfHSD", true, false, false, false)]
        [InlineData("fjhgASFDjhgfjhaghafsdhjfHJGSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "fhgfHSD", false, false, false, true)]
        [InlineData("fjhgASFDjhgfjhaghafsdhjfHJGSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "fjhgASFDjhgfjhaghafsdhjfHJGSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", true, true, false, true)]
        [InlineData("fjhgASFDjhgfjhaghafsdhjfHJGSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "fjhgASFDjhgfjhaghafsdhjfHJGSdfhgFHSDfAGHSFdhgfwekdvkjscglGdSkdjfSLJgkJvqkjhvkASdljv", false, true, false, true)]
        [InlineData("fjhgASFDjhgfjhaghafsdhjfHJGSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "fjhgASFDjhgfjhaghafsdhjfHJGSdfhgFHSDfAGHSFdhgfwekdvkjscglGdSkdjfSLJgkJvqkjhvkASdljv", true, true, false, false)]
        public async Task LoadFromFolderZip(string text, string keyword, bool matchCase, bool matchWholeWord, bool useRegex, bool excepted)
        {
            var result = Search(text, keyword, matchCase, matchWholeWord, useRegex);
            Assert.Equal(result, excepted);
        }
        private bool Search(string text, string keyword, bool matchCase, bool matchWholeWord, bool useRegex)
        {
            StringComparison stringComparison = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            string pattern = matchWholeWord ? $@"\b{Regex.Escape(keyword)}\b" : Regex.Escape(keyword);
            if (useRegex)
            {
                pattern = $"(?{stringComparison}m){pattern}";
                return Regex.IsMatch(text, pattern);
            }
            else
            {
                return text.IndexOf(keyword, stringComparison) >= 0;
            }
        }
    }
}
