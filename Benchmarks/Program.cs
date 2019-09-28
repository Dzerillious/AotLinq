using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    public static partial class CompiledLinqClass
    {
        public static int[] CompiledLinqMethod(int[] source)
        {
            return Program.ArraySource
                .GetCompiledLinq(typeof(CompiledLinqClass))
                .Select(x => x + 2)
                .ToArray();
        }
    }
    
    [ClrJob/*ClrJob, CoreJob, MonoJob, CoreRtJob*/]
    public partial class Program
    {
        public static readonly List<int> ListSource = Enumerable.Range(0, 1000).ToList();
        public static readonly int[] ArraySource = Enumerable.Range(0, 1000).ToArray();
        public static Program ProgramInstance = new Program();
        public const int GreaterThan = 1000;

        [Benchmark]
        public void CompiledLinq()
        {
            var result = CompiledLinqClass.CompiledLinqMethod(Program.ArraySource);
        }
//        
        [Benchmark]
        public void ForLinq()
        {
            var result = ForLinqMethod(Program.ArraySource);
        }
        
        public static int[] ForLinqMethod(int[] source)
        {
            var result = new int[source.Length];
            for (var i = 0; i < source.Length; i++)
                result[i] = source[i] + 2;
            
            return result;
        }

        [Benchmark]
        public void Net3ArrayLinq()
        {
            Program.ArraySource
                .Select(x => x + 2)
                .ToArray();
        }
        
        [Benchmark]
        public void Net3ListLinq()
        {
            Program.ListSource
                .Select(Select)
                .ToList();

            int Select(int input) => input + 2;
        }

        static void Main(string[] args)
        {
            new Program().Net3ArrayLinq();
            BenchmarkSwitcher.FromTypes(new[] {typeof(Program)}).Run(args);
        }
    }

    public static class Extensions
    {
        public static ICompiledLinq<T> GetCompiledLinq<T>(this T[] array,
            Type callerType,
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new CompiledLinq<T>(callerType, array, memberName, sourceLineNumber);
        }
        
        public static ICompiledLinq<T> GetCompiledLinq<T>(this T[] array,
            object callerObject,
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new CompiledLinq<T>(callerObject, array, memberName, sourceLineNumber);
        }

        public interface ICompiledLinq<T>
        {
            ICompiledLinq<TResult> Select<TResult>(Func<T, TResult> select);
            ICompiledLinq<T> Where(Func<T, bool> where);
            T[] ToArray();
        }

        public class CompiledLinqData
        {
            public readonly object CallerObject;
            public readonly Type CallerObjectType;
            public readonly object Source;
            public readonly string MemberName;
            public readonly int SourceLineNumber;

            public CompiledLinqData(object callerObject, object source, string memberName, int sourceLineNumber)
            {
                CallerObject = callerObject;
                Source = source;
                MemberName = memberName;
                SourceLineNumber = sourceLineNumber;
            }

            public CompiledLinqData(Type callerObjectType, object source, string memberName, int sourceLineNumber)
            {
                CallerObjectType = callerObjectType;
                Source = source;
                MemberName = memberName;
                SourceLineNumber = sourceLineNumber;
            }
        }
        
        public class CompiledLinq<T> : ICompiledLinq<T>
        {
            private CompiledLinqData _data;
            public CompiledLinq(Type callerObjectType, T[] source, string memberName, int sourceLineNumber)
                => _data = new CompiledLinqData(callerObjectType, source, memberName, sourceLineNumber);
            public CompiledLinq(object callerObject, T[] source, string memberName, int sourceLineNumber)
                => _data = new CompiledLinqData(callerObject, source, memberName, sourceLineNumber);

            private CompiledLinq(CompiledLinqData data)
                => _data = data;

            public ICompiledLinq<TResult> Select<TResult>(Func<T, TResult> select)
                => new CompiledLinq<TResult>(_data);
            
            public ICompiledLinq<T> Where(Func<T, bool> where)
                => new CompiledLinq<T>(_data);

            public T[] ToArray() => (T[])InvokeLinq();

            public object InvokeLinq()
            {
                var data = _data;
                if (data.CallerObjectType != null)
                {
                    var methodName = $"CompiledLinq_{data.MemberName}{data.SourceLineNumber}";
                    var compiledLinq = data.CallerObjectType.GetMethod(methodName);
                    return compiledLinq.Invoke(this, new []{data.Source});
                }
                else
                {
                    var callerType = data.CallerObject.GetType();
                    var methodName = $"CompiledLinq_{data.MemberName}{data.SourceLineNumber}";
                    var compiledLinq = callerType.GetMethod(methodName);
                    return compiledLinq.Invoke(data.CallerObject, new []{data.Source});
                }
            }
        }
    }
}