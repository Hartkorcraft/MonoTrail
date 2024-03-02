using FUnK.Option.SubTypes;

namespace FUnK.Option;

public readonly record struct Option<T>(T Value, bool IsSome)
{
  private Option(T v) : this()
  {
    IsSome = true;
    Value = v;
  }

  public Option() : this(default, false)
  {
    Value = default;
    IsSome = false;
  }

  public bool TryMatch(out T value)
  {
    value = Value;
    return IsSome;
  }

  public bool TryCastOut<G>(out G val) where G : class
  {
    val = Value as G;
    return val is not null;
  }

  public static implicit operator Option<T>(None _)
      => new();

  public static implicit operator Option<T>(Some<T> some)
      => new(some.Value);

  public static implicit operator Option<T>(T value)
      => value == null
          ? FuNk.None
          : FuNk.Some(value);

  public R Match<R>(Func<R> None, Func<T, R> Some)
      => IsSome
          ? Some(Value)
          : None();

  public T Match(Func<T> None)
      => IsSome
          ? Value
          : None();

  public R Match<R>(R None, Func<T, R> Some)
      => IsSome
          ? Some(Value)
          : None;

  public R Match<R>(R None, R Some)
      => IsSome
          ? Some
          : None;

  public T MatchOr(Func<T> None)
      => IsSome
          ? Value
          : None();

  public T Or(T OnNone)
      => IsSome
          ? Value
          : OnNone;

  public void MatchEffect(Action None, Action<T> Some)
  {
    if (IsSome) Some(Value);
    else None();
  }

  public void MatchEffect(Action None)
  {
    if (IsSome is false) None();
  }

  public void MatchEffect(Action<T> Some)
  {
    if (IsSome) Some(Value);
  }

  public void MatchEffect(Func<T, bool> condition, Action<T> effect)
  {
    if (IsSome && condition(Value)) effect(Value);
  }

  public override string ToString() => "Option " + (IsSome ? Value?.ToString() ?? throw new Exception("can't be null") : "{None}");
}

public readonly record struct Binder<T, R>(Func<T, R> Bind)
{
  private static Option<T> optCache;

  public R Match(Func<R> onNone, Func<T, R> onSome) => optCache.Match(onNone, onSome);

  public static Option<R> operator |(Option<T> opt, Binder<T, R> f)
  {
    optCache = opt;
    return opt.Bind(f.Bind);
  }

  public static implicit operator Func<T, R>(Binder<T, R> b) => b.Bind;
  public static implicit operator Binder<T, R>(Func<T, R> f) => new(f);
}

// public static class Lol
// {
//     public static void XD()
//     {
//         var maybeInt = Some(10);
//         var maybeResult = maybeInt | (Binder<int, string>)(number => (number * 2) + "lol")
//                                    | (Binder<string, int>)(text => text.Select(c => c + "lol").Count());

//         var result = maybeResult.Match(
//             () => 0,
//             text => text);
//     }
// }
