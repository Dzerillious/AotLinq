# AOTLinq
Ahead of time (AOT) compilation of Linq. In .NET Core 3.0 was Linq optimized. But still it does not support method inlining, because it is cast into delegate and for each element and for each linq operation callvirt instruction must be called. And for low cost operations it slows down a lot calculation of select.

To inline methods you have two options. You must compile lambda expressions and then invoke them or AOT compile linq. 

Advantages and disadvantages of Lambda expressions are:
  + Compiled almost as fast as for cycle
  + Accepts lambdas
  - When lambda cannot be converted to expression a great slowdown
  - Method is compiled to dynamic method, which is slightly slower
  - Expressions are not know between common programmers
  - Very expensive initialization (can be 10000x duration of simple linq)
  
When compared to aot compilation:
  + AOT as fast as for cycle (in current version is not because of reflection)
  + Accepts everything that is supported (Can be lambdas, delegates)
  + When is not lambda, but delegate classic call is called
  + Compiled to classic IL
  + Initialization is done when compiling. Running application is not affected by it
  - Harder to code
  - Slowing compilation
  - Not that transparent and must provide own build errors
  
When testing current version it is almost everytime faster than NET Linq. Just Mono is slow (maybe because of reflection) and CoreRT could not be run (i will try to get it running). But large performance boost is detected both in CLR and CoreCLR.

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i7-8705G CPU 3.10GHz (Kaby Lake G), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.8.4010.0
  DefaultJob : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.8.4010.0

|        Method |       Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-----------:|----------:|----------:|-------:|------:|------:|----------:|
|  CompiledLinq | 1,030.0 ns |  4.164 ns |  3.691 ns | 1.0338 |     - |     - |   4.24 KB |
|       ForLinq |   635.7 ns |  4.015 ns |  3.353 ns | 0.9613 |     - |     - |   3.94 KB |
|  NetArrayLinq | 7,989.7 ns | 58.409 ns | 54.636 ns | 2.9755 |     - |     - |  12.22 KB |
|   NetListLinq | 9,673.0 ns | 58.308 ns | 48.690 ns | 2.0447 |     - |     - |    8.4 KB |
|  FastListLinq | 1,665.3 ns |  3.668 ns |  3.252 ns | 0.9956 |     - |     - |   4.08 KB |


================================================================================================


BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i7-8705G CPU 3.10GHz (Kaby Lake G), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100
  [Host]     : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.6 (CoreCLR 4.6.27817.03, CoreFX 4.6.27818.02), 64bit RyuJIT

|        Method |       Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-----------:|----------:|----------:|-------:|------:|------:|----------:|
|  CompiledLinq |   941.0 ns |  5.455 ns |  5.103 ns | 1.0233 |     - |     - |    4.2 KB |
|       ForLinq |   515.1 ns |  2.660 ns |  2.489 ns | 0.9584 |     - |     - |   3.93 KB |
|  NetArrayLinq | 1,961.1 ns | 15.589 ns | 14.582 ns | 0.9689 |     - |     - |   3.98 KB |
|   NetListLinq | 2,507.6 ns | 22.127 ns | 20.698 ns | 0.9995 |     - |     - |    4.1 KB |
|  FastListLinq | 1,670.7 ns |  5.771 ns |  5.116 ns | 0.9918 |     - |     - |   4.07 KB |


================================================================================================


BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i7-8705G CPU 3.10GHz (Kaby Lake G), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100
  [Host]     : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT
  DefaultJob : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT
  
|        Method |       Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-----------:|----------:|----------:|-------:|------:|------:|----------:|
|  CompiledLinq |   913.3 ns |  6.182 ns |  5.480 ns | 1.0262 |     - |     - |    4.2 KB |
|       ForLinq |   628.8 ns |  4.313 ns |  4.034 ns | 0.9613 |     - |     - |   3.93 KB |
|  NetArrayLinq | 2,199.9 ns | 10.506 ns |  9.827 ns | 0.9727 |     - |     - |   3.98 KB |
|   NetListLinq | 2,660.5 ns | 25.998 ns | 23.046 ns | 0.9995 |     - |     - |   4.09 KB |
|  FastListLinq | 1,983.6 ns | 39.347 ns | 68.912 ns | 0.9956 |     - |     - |   4.07 KB |


================================================================================================


BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i7-8705G CPU 3.10GHz (Kaby Lake G), 1 CPU, 8 logical and 4 physical cores
  [Host] : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.8.4010.0
  Mono   : Mono 6.4.0 (Visual Studio), 64bit

Job=Mono  Runtime=Mono
|        Method |     Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |---------:|----------:|----------:|-------:|------:|------:|----------:|
|  CompiledLinq | 3.462 us | 0.0181 us | 0.0160 us | 1.0452 |     - |     - |         - |
|       ForLinq | 1.077 us | 0.0103 us | 0.0096 us | 0.9823 |     - |     - |         - |
|  NetArrayLinq | 2.392 us | 0.0181 us | 0.0169 us | 0.9804 |     - |     - |         - |
|   NetListLinq | 5.139 us | 0.0599 us | 0.0560 us | 1.0300 |     - |     - |         - |
|  FastListLinq | 2.661 us | 0.0304 us | 0.0284 us | 1.0262 |     - |     - |         - |	

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i7-8705G CPU 3.10GHz (Kaby Lake G), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100
  [Host] : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT
  CoreRT : .NET CoreRT 1.0.28124.02 @BuiltBy: dlab14-DDVSOWINAGE101 @Branch: master @Commit: 49b9984a47732f8e3f5a74b02d5732424735e2a6, 64bit AOT

Job=CoreRT  Runtime=CoreRT

|        Method |       Mean |     Error |    StdDev |
|-------------- |-----------:|----------:|----------:|
|  CompiledLinq |         NA |        NA |        NA |
|       ForLinq |   550.2 ns |  5.023 ns |  4.452 ns |
|  NetArrayLinq | 8,959.3 ns | 43.556 ns | 40.743 ns |
|   NetListLinq | 8,953.1 ns | 59.659 ns | 55.805 ns |

For clear showing what is done in each method we will define new syntax for method calling.
