using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TrailCore.data.tactics.map;

namespace MonoTrail.code.tactics.data;

public record TileSetData(
    string Name,
    int TileSize,
    Dictionary<GroundType, Rectangle> TileSet
);
