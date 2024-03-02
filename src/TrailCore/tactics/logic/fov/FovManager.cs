using TrailCore.tactics.data;
using TrailCore.tactics.data.components;
using TrailCore.tactics.data.fov;

namespace TrailCore.tactics.logic.fov;

public static class FovManager
{
  public static void ResetFov(TacticsSceneData tactics)
  {
    var fovData = tactics.EntityData.FovData;
    fovData.PlayerView.Clear();
    fovData.EnemyView.Clear();

    var componentStorage = tactics.EntityData.ComponentStorage;

    foreach (var (id, fov) in componentStorage.GetComponents<FovComponent>())
    {
      if (componentStorage.TryGet<FovComponent>(id, out var fovComponent) is false) return;

      if (fovComponent.IsSeeing is false) return;

      var mapPosComponent = componentStorage.Get<MapPosComponent>(id);
      var fovParams = new FovParams(mapPosComponent.MapPos, pos => IsBlockingViewDefault(tactics, pos), 50, 0);
      fovComponent.Saw.UnionWith(fovComponent.Seeing);
      fovComponent.Seeing = fovComponent.LookingDirection.Match(
          () => SymetricShadowCastingFov.ComputeAllDirections(fovParams),
          dir => SymetricShadowCastingFov.ComputeForDirection(fovParams, dir));

      var (playerView, enemyView, seenByPlayer) = fovData;
      var (view, seen) = componentStorage.Get<MindComponent>(id).FactionAligment switch
      {
        FactionAligment.Player => (playerView, fovComponent.Saw),
        FactionAligment.Monsters => (enemyView, []),
        // TurnType.PlayerTurn => (playerView, fovComponent.Saw),
        // TurnType.EnemyTurn => (enemyView, []),
        _ => ([], [])
      };

      seenByPlayer.UnionWith(seen);
      view.UnionWith(fovComponent.Seeing);
    }
  }

  public static bool IsSeenByPlayer(EntityID id, TacticsSceneData tactics)
      => !tactics.EntityData.ComponentStorage.TryGet<MapPosComponent>(id, out var mapPosComponent) ||
          tactics.EntityData.FovData.PlayerView.Contains(mapPosComponent.MapPos);

  private static bool IsBlockingViewDefault(TacticsSceneData tacticsData, GridPos gridPos)
      => gridPos.IsOutsideMap(tacticsData.MapData) || tacticsData.MapData.TryGetBlocked(gridPos, out var id);
  // && tacticsData.EntityData.ComponentStorage.Has<NotBlockingViewComponent>(id) is false);
}
