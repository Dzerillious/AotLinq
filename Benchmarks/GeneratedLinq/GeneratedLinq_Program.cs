using System;

namespace Benchmarks
{
    public partial class Program
    {
        public static int[] CompiledLinq_CompiledLinq21(int[] source)
        {
            var result = new int[source.Length];
            for (var i = 0; i < source.Length; i++)
                result[i] = source[i] + 2;
            
            return result;
        }
    }
}