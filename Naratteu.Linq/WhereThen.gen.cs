namespace Naratteu.Linq;

partial class WhereThen
{
    public static TResult First<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Pair<TResult>> predicate) => source.Where(predicate).First();
    public static TResult FirstOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Pair<TResult>> predicate) => source.Where(predicate).FirstOrDefault();
    public static TResult Last<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Pair<TResult>> predicate) => source.Where(predicate).Last();
    public static TResult LastOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Pair<TResult>> predicate) => source.Where(predicate).LastOrDefault();
    public static TResult Single<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Pair<TResult>> predicate) => source.Where(predicate).Single();
    public static TResult SingleOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Pair<TResult>> predicate) => source.Where(predicate).SingleOrDefault();
}