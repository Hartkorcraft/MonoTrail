
namespace FuNK.collections;

public static class IEnumerableExtensions
{
  public static T NextInList<T>(this IEnumerable<T> list, T value)
    => list.Any() is false ? default : list
        .SkipWhile(x => x.Equals(value) == false)
        .Skip(1)
        .DefaultIfEmpty(list.First())
        .FirstOrDefault();

  public static T PrevInList<T>(this IEnumerable<T> list, T value)
    => list.Any() is false ? default : list
      .TakeWhile(x => x.Equals(value) == false)
      .DefaultIfEmpty(list.Last())
      .LastOrDefault();

  public static bool AnyOut<T>(this IEnumerable<T> list, Func<IEnumerable<T>, T> getValue, out T value)
  {
    var hasValue = list.Any();
    value = hasValue ? getValue(list) : default;
    return hasValue;
  }

  public static void Foreach<T>(this IEnumerable<T> list, Action<T> act)
  {
    foreach (var item in list)
      act(item);
  }

  public static void ForeachAct(this IEnumerable<Action> actions)
  {
    foreach (var act in actions)
      act();
  }

  public static void Foreach<T>(this IEnumerable<T> list, Action<T, int> act)
  {
    var i = 0;
    foreach (var (item, index) in list.Select(x => (x, i++)))
    {
      act(item, index);
    }
  }

  public static bool ContainsAny<T>(this IEnumerable<T> list, IEnumerable<T> any)
  {
    foreach (var contains in any)
      if (list.Contains(contains) is false)
        return false;
    return true;
  }
}
