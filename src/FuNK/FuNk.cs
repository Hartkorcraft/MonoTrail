namespace FUnK;

public static partial class FuNk
{
  public static readonly Unit Unit;

  public static readonly Action EmptyAction = () => { };

  public static Func<Unit> ToFunc(this Action a) => () => { a(); return default; };

  public static T GetOut<T>(this T t, out T tOut) => tOut = t;

  public static void Effect<T>(this IEnumerable<T> list, Action<T> effect)
  {
    foreach (var x in list)
      effect(x);
  }

  public static void Effect(this Action effect) => effect();

  public static void EffectOnTrue(this bool isTrue, Action effect)
  {
    if (isTrue)
      effect();
  }

  public static void EffectOnFalse(this bool isTrue, Action effect)
  {
    if (isTrue is false)
      effect();
  }

  public static void Effect<T>(this T val, Action<T> effect) => effect(val);

  public static void Effect<T>(this T val, Predicate<T> when, Action<T> work)
  {
    if (when(val))
      work(val);
  }

  public static void Effect<T>(this IEnumerable<T> list, Predicate<T> when, Action<T> work)
  {
    foreach (var x in list)
      if (when(x)) work(x);
  }

  public static bool TryCast<G, T>(this G maybeT, out T t) where T : class
  {
    t = maybeT as T;
    return t is not null;
  }

  public static T Get<T>(this object _, T value) => value;
  public static T Get<T>(this object _, Func<T> getValue) => getValue();

  // public static TResult SelectResult<TSource, TResult>(this TSource source, Func<TSource, int, TResult> selector);
}
