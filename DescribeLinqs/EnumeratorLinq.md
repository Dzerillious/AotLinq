.NET Framework 4.6.2 Enumerator implementation (Default enumerator is more complex, without initialization)
Goes from top to bottom and back, asks if has next item and then gets item

+ simple implementation
+ easily extended
- bad performance

```cs
var result = Array.Select(x => x + 2)
			  .Select(x => x * x)
			  .ToArray;
```

=>

```cs
this.select1 = Array.Select(x => x + 2);
this.select2 = select1.Select(x => x * x);

var result = call
({
	var i = -1;		
	var resultArray = new T[1];
		
	while (true)
	{
		var valid = this.select2.call
		({
			var valid = this.select1.call
			({
				if (++index == this.Array.Length) return false;
				this.Current = virtcall(input = this.Array[index]) 
				({
					return input+ 2;
				});
				return true;
			})
			
			if (!valid) return false;
			Current = virtcall(input = this.select1.Current) 
			({
				return input * input;
			})
			return true;
		})
		
		if (!valid) break;
		if (++i == resultArray.Length)
		{
			var tempArray = new T[this.resultArray.Length * 2];
			Array.Copy(resultArray, 0, tempArray, 0, resultArray.Length);
			resultArray = tempArray;
		}
		resultArray[i] = this.select2.Current;
	}
	return resultArray;
})
```
