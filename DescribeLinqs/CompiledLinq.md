For AOT compilation, Compiled expression is simmilar
Created decorated object in ToArray() reflection call of generated method.

```cs
var result = Array.GetCompiledLinq(this)
			  .Select(x => x + 2)
			  .Select(x => x * x)
			  .ToArray();
```																																																																																																																																																																																																		
=>

```
var result = reflectcall (sourceArray = Array)
({
	var i = -1;		
	var length = sourceArray.Length;
	var resultArray = new T[length];
		
	while (++i < length)
	{
		var temp1 = sourceArray[i] + 2;
		var temp2 = temp1 * temp1;
		resultArray[i] = temp2;
	}
	return resultArray;
})
```				
