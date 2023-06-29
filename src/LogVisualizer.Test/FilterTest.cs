using ReverseMarkdown.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Joins;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogVisualizer.Test
{
    public class FilterTest
    {
        [Theory]
        [InlineData("fjhgASFDjhgfjhaghafs dhjfHJ GSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "fhgFHSD", true, false, false, true)]
        [InlineData("fjhgASFDjhgfjhaghafs dhjfHJ GSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "fhgfHSD", true, false, false, false)]
        [InlineData("fjhgASFDjhgfjhaghafs dhjfHJ GSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "fhgfHSD", false, false, false, true)]
        [InlineData("fjhgASFDjhgfjhaghafs dhjfHJ GSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "dhjfHJ", false, true, false, true)]
        [InlineData("fjhgASFDjhgfjhaghafs dhjfHJ GSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "dhjfHJ", false, false, false, true)]
        [InlineData("fjhgASFDjhgfjhaghafs dhjfHJ GSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "fhgfHSD", false, true, false, false)]
        [InlineData("fjhgASFDjhgfjhaghafs dhjfHJ GSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", @"\bdhjfHJ\b", false, false, true, true)]
        [InlineData("fjhgASFDjhgfjhaghafs dhjfHJ GSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", @"\bdhjfHJ\b", false, true, true, true)]
        [InlineData("fjhgASFDjhgfjhaghafs dhjfHJ GSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", @"\bdhjfHJ\b", true, false, true, true)]
        [InlineData("fjhgASFDjhgfjhaghafs dhjfHJ GSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", @"\bdhjfHJ\b", true, true, true, true)]
        [InlineData("fjhgASFDjhgfjhaghafs dhjfHJ GSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", @"\bdhjfHJ\b", false, false, false, false)]
        [InlineData("fjhgASFDjhgfjhaghafs dhjfHJ GSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "dhjfHJ", false, false, false, true)]
        [InlineData("fjhgASFDjhgfjhaghafs dhjfHJ GSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "gfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", true, true, false, false)]
        [InlineData("fjhgASFDjhgfjhaghafs dhjfHJ GSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "gfwekdvkjscglGdSkdjfSLJgkJvqkjhvkASdljv", false, true, false, false)]
        [InlineData("fjhgASFDjhgfjhaghafs dhjfHJ GSdfhgFHSDfAGHSFdhgfwekdvkjscglGDSkdjfSLJgkJvqkjhvkASdljv", "dhjfHJ", true, true, false, true)]
        public async Task LoadFromFolderZip(string text, string keyword, bool matchCase, bool matchWholeWord, bool useRegex, bool excepted)
        {
            var result = Search(text, keyword, matchCase, matchWholeWord, useRegex);
            Assert.Equal(result, excepted);
        }
        private bool Search(string text, string keyword, bool matchCase, bool matchWholeWord, bool useRegex)
        {
            string pattern = keyword;
            if (!useRegex)
            {
                pattern = matchWholeWord ? $@"\b{Regex.Escape(keyword)}\b" : Regex.Escape(keyword);
            }

            return Regex.IsMatch(text, pattern, (matchCase | useRegex) ? RegexOptions.None : RegexOptions.IgnoreCase);
        }
    }
}
