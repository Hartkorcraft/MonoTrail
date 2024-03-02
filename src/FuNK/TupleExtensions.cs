using FuNK;

namespace FuNK;

public static class TupleExtensions
{
    public static T Multiply<T>(this (T x, T y) values) where T : System.Numerics.INumber<T>
        => values.x * values.y;

    public static (T, T) Multiply<T>(this (T x, T y) values, T by) where T : System.Numerics.INumber<T>
        => (values.x * by, values.y * by);

    public static (T, T) Divide<T>(this (T x, T y) values, T value) where T : System.Numerics.INumber<T>
        => (values.x / value, values.y / value);

    public static (float x, float y) Multiply(this (float x, float y) values, int valueX, int valueY)
        => (values.x * valueX, values.y * valueY);

    public static (T x, T y) Subtract<T>(this (T x, T y) values, (T x, T y) minus) where T : System.Numerics.INumber<T>
        => (values.x - minus.x, values.y - minus.y);

    public static bool BoundsCheckFail(this (int x, int y) pos, (int x, int y) bounds)
        => pos.x < 0 || pos.x >= bounds.x || pos.y < 0 || pos.y >= bounds.y;

    public static (int x, int y) Add(this (int x, int y) vec1, (int x, int y) vec2)
        => (vec1.x + vec2.x, vec1.y + vec2.y);

    public static (int x, int y) Subtract(this (int x, int y) vec1, (int x, int y) vec2)
        => (vec1.x - vec2.x, vec1.y - vec2.y);

    public static (int x, int y) Abs(this (int x, int y) vec)
        => (Math.Abs(vec.x), Math.Abs(vec.y));

    public static T Merge<T>(this (T t1, T t2) tuple, Func<T, T, T> merger)
        => merger(tuple.t1, tuple.t2);

    public static (T, T) Dupe<T>(this T value)
        => (value, value);

    public static int GridDistance(this (int x, int y) pos1, (int x, int y) pos2)
        => pos2.Subtract(pos1).Abs().Merge(static (x, y) => x + y);
}
