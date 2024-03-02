using FuNK;
using TrailCore.tactics.data;
using TrailCore.tactics.data.components;

namespace TrailCore.extensions;

public static class MapComponentExtensions
{
  public static GridPos GetRelativeGridPos(this EntityID e, GridPos dir, TacticsSceneData t)
    => t.EntityData.ComponentStorage.Get<MapPosComponent>(e).MapPos.Add(dir);
}
