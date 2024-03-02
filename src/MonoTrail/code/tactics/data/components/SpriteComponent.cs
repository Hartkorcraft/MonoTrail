using Microsoft.Xna.Framework;
using TrailCore.data.tactics;

namespace MonoTrail.code.tactics.data.components;

public record SpriteComponent : IComponent
{
    public const float TILE_SELECTOR_LAYER = 0.11f;
    public const float CHARACTER_LAYER = 0.1f;
    public const float WALL_LAYER = 0.09f;

    public required string SpriteName { get; init; }
    public required bool Visible { get; set; }
    public required float Layer { get; set; }
    public Color Color { get; set; } = Color.White;
}