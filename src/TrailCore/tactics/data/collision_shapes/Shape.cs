namespace TrailCore.tactics.data.collision_shapes;

public record Shape
{
    public required ShapeType ShapeType { get; init; }
    public required float Size { get; set; }
}
