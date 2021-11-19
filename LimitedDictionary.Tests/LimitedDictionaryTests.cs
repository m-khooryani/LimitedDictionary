using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace LimitedDictionary.Tests
{
    [ExcludeFromCodeCoverage]
    public class LimitedDictionaryTests
    {
        [Fact]
        public void After_insert_collection_is_not_empty()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");

            Assert.NotEmpty(limitedDictionary);
            Assert.NotEmpty(limitedDictionary.Keys);
            Assert.NotEmpty(limitedDictionary.Values);
            Assert.NotEmpty((limitedDictionary as IDictionary).Keys);
            Assert.NotEmpty((limitedDictionary as IDictionary).Values);
            Assert.NotEmpty((limitedDictionary as IReadOnlyDictionary<string, string>).Keys);
            Assert.NotEmpty((limitedDictionary as IReadOnlyDictionary<string, string>).Values);
        }

        [Fact]
        public void Get_value_from_indexer_successfully()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");

            Assert.Equal("A", limitedDictionary["a"]);
            Assert.Equal("A", (limitedDictionary as IDictionary)["a"]);
            Assert.Null((limitedDictionary as IDictionary)[0]);
        }

        [Fact]
        public void Add_item_through_indexer_with_null_key_throws_exception()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (limitedDictionary as IDictionary)[null] = "A");

            Assert.NotNull(ex);
        }

        [Fact]
        public void Add_item_with_null_value_through_indexer_throws_exception()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (limitedDictionary as IDictionary)["a"] = null);

            Assert.NotNull(ex);
        }

        [Fact]
        public void Add_item_with_null_key_throws_exception()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (limitedDictionary as IDictionary).Add(null, "A"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Add_item_with_null_value_throws_exception()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (limitedDictionary as IDictionary).Add("A", null));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Add_item_with_not_matched_value_type_throws_exception()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (limitedDictionary as IDictionary).Add("A", 3));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Add_item_with_not_matched_key_type_throws_exception()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (limitedDictionary as IDictionary).Add(3, "a"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void After_insert_through_IDictionary_interface_collection_is_not_empty()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            (limitedDictionary as IDictionary).Add("a", "A");

            Assert.NotEmpty(limitedDictionary);
        }

        [Fact]
        public void Add_item_through_IDictionary_indexer_with_not_matched_value_type_throws_exception()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (limitedDictionary as IDictionary)["a"] = 3);

            Assert.NotNull(ex);
        }

        [Fact]
        public void Add_item_through_IDictionary_indexer_with_not_matched_key_type_throws_exception()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (limitedDictionary as IDictionary)[1] = "A");

            Assert.NotNull(ex);
        }

        [Fact]
        public void Add_item_through_IDictionary_indexer_successful()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary["a"] = "A";
            (limitedDictionary as IDictionary)["b"] = "B";

            Assert.Equal("A", limitedDictionary["a"]);
            Assert.Equal("B", limitedDictionary["b"]);
        }

        [Fact]
        public void GetOrAdd_add_item_to_collection_successfully()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            var value = limitedDictionary.GetOrAdd("a", "A");

            Assert.NotEmpty(limitedDictionary);
            Assert.Equal("A", value);
        }

        [Fact]
        public void TryGetValue_with_null_key_throws_exception()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => limitedDictionary.TryGetValue(null, out string _));

            Assert.NotNull(ex);
        }

        [Fact]
        public void TryGetValue_when_item_does_not_exist_returns_null()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.TryGetValue("a", out string value);

            Assert.Null(value);
        }

        [Fact]
        public void TryGetValue_when_item_exists_returns_value_successfully()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);
            limitedDictionary.Add("a", "A");

            limitedDictionary.TryGetValue("a", out string value);

            Assert.Equal("A", value);
        }

        [Fact]
        public void Add_item_with_KeyValuePair_successful()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add(new KeyValuePair<string, string>("a", "A"));

            Assert.NotEmpty(limitedDictionary);
        }

        [Fact]
        public void Count_show_number_of_existed_items_in_collection()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");
            var count = limitedDictionary.Count;

            Assert.Equal(1, count);
        }

        [Fact]
        public void Iterate_through_items_is_supported()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");
            limitedDictionary.Add("b", "B");

            foreach (var item in limitedDictionary)
            {
                Assert.NotNull(item.Key);
                Assert.NotNull(item.Value);
            }
        }
        
        [Fact]
        public void Contains_when_item_exists_in_collection_returns_true()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");

            Assert.True(limitedDictionary.Contains("a"));
        }

        [Fact]
        public void Contains_when_item_does_not_exist_in_collection_returns_false()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");

            Assert.False(limitedDictionary.Contains("b"));
        }

        [Fact]
        public void Contains_with_not_match_key_type_returns_false()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");

            Assert.False(limitedDictionary.Contains(0));
        }

        [Fact]
        public void Contains_with_null_key_throws_exception()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");

            var ex = Record.Exception(() =>
            {
                Assert.False(limitedDictionary.Contains(null));
            });

            Assert.NotNull(ex);
        }

        [Fact]
        public void Remove_with_null_key_throws_exception()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");

            var ex = Record.Exception(() =>
            {
                (limitedDictionary as IDictionary).Remove(null);
            });

            Assert.NotNull(ex);
        }

        [Fact]
        public void Remove_with_not_match_key_type()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");

            (limitedDictionary as IDictionary).Remove(0);

            Assert.Single(limitedDictionary);
        }

        [Fact]
        public void Remove_with_not_match_key()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");

            (limitedDictionary as IDictionary).Remove("b");

            Assert.Single(limitedDictionary);
        }

        [Fact]
        public void Remove_sucessful()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");

            (limitedDictionary as IDictionary).Remove("a");

            Assert.Empty(limitedDictionary);
        }

        [Fact]
        public void Remove_with_KeyValuePair_successful()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");

            limitedDictionary.Remove(new KeyValuePair<string, string>("a", "A"));

            Assert.Empty(limitedDictionary);
        }


        [Fact]
        public void Copy_to_successful()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");

            var array = new KeyValuePair<string, string>[1];
            limitedDictionary.CopyTo(array, 0);

            Assert.Equal("a", array[0].Key);
        }

        [Fact]
        public void _Copy_to_as_ICollection_successful()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");

            var array = Array.CreateInstance(typeof(KeyValuePair<string, string>), 1);
            (limitedDictionary as ICollection).CopyTo(array, 0);

            Assert.Equal(new KeyValuePair<string, string>("a", "A"), array.GetValue(0));
        }

        [Fact]
        public void Contains_with_KeyVauePair_when_item_exists_in_collection_returns_true()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");

            Assert.Contains(new KeyValuePair<string, string>("a", "A"), limitedDictionary);
        }

        [Fact]
        public void Get_dictionary_enumerator_successful()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(3);

            limitedDictionary.Add("a", "A");

            var enumerator = (limitedDictionary as IDictionary).GetEnumerator();
            Assert.True(enumerator.MoveNext());
        }

        [Fact]
        public void Clear_remove_all_items_in_collection()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(1);

            limitedDictionary.Add("a", "A");
            limitedDictionary.Clear();

            Assert.Empty(limitedDictionary);
        }

        [Fact]
        public void Add_removes_the_first_item_when_collection_exceed_limitation_size()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(1);

            limitedDictionary.Add("a", "A");
            limitedDictionary.Add("b", "B");

            Assert.False(limitedDictionary.ContainsKey("a"));
        }

        [Fact]
        public void Add_removes_the_less_frequent_used_item_when_collection_exceed_limitation_size()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(2);

            limitedDictionary.Add("a", "A");
            limitedDictionary.Add("b", "B");
            _ = limitedDictionary["a"];
            limitedDictionary.Add("c", "C");

            Assert.False(limitedDictionary.ContainsKey("b"));
        }

        [Fact]
        public void SyncRoot_is_implemented()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(1);

            Assert.NotNull((limitedDictionary as ICollection).SyncRoot);
        }

        [Fact]
        public void IsFixedSize_is_implemented()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(1);

            Assert.False((limitedDictionary as IDictionary).IsFixedSize);
        }

        [Fact]
        public void IsReadOnly_is_implemented()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(1);

            Assert.False((limitedDictionary as IDictionary).IsReadOnly);
        }

        [Fact]
        public void ICollection_IsReadOnly_is_implemented()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(1);

            Assert.False((limitedDictionary as ICollection<KeyValuePair<string, string>>).IsReadOnly);
        }

        [Fact]
        public void ICollection_IsSynchronized_is_implemented()
        {
            var limitedDictionary = new LimitedDictionary<string, string>(1);

            Assert.False((limitedDictionary as ICollection).IsSynchronized);
        }
    }
}