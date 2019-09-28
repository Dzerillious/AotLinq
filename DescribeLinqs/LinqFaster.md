Linq which immediately executes
Implemented in https://github.com/jackmott/LinqFaster/tree/master/LinqFaster

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

				
