namespace TrailCore.tactics.data;

public record EntityID(Guid ID)
{
    public static EntityID New() => new(Guid.NewGuid());
}
