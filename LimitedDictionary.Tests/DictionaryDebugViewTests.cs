using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace LimitedDictionary.Tests
{
    [ExcludeFromCodeCoverage]
    public class DictionaryDebugViewTests
    {
        [Fact]
        public void Pass_null_to_constructor_throws_exception()
        {
            var ex = Record.Exception(() => new DictionaryDebugView<string, string>(null));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Debug_view_show_items_in_collection_successfully()
        {
            var dictionary = new Dictionary<string, string>
            {
                { "a", "A" },
                { "b", "B" }
            };

            var view = new DictionaryDebugView<string, string>(dictionary);

            Assert.NotEmpty(view.Items);
            Assert.Equal(2, view.Items.Length);
        }
    }
}
