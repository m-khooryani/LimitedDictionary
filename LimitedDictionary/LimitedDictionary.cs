using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public partial class LimitedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    private ulong _version = 0UL;
    private readonly int _limit;
    private readonly Dictionary<TKey, TValue> _items;
    private readonly SortedDictionary<ulong, TKey> _versionKeyMap;
    private readonly SortedDictionary<TKey, ulong> _keyVersionMap;
    private object? syncRoot;

    public LimitedDictionary(int limit)
        : this()
    {
        _limit = limit;
    }

    private LimitedDictionary()
    {
        _items = new Dictionary<TKey, TValue>();
        _versionKeyMap = new SortedDictionary<ulong, TKey>();
        _keyVersionMap = new SortedDictionary<TKey, ulong>();
    }

    public int Count => _items.Count;

    public ICollection<TKey> Keys => _items.Keys;

    public ICollection<TValue> Values => _items.Values;

    bool IDictionary.IsReadOnly => false;

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

    bool IDictionary.IsFixedSize => false;

    ICollection IDictionary.Keys => _items.Keys;

    ICollection IDictionary.Values => _items.Values;

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot
    {
        get
        {
            if (this.syncRoot == null)
            {
                Interlocked.CompareExchange(ref this.syncRoot, new object(), null);
            }
            return this.syncRoot;
        }
    }

    IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => _items.Keys;

    IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => _items.Values;

    object IDictionary.this[object key]
    {
        get
        {
            if (key is TKey key1)
            {
                return GetValueAndUpdateUsageFrequency(key1);
            }
            else
            {
                return null;
            }
        }
        set
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            try
            {
                TKey key1 = (TKey)key;
                try
                {
                    Add(key1, (TValue)value);
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException("value");
                }
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException("key");
            }
        }
    }

    public TValue GetOrAdd(TKey key, TValue value)
    {
        if (!_items.ContainsKey(key))
        {
            Add(key, value);
        }
        return this[key];
    }

    public bool ContainsKey(TKey key)
    {
        return _items.ContainsKey(key);
    }

    public TValue this[TKey key]
    {
        get
        {
            return GetValueAndUpdateUsageFrequency(key);
        }
        set
        {
            Add(key, value);
        }
    }

    private TValue GetValueAndUpdateUsageFrequency(TKey key)
    {
        var value = _items[key];
        var oldVersion = _keyVersionMap[key];
        _versionKeyMap.Remove(oldVersion);
        _keyVersionMap.Remove(key);

        _versionKeyMap.Add(_version, key);
        _keyVersionMap.Add(key, _version);

        _version++;

        return value;
    }

    private bool IsFull()
    {
        return _versionKeyMap.Count >= _limit;
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (!_items.ContainsKey(key))
        {
            value = default;
            return false;
        }
        var node = GetValueAndUpdateUsageFrequency(key);
        value = node;
        return true;
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Add(TKey key, TValue value)
    {
        if (IsFull())
        {
            RemoveOldest();
        }
        _items.Add(key, value);
        _versionKeyMap.Add(_version, key);
        _keyVersionMap.Add(key, _version);
        _version++;
    }

    void IDictionary.Add(object key, object? value)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        if (key is TKey key1)
        {
            if (value is TValue value2)
            {
                Add(key1, value2);
            }
            else
            {
                throw new ArgumentException("value");
            }
        }
        else
        {
            throw new ArgumentException("key");
        }
    }

    public void Clear()
    {
        _items.Clear();
        _versionKeyMap.Clear();
        _keyVersionMap.Clear();
        _version = 0UL;
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return _items.Contains(item);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    public bool Contains(object key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (key is TKey key1)
        {
            return _items.ContainsKey(key1);
        }
        return false;
    }

    IDictionaryEnumerator IDictionary.GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return Remove(item.Key);
    }

    public bool Remove(TKey key)
    {
        if (!_items.Remove(key))
        {
            return false;
        }
        var version = _keyVersionMap[key];
        _versionKeyMap.Remove(version);
        _keyVersionMap.Remove(key);
        return true;
    }

    public void Remove(object key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (key is TKey key1)
        {
            Remove(key1);
        }
    }

    private void RemoveOldest()
    {
        var minKey = _versionKeyMap.Keys.Min();
        var minValue = _versionKeyMap[minKey];
        _versionKeyMap.Remove(minKey);
        _keyVersionMap.Remove(minValue);
        _items.Remove(minValue);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        (_items as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
    }

    void ICollection.CopyTo(Array array, int index)
    {
        (_items as ICollection).CopyTo(array, index);
    }
}
