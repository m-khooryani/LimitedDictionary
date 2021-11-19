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
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");

            Assert.NotEmpty(dic);
            Assert.NotEmpty(dic.Keys);
            Assert.NotEmpty(dic.Values);
            Assert.NotEmpty((dic as IDictionary).Keys);
            Assert.NotEmpty((dic as IDictionary).Values);
            Assert.NotEmpty((dic as IReadOnlyDictionary<string, string>).Keys);
            Assert.NotEmpty((dic as IReadOnlyDictionary<string, string>).Values);
        }

        [Fact]
        public void Get_value_from_indexer_successfully()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");

            Assert.Equal("A", dic["a"]);
            Assert.Equal("A", (dic as IDictionary)["a"]);
            Assert.Null((dic as IDictionary)[0]);
        }

        [Fact]
        public void Add_item_through_indexer_with_null_key_throws_exception()
        {
            var dic = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (dic as IDictionary)[null] = "A");

            Assert.NotNull(ex);
        }

        [Fact]
        public void Add_item_with_null_value_through_indexer_throws_exception()
        {
            var dic = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (dic as IDictionary)["a"] = null);

            Assert.NotNull(ex);
        }

        [Fact]
        public void Add_item_with_null_key_throws_exception()
        {
            var dic = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (dic as IDictionary).Add(null, "A"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Add_item_with_null_value_throws_exception()
        {
            var dic = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (dic as IDictionary).Add("A", null));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Add_item_with_not_matched_value_type_throws_exception()
        {
            var dic = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (dic as IDictionary).Add("A", 3));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Add_item_with_not_matched_key_type_throws_exception()
        {
            var dic = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (dic as IDictionary).Add(3, "a"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void After_insert_through_IDictionary_interface_collection_is_not_empty()
        {
            var dic = new LimitedDictionary<string, string>(3);

            (dic as IDictionary).Add("a", "A");

            Assert.NotEmpty(dic);
        }

        [Fact]
        public void Add_item_through_IDictionary_indexer_with_not_matched_value_type_throws_exception()
        {
            var dic = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (dic as IDictionary)["a"] = 3);

            Assert.NotNull(ex);
        }

        [Fact]
        public void Add_item_through_IDictionary_indexer_with_not_matched_key_type_throws_exception()
        {
            var dic = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => (dic as IDictionary)[1] = "A");

            Assert.NotNull(ex);
        }

        [Fact]
        public void Add_item_through_IDictionary_indexer_successful()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic["a"] = "A";
            (dic as IDictionary)["b"] = "B";

            Assert.Equal("A", dic["a"]);
            Assert.Equal("B", dic["b"]);
        }

        [Fact]
        public void GetOrAdd_add_item_to_collection_successfully()
        {
            var dic = new LimitedDictionary<string, string>(3);

            var value = dic.GetOrAdd("a", "A");

            Assert.NotEmpty(dic);
            Assert.Equal("A", value);
        }

        [Fact]
        public void TryGetValue_with_null_key_throws_exception()
        {
            var dic = new LimitedDictionary<string, string>(3);

            var ex = Record.Exception(() => dic.TryGetValue(null, out string _));

            Assert.NotNull(ex);
        }

        [Fact]
        public void TryGetValue_when_item_does_not_exist_returns_null()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.TryGetValue("a", out string value);

            Assert.Null(value);
        }

        [Fact]
        public void TryGetValue_when_item_exists_returns_value_successfully()
        {
            var dic = new LimitedDictionary<string, string>(3);
            dic.Add("a", "A");

            dic.TryGetValue("a", out string value);

            Assert.Equal("A", value);
        }

        [Fact]
        public void Add_item_with_KeyValuePair_successful()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add(new KeyValuePair<string, string>("a", "A"));

            Assert.NotEmpty(dic);
        }

        [Fact]
        public void Count_show_number_of_existed_items_in_collection()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");
            var count = dic.Count;

            Assert.Equal(1, count);
        }

        [Fact]
        public void Iterate_through_items_is_supported()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");
            dic.Add("b", "B");

            foreach (var item in dic)
            {
                Assert.NotNull(item.Key);
                Assert.NotNull(item.Value);
            }
        }
        
        [Fact]
        public void Contains_when_item_exists_in_collection_returns_true()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");

            Assert.True(dic.Contains("a"));
        }

        [Fact]
        public void Contains_when_item_does_not_exist_in_collection_returns_false()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");

            Assert.False(dic.Contains("b"));
        }

        [Fact]
        public void Contains_with_not_match_key_type_returns_false()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");

            Assert.False(dic.Contains(0));
        }

        [Fact]
        public void Contains_with_null_key_throws_exception()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");

            var ex = Record.Exception(() =>
            {
                Assert.False(dic.Contains(null));
            });

            Assert.NotNull(ex);
        }

        [Fact]
        public void Remove_with_null_key_throws_exception()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");

            var ex = Record.Exception(() =>
            {
                (dic as IDictionary).Remove(null);
            });

            Assert.NotNull(ex);
        }

        [Fact]
        public void Remove_with_not_match_key_type()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");

            (dic as IDictionary).Remove(0);

            Assert.Single(dic);
        }

        [Fact]
        public void Remove_with_not_match_key()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");

            (dic as IDictionary).Remove("b");

            Assert.Single(dic);
        }

        [Fact]
        public void Remove_sucessful()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");

            (dic as IDictionary).Remove("a");

            Assert.Empty(dic);
        }

        [Fact]
        public void Remove_with_KeyValuePair_successful()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");

            dic.Remove(new KeyValuePair<string, string>("a", "A"));

            Assert.Empty(dic);
        }


        [Fact]
        public void Copy_to_successful()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");

            var array = new KeyValuePair<string, string>[1];
            dic.CopyTo(array, 0);

            Assert.Equal("a", array[0].Key);
        }

        [Fact]
        public void _Copy_to_as_ICollection_successful()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");

            var array = Array.CreateInstance(typeof(KeyValuePair<string, string>), 1);
            (dic as ICollection).CopyTo(array, 0);

            Assert.Equal(new KeyValuePair<string, string>("a", "A"), array.GetValue(0));
        }

        [Fact]
        public void Contains_with_KeyVauePair_when_item_exists_in_collection_returns_true()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");

            Assert.Contains(new KeyValuePair<string, string>("a", "A"), dic);
        }

        [Fact]
        public void Get_dictionary_enumerator_successful()
        {
            var dic = new LimitedDictionary<string, string>(3);

            dic.Add("a", "A");

            var enumerator = (dic as IDictionary).GetEnumerator();
            Assert.True(enumerator.MoveNext());
        }

        [Fact]
        public void Clear_remove_all_items_in_collection()
        {
            var dic = new LimitedDictionary<string, string>(1);

            dic.Add("a", "A");
            dic.Clear();

            Assert.Empty(dic);
        }

        [Fact]
        public void Add_removes_the_first_item_when_collection_exceed_limitation_size()
        {
            var dic = new LimitedDictionary<string, string>(1);

            dic.Add("a", "A");
            dic.Add("b", "B");

            Assert.False(dic.ContainsKey("a"));
        }

        [Fact]
        public void Add_removes_the_less_frequent_used_item_when_collection_exceed_limitation_size()
        {
            var dic = new LimitedDictionary<string, string>(2);

            dic.Add("a", "A");
            dic.Add("b", "B");
            _ = dic["a"];
            dic.Add("c", "C");

            Assert.False(dic.ContainsKey("b"));
        }

        [Fact]
        public void SyncRoot_is_implemented()
        {
            var dic = new LimitedDictionary<string, string>(1);

            Assert.NotNull((dic as ICollection).SyncRoot);
        }

        [Fact]
        public void IsFixedSize_is_implemented()
        {
            var dic = new LimitedDictionary<string, string>(1);

            Assert.False((dic as IDictionary).IsFixedSize);
        }

        [Fact]
        public void IsReadOnly_is_implemented()
        {
            var dic = new LimitedDictionary<string, string>(1);

            Assert.False((dic as IDictionary).IsReadOnly);
        }

        [Fact]
        public void ICollection_IsReadOnly_is_implemented()
        {
            var dic = new LimitedDictionary<string, string>(1);

            Assert.False((dic as ICollection<KeyValuePair<string, string>>).IsReadOnly);
        }

        [Fact]
        public void ICollection_IsSynchronized_is_implemented()
        {
            var dic = new LimitedDictionary<string, string>(1);

            Assert.False((dic as ICollection).IsSynchronized);
        }
    }
}