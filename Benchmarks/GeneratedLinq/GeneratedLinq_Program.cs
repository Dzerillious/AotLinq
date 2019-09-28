using System;

namespace Benchmarks
{
    public static partial class CompiledLinqClass
    {
        public static int[] CompiledLinq_CompiledLinqMethod15(int[] source)
        {
            var result = new int[source.Length];
            for (var i = 0; i < source.Length; i++)
                result[i] = source[i] + 2;
            
            return result;
        }
    }
}