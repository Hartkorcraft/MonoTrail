using MonoTrail.code.tactics.content;
using MonoTrail.code.tactics.data.components;
using MonoTrail.code.tactics.logic;
using TiledCS;
using TrailCore.tactics.data;
using TrailCore.tactics.data.components;
using TrailCore.tactics.data.map;
using TrailCore.tactics.initialization;

namespace MonoTrail.code.tactics.initialization;

public static class EntityObjectInitialization
{
  const int tileSize = 8;

  public static TacticsInitData CreateDefaultPlayerObject(this TacticsSceneData tacticsData, TiledObject tiledObject) => tacticsData
      .CreateDefaultPlayer(tiledObject.MapPosFromTiledObject(tileSize))
      .AddComponent<MindComponent>(data => new() { Name = "Player", FactionAligment = FactionAligment.Player })
      .AddComponent<HighlightComponent>(data => new(DisplayInfo: true))
      .AddComponent<OffsetComponent>(data => new([]))
      .AddComponent<FovComponent>(data => new())
      .AddComponent<SpriteComponent>(data => new()
      {
        Layer = SpriteComponent.CHARACTER_LAYER,
        Visible = true,
        SpriteName = TextureNames.PlayerTex
      });

  public static TacticsInitData AddWall(this TacticsSceneData tacticsData, WallInfo wallInfo) => tacticsData
      .CreateDefaultWall(wallInfo)
      .AddComponent<OffsetComponent>(data => new([]))
      .AddComponent<SpriteComponent>(data => new()
      {
        SpriteName = TextureNames.WallTex,
        Layer = SpriteComponent.CHARACTER_LAYER,
        Visible = true
      });
  //.AddComponent<AreaComponent>(data => new(Shape: GetDefaultShape(), CollisionMask: CollisionMask.Wall))
  //.AddComponent<HealthComponent>(data => new(Current: 2, Max: 2));

  private static GridPos MapPosFromTiledObject(this TiledObject obj, int tileSize)
      => (obj.x, obj.y).WorldToMap(tileSize);
}
