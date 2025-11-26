namespace Naratteu.Linq;

public static partial class WhereThen
{
    public static IEnumerable<TResult> Where<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Pair<TResult>> predicate)
    {
        foreach (var item in source)
            if (predicate(item) is (true, var value))
                yield return value;
    }
    public static Pair<TResult>.ValueOnly Then<TResult>(out TResult value)
    {
        System.Runtime.CompilerServices.Unsafe.SkipInit(out value);
        return new(value);
    }
    public record struct Pair<T>(bool Condition, T Value)
    {
        public record struct ValueOnly(T Value)
        {
            public static Pair<T> operator |(bool l, ValueOnly r) => new(l, r.Value);
        }
    }
}