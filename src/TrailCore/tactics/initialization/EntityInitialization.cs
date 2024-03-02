using TrailCore.tactics.data;
using TrailCore.tactics.data.collision_shapes;
using TrailCore.tactics.data.components;
using TrailCore.tactics.data.map;
using TrailCore.tactics.logic;

namespace TrailCore.tactics.initialization;

public static class EntityInitialization
{
  public static Shape GetDefaultShape() => new() { ShapeType = ShapeType.Square, Size = 4 };

  public static TacticsInitData CreateDefaultPlayer(this TacticsSceneData tacticsData, (int x, int y) mapPos) => tacticsData
      .AddCharacter(mapPos);

  public static TacticsInitData CreateDefaultWall(this TacticsSceneData tacticsData, WallInfo wallInfo) => tacticsData
      .AddMapEntity(wallInfo.MapPos)
      .AddComponent<HealthComponent>(data => new(Current: 2, Max: 2));


  // .AddComponent<MindComponent>(data => new()
  // {
  //     Name = "Player TODO",
  //     TurnType = GetTurnType(tiledObject),
  //     TurnPoints = new(Current: 100, Max: 100),
  //     Brain = new Brain(Behaviour.All)
  // })
  // .AddComponent<SpriteComponent>(data => new()
  // {
  //     Layer = SpriteComponent.CHARACTER_LAYER,
  //     SpriteName = SpriteComponent.PlayerTex,
  //     Visible = true
  // })
  // .AddComponent<InventoryComponent>(data => new() { ItemInHand = ItemFactory.Rifle });

  // public static TacticsInitData CreateDefaultDziaders(this TacticsInitData tacticsData, TiledObject tiledObject)
  // {
  //     return tacticsData
  //         .AddCharacter(tiledObject)
  //         .AddComponent<MindComponent>(data => new()
  //         {
  //             Name = "Dziaders",
  //             TurnType = GetTurnType(tiledObject),
  //             TurnPoints = new(Current: 30, Max: 30),
  //             Brain = new Brain(Behaviour.All)
  //         })
  //         .AddComponent<SpriteComponent>(data => new()
  //         {
  //             Layer = SpriteComponent.CHARACTER_LAYER,
  //             SpriteName = TextureNames.DziadersTex,
  //             Visible = true
  //         })
  //         .AddComponent<InventoryComponent>(data => new() { ItemInHand = ItemFactory.Pistol })
  //         .AddComponent<HealthComponent>(data => new(Current: 5, Max: 5));
  // }

  // public static TacticsInitData CreateDefaultZombie(this TacticsInitData tacticsData, TiledObject tiledObject)
  // {
  //     return tacticsData
  //         .AddCharacter(tiledObject)
  //         .AddComponent<MindComponent>(data => new()
  //         {
  //             Name = "Zombie",
  //             TurnType = GetTurnType(tiledObject),
  //             TurnPoints = new(Current: 30, Max: 30),
  //             Brain = new Brain(Behaviour.All)
  //         })
  //         .AddComponent<SpriteComponent>(data => new()
  //         {
  //             Layer = SpriteComponent.CHARACTER_LAYER,
  //             SpriteName = TextureNames.ZombieTex,
  //             Visible = true
  //         })
  //         .AddComponent<InventoryComponent>(data => new() { ItemInHand = ItemFactory.Fist })
  //         .AddComponent<HealthComponent>(data => new(Current: 5, Max: 5));
  // }

  public static TacticsInitData AddEntity(this TacticsSceneData tacticsData)
    => new TacticsInitData(tacticsData, EntityID.New())
        .RegisterEntity();

  public static TacticsInitData AddMapEntity(this TacticsSceneData tacticsData, GridPos mapPos) => tacticsData
      .AddEntity()
      .AddComponent<MapPosComponent>(data => new() { MapPos = mapPos, IsBlockingPos = true })
      .RegisterMapComponent();

  public static TacticsInitData AddCharacter(this TacticsSceneData tacticsSceneData, GridPos mapPos) => tacticsSceneData
      .AddMapEntity(mapPos);

  // .AddComponent<FovComponent>(data => new())
  // .AddComponent<OffsetColorComponent>(data => new())
  // .AddComponent<MouseHighlightComponent>(data => new(DisplayInfo: true))
  // .AddComponent<OffsetPosComponent>(data => new() { Offsets = [] })
  // .AddComponent<AreaComponent>(data => new(Shape: GetDefaultShape(), CollisionMask: CollisionMask.Character))

  static TacticsInitData RegisterMapComponent(this TacticsInitData initData)
  {
    var mapPosComponent = initData.TacticsSceneData.EntityData.ComponentStorage.Get<MapPosComponent>(initData.Id);
    initData.Id.MoveTeleport(mapPosComponent.MapPos, initData.TacticsSceneData);
    return initData;
  }

  static TacticsInitData RegisterEntity(this TacticsInitData initData)
    => initData.TacticsSceneData.EntityData.Entities.Add(initData.Id).Get(initData);

  // public static TacticsInitData AddBullet(this TacticsInitData tacticsData, BulletInfo bulletInfo)
  //     => tacticsData.AddEntity()
  //       .AddComponent<OffsetPosComponent>(data => new() { Offsets = new() { [OffsetPos.World] = bulletInfo.Origin }, OriginCenter = true })
  //       .AddComponent<SpriteComponent>(data => new()
  //       {
  //           SpriteName = SpriteComponent.DEFAULT_BULLET_NAME,
  //           Layer = SpriteComponent.WALL_LAYER,
  //           Visible = true
  //       })
  //       .AddComponent<VelocityComponent>(data => new() { Dir = bulletInfo.Dir, Speed = bulletInfo.Speed })
  //       .AddComponent<RotationComponent>(data => new() { Dir = bulletInfo.Dir })
  //       .AddComponent<BulletComponent>(data => new()
  //       {
  //           Origin = bulletInfo.Origin,
  //           MaxRange = bulletInfo.MaxRange,
  //           Ignore = bulletInfo.Ignore
  //       })
  //       .AddComponent<AreaComponent>(data => new(Shape: new() { ShapeType = ShapeType.Circle, Size = 0.01f }, CollisionMask: CollisionMask.Bullet));

  // private static GridPos MapPosFromTiledObject(TiledObject obj)
  //     => TilemapManager.WorldToMap(new(obj.x, obj.y));

  // private static TurnType GetTurnType(TiledObject tiledObject) => tiledObject.name switch
  // {
  //     "player" => TurnType.PlayerTurn,
  //     "enemy" => TurnType.EnemyTurn,
  //     _ => "invalid turn type".LogError().Get(TurnType.PlayerTurn)
  // };
}
