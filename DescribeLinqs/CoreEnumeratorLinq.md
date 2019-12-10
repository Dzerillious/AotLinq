.NET Core 3.0 Enumerator implementation (without initialization)
Decorated object on ToArray() executed

+ quite good performance
+ simple usage
- not inlining expressions
- only for .NET Core 3.0

```cs
var result = Array.Select(x => x + 2)
			  .Select(x => x * x)
			  .ToArray();
```

=>

```cs
var result = call (sourceArray = Array)
({
	var i = -1;		
	var resultArray = new T[1];
	var length = sourceArray.Length;
		
	while (++i < length)
	{
		resultArray[i] = .virtcall(input = sourceArray[i])
		({
			var temp = .virtcall(input = temp1)
			({
				return input + 2;
			})
			return temp + 2;
		})
	}
	return resultArray;
})
```

