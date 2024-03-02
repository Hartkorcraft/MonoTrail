namespace TrailCore.tactics.ext;

public static class GridExtensions
{
    public static readonly GridPos Up = (0, -1);
    public static readonly GridPos Down = (0, 1);
    public static readonly GridPos Left = (-1, 0);
    public static readonly GridPos Right = (1, 0);

    public static float GetDistance(this GridPos pos1, GridPos pos2)
        => MathF.Sqrt(MathF.Pow(pos2.x - pos1.x, 2) + MathF.Pow(pos2.y - pos1.y, 2));
}
