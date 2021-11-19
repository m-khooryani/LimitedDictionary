# LimitedDictionary
Dictionary with limited items count

Removes the less frequently used item in the dictionary when collections items exceed the limit in O(log(n))

[on NuGet](https://www.nuget.org/packages/LimitedDictionary/1.0.1)

```csharp

var limitedItems = new LimitedDictionary<string, string>(limit: 2);

limitedItems.Add("a", "A");
limitedItems.Add("b", "B");
_ = limitedDictionary["a"]; // use 'a', now 'b' is the less frequently used item
limitedItems.Add("c", "C"); // exceed the limit, remove 'b', add the new item 'c'

limitedItems.ContainsKey("b"); // false
```
