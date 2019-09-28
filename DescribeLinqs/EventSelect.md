Event based Linq which would be used for FastLinq in not optimized routes (where would be used decorator and for ForLinq)
Delegate assigned to parent next action is invoked from bottom to top

```cs
var result = Array.Select(x => x + 2)
			  .Select(x => x * x)
			  .ToArray;
```

=>

```cs
sourceArray
({
	var i = -1;		
	var length = sourceArray.Length;
	var resultArray = new T[length];
		
	while (++i < length)
	{
		select1 .virtcall(input = sourceArray[i])
		({
			var item = .virtcall(input = input)
			({
				return input + 2;
			})
			select2.NextAction(input = item)
			({
				var item = .virtcall(input = input)
				({
					return input * input;
				})
				.virtcall (input = item)
				({
					resultArray[i] = input;
				})
			})
		})
	}
	return resultArray;
})
```