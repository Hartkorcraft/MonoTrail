
namespace FUnK.Option;

public static class OptionExtensions
{
  public static readonly Action NoAction = () => { };

  public static Option<T>[] ToArray<T>(this Option<T> opt) => [opt];

  public static Option<T> ToOption<T>(this T value) => value;

  public static Option<R> Bind<T, R>(this Option<T> optT, Func<T, Option<R>> f)
      => optT.Match(
          () => FuNk.None,
          (t) => f(t));

  public static void MapEffect<T>(this Option<T> optT, Action<T> f)
      => optT.MatchEffect((t) => f(t));

  public static void MapEffect<T>(this Option<T> optT, Action f)
      => optT.MatchEffect(f, (_) => NoAction());

  public static Option<R> Bind<T, R>(this Option<T> optT, Func<T, Some<R>> f)
      => optT.Match(
          () => FuNk.None,
          (t) => (Option<R>)f(t));

  public static void MapEffect<T, R>(this (Option<T> opt1, Option<R> opt2) options, Action<T, R> f)
      => options.opt1.MapEffect(x => options.opt2.MapEffect(y => f(x, y)));

  public static Option<T> BindOnNone<T>(this Option<T> optT, Func<Option<T>> onNone)
  => optT.Match(
      () => onNone(),
      (t) => t);

  public static Option<R> Bind<T, R>(this Option<T> optT, Func<T, R> f)
      => optT.Match(
          () => FuNk.None,
          (t) => (Option<R>)f(t));

  public static Option<T> Where<T>(this Option<T> opt, Predicate<T> predicate) => opt.Bind<T, T>(x => predicate(x) ? x : FuNk.None);
  public static Option<T> Where<T>(this Option<T> opt, bool predicate) => opt.Bind<T, T>(x => predicate ? x : FuNk.None);
  public static Option<T> Where<T>(this T value, bool predicate) => predicate ? value : FuNk.None;
  public static Option<T> WhereOpt<T>(this T value, Predicate<T> predicate) => predicate(value) ? value : FuNk.None;
  public static bool Validate<T>(this Option<T> opt, Predicate<T> predicate) => opt.IsSome && predicate(opt.Value);

  public static Option<T> TryBind<T>(this Option<T> optT, out T value)
  {
    value = optT.Value;
    return optT;
  }

  public static (Option<T>, Option<R>) Compress<T, R>(this Option<(T, R)> opt)
      => opt.Match<(Option<T>, Option<R>)>(() => (FuNk.None, FuNk.None), (t) => (t.Item1, t.Item2));

  public static Option<(T, R)> Expand<T, R>(this (Option<T> t, Option<R> r) opt)
      => opt.t.Match(
          () => FuNk.None,
          t => opt.r.Match<Option<(T, R)>>(
              () => FuNk.None,
              r => (t, r)));

  public static Option<(T, R)> Expand<T, R>(this Option<(Option<T> t, Option<R> r)> opt)
      => opt.Match(
          () => FuNk.None,
          g => g.t.Match(
              () => FuNk.None,
              r => g.r.Match<Option<(T, R)>>(
                  () => FuNk.None,
                  z => (r, z))));

  public static Option<(T, R, G)> Expand<T, R, G>(this (Option<T> t, Option<R> r, Option<G> g) opt)
      => opt.t.Match(
          () => FuNk.None,
          t => opt.r.Match(
              () => FuNk.None,
              r => opt.g.Match<Option<(T, R, G)>>(
                  () => FuNk.None,
                  g => (t, r, g))));

  public static Option<(T, R, G, U)> Expand<T, R, G, U>(this (Option<T> t, Option<R> r, Option<G> g, Option<U> u) opt)
  {
    return opt.t.Match(
            () => FuNk.None,
            t => opt.r.Match(
                () => FuNk.None,
                r => opt.g.Match(
                    () => FuNk.None,
                    g => opt.u.Match<Option<(T, R, G, U)>>(
                        () => FuNk.None,
                         u => (t, r, g, u)))));
  }

  public static Option<R> BindOption<T, R>(this Option<T> optT, Func<T, Option<R>> f)
      => optT.Match(
          () => FuNk.None,
          (t) => f(t));

  public static Option<R> AsOptionCast<T, R>(this T value) => value is R ? (R)(object)value : FuNk.None;

  public static Option<T> FirstOrNone<T>(this IEnumerable<T> collection) => collection.Any() ? collection.First() : FuNk.None;

  public static Option<T> FirstOrNone<T>(this IEnumerable<T> collection, Func<T, bool> where) => collection.Any(where) ? collection.First() : FuNk.None;

  //public static Option<T> BindFirstOrNone<T>(this IEnumerable<T> collection, Func<T, bool> where) => collection.Any(where) ? collection.First() : FuNk.None;

  public static Option<R> Map<T, R>(this Option<T> optT, Func<T, R> f)
      => optT.Match<Option<R>>(
          () => FuNk.None,
          t => FuNk.Some(f(t)));
}
