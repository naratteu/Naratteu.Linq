namespace Naratteu.Linq;

public static class ArraySegmentExtensions
{
    public static ArraySegment<T> AsSegment<T>(this T[] arr) => arr;

    public static IEnumerable<ArraySegment<T>> SegmentBy<T>(this ArraySegment<T> seg) where T : IEquatable<T>
        => SegmentBy((seg, i => i), i => i.Equals);
    public static IEnumerable<ArraySegment<T>> SegmentBy<T, TT>(this ArraySegment<T> seg, Func<T, TT> match) where TT : IEquatable<TT>
        => SegmentBy((seg, match), i => i.Equals);
    public static IEnumerable<ArraySegment<T>> SegmentBy<T>(this ArraySegment<T> seg, Func<T, T, bool> match)
        => SegmentBy((seg, i => i), match);
    //todo: Span<T>에 대한 SegmentBy

    extension<T, TT>((ArraySegment<T> seg, Func<T, TT> select) ss)
    {
        IEnumerable<ArraySegment<T>> SegmentBy(Func<TT, TT, bool> comp, SegmentationMode mode = SegmentationMode.Prev) => mode switch
        {
            SegmentationMode.Prev => ss.Segment(prev => new Prev<TT>(prev, comp)), // 권장
            SegmentationMode.Head => ss.Segment(head => new Head<TT>(data => comp(head, data))),
        };
        IEnumerable<ArraySegment<T>> SegmentBy(Func<TT, Func<TT, bool>> eqFactory, SegmentationMode mode = SegmentationMode.Head) => mode switch
        {
            SegmentationMode.Prev => ss.Segment(prev => new Prev<TT>(prev, (cache, data) => eqFactory(cache)(data))),
            SegmentationMode.Head => ss.Segment(head => new Head<TT>(eqFactory(head))), // 권장
        };
        IEnumerable<ArraySegment<T>> Segment<TTT>(Func<TT, TTT> getEquals) where TTT : IEq<TT>, allows ref struct
        {
            var (seg, select) = ss;
            return seg.Segment((in span, out cnt) =>
            {
                var len = span.Length;
                var equals = getEquals(select(span[0]));
                for (cnt = 1; cnt < len; cnt++)
                    if (equals.Equals(select(span[cnt])))
                        return true;
                return false;
            });
            //todo: select가 없는 경우도 포괄하는 일반적 최적화 구현. IEq 에서 처리하면 될듯?
        }
    }

    interface IEq<T> { bool Equals(T other); }
    ref struct Prev<T>(T cache, Func<T, T, bool> comp) : IEq<T> { bool IEq<T>.Equals(T data) => !comp(cache, cache = data); }
    readonly ref struct Head<T>(Func<T, bool> eq) : IEq<T> { readonly bool IEq<T>.Equals(T tt) => !eq(tt); }

    enum SegmentationMode { Prev, Head } //todo: 사용자선택 가능하도록 public 노출
    delegate bool Segmentation<T>(in ReadOnlySpan<T> span, out int cnt);
    static IEnumerable<ArraySegment<T>> Segment<T>(this ArraySegment<T> seg, Segmentation<T> doSegment)
    {
        if (seg is [_, ..])
            for (int cnt; doSegment(seg, out cnt); seg = seg.Slice(cnt))
                yield return seg.Slice(0, cnt);
        yield return seg;
    }
}