using MonoTrail.code.rendering.data.camera;
using MonoTrail.code.tactics.data.turn;
using TrailCore.tactics.data;
using TrailCore.tactics.data.map;

namespace MonoTrail.code.tactics.data;

public record TacticsSceneData(
    MapData MapData,
    EntityData EntityData,
    TurnData TurnData,
    TileSetData TileSetData,
    SelectorData SelectorData,
    CameraData CameraData) : TrailCore.tactics.data.TacticsSceneData(MapData, EntityData);
