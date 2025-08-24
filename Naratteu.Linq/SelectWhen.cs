namespace Naratteu.Linq;

public static class SelectWhen
{
    public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Pair<TResult>> selector)
    {
        foreach (var item in source)
            if (selector(item) is (true, var value))
                yield return value;
    }
    public static Pair<TResult> When<TResult>(bool condition, out TResult value)
    {
        System.Runtime.CompilerServices.Unsafe.SkipInit(out value);
        return new(condition, value);
    }
    public record struct Pair<T>(bool Condition, T Value);
}