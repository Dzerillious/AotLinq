Linq which immediately executes
Implemented in https://github.com/jackmott/LinqFaster/tree/master/LinqFaster

+ good performance for simple queries
+ simple implementation and usage
+ not creating wrap object
- large memory usage for chained query (creating array for every layer)
- not inlining expressions

```cs
var result = Array.SelectF(x => x + 2)
			  .SelectF(x => x * x);
```

=>

```cs
var temp = call (sourceArray = Array)
({
	int i = -1;
	var length = sourceArray.Length;
	var resultArray = new T[length];
	
	while (++i < length)
	{
		resultArray[i] = .virtcall(input = sourceArray[i]);
		({
			return item + 2;
		})
	}
})
var result = call (sourceArray = temp)
({
	int i = -1;
	var length = sourceArray.Length;
	var resultArray = new T[length];
	
	while (++i < length)
	{
		resultArray[i] = .virtcall(input = sourceArray[i]);
		({
			return item * item;
		})
	}
})
```

				
