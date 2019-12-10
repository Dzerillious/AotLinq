For AOT compilation, Compiled expression is simmilar
Created decorated object in ToArray() reflection call of generated method.

+ very good performance
+ inlining expressions
+ chaining of queries is simple

- cannot extend query after creation
- difficult implementation
- must add .GetCompiledLinq(this) or .GetCompiledLinq(typeof(Type))
- slower compilation time
- initialization (in the start of application Dictionary with method definitions must be called)

```cs
var result = Array.GetCompiledLinq(this)
			  .Select(x => x + 2)
			  .Select(x => x * x)
			  .ToArray();
```																								
=>

```
var method = methodDictionary[key];
var result = call method (sourceArray = Array)
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
