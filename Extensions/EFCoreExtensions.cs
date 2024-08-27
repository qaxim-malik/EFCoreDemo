namespace EFCoreDemo.Extensions;

public static class EFCoreExtensions
{
    public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
    {
        if (condition)
            return source.Where(predicate);
        else
            return source;
    }
}