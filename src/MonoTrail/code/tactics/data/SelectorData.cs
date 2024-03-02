namespace MonoTrail.code.tactics.data;

public record SelectorData()
{
    public GridPos GridPos { get; set; }
    public bool Show { get; set; }
    public Vec2 Offset { get; set; }
}
