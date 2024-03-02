namespace FuNK;

public static class MathExtensions
{
    public static float Clamp(this float val, float min, float max) => Math.Clamp(val, min, max);

    public static float Round(this float x, int places) => MathF.Round(x, places);

    public static float ConvertToDegree(this float rad) => 180 / MathF.PI * rad;

    public static float ConvertToRadians(this float angle) => MathF.PI / 180 * angle;
}
