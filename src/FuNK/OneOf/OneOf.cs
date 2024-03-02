using System;

namespace FUnK.OneOf;

public readonly struct OneOf<L, R>
{
  public readonly bool IsRight { get; }
  public readonly bool IsLeft => !IsRight;

  public readonly L Left;
  public readonly R Right;

  OneOf(L leftValue)
  {
    IsRight = false;
    Left = leftValue;
    Right = default;
  }

  OneOf(R rightValue)
  {
    IsRight = true;
    Right = rightValue;
    Left = default;
  }

  public static implicit operator OneOf<L, R>(L left) => new(left);
  public static implicit operator OneOf<L, R>(R right) => new(right);

  public static implicit operator OneOf<L, R>(OneOf.Left<L> left) => new(left.Value);
  public static implicit operator OneOf<L, R>(OneOf.Right<R> right) => new(right.Value);

  public readonly TR Match<TR>(Func<L, TR> Left, Func<R, TR> Right)
     => IsLeft ? Left(this.Left ?? throw new Exception("bruh")) : Right(this.Right ?? throw new Exception("bruh"));

  public readonly void MatchEffect(Action<L> l, Action<R> r)
  {
    if (IsLeft) l(Left);
    else r(Right);
  }

  // new Action((x) => l(x)).ToFunc(), new Action((x) => r(x)).ToFunc());

  // public override string ToString() => Match(l => $"Left({l})", r => $"Right({r})");
}

public static class OneOf
{
  public readonly struct Left<L>
  {
    internal L Value { get; }
    internal Left(L value) { Value = value; }

    public override string ToString() => $"Left({Value})";
  }

  public readonly struct Right<R>
  {
    internal R Value { get; }
    internal Right(R value) { Value = value; }

    public override string ToString() => $"Right({Value})";
  }
}

// public static class EitherExtensions
// {
//     public static Either<L, RR> Map<L, R, RR>(this Either<L, R> either, Func<R, RR> f)
//         => either.Match<Either<L, RR>>(
//         l => Left(l),
//         r => Right(f(r)));

//     public static Either<L, Unit> ForEach<L, R>(this Either<L, R> either, Action<R> act)
//         => Map(either, act.ToFunc());

//     public static Either<L, RR> Bind<L, R, RR>(this Either<L, R> either, Func<R, Either<L, RR>> f)
//         => either.Match(
//         l => Left(l),
//         r => f(r));
// }
