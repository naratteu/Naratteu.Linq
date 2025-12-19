namespace Naratteu.Linq;

public static class ArraySegmentExtensions
{
    public static ArraySegment<T> AsSegment<T>(this T[] arr) => arr;

    public static IEnumerable<ArraySegment<T>> SegmentBy<T>(this ArraySegment<T> seg) where T : IEquatable<T>
    {
        for (int cnt; seg is [var fst, ..]; seg = seg.Slice(cnt))
            yield return seg.Slice(0, cnt = 1 + seg.Slice(1).TakeWhile(fst.Equals).Count());
    }
    public static IEnumerable<ArraySegment<T>> SegmentBy<T, TT>(this ArraySegment<T> seg, Func<T, TT> match) where TT : IEquatable<TT>
    {
        for (int cnt; seg is [var fst, ..]; seg = seg.Slice(cnt))
            yield return seg.Slice(0, cnt = 1 + seg.Slice(1).Select(match).TakeWhile(match(fst).Equals).Count());
    }
    public static IEnumerable<ArraySegment<T>> SegmentBy<T>(this ArraySegment<T> seg, Func<T, T, bool> match)
    {
        for (int cnt; seg is [_, _, ..]; seg = seg.Slice(cnt))
            yield return seg.Slice(0, cnt = 1 + seg.Zip(seg.Slice(1)).TakeWhile(zip => match(zip.First, zip.Second)).Count());
        if (seg is not [])
            yield return seg;
    }
}