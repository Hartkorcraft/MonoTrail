using FUnK.OneOf;
using Microsoft.Xna.Framework;
using TrailCore.tactics.data;

namespace MonoTrail.code.tactics.logic.systems;

public class CameraTargetSelectionSystem : ISystem
{
    public void Update(GameState gameState, GameTime gameTime)
    {
        var tactics = gameState.Scene.Left;
        var componentData = tactics.EntityData.ComponentStorage;
        var (posInfo, zoomInfo) = tactics.CameraData;

        posInfo.Target = tactics
          .TurnData
          .CurrentSelection
          .Match(
            () => posInfo.Target,
            id => BindEntityVisibleByPlayer(id, tactics));
    }

    static OneOf<EntityID, Vector2> BindEntityVisibleByPlayer(EntityID id, data.TacticsSceneData tactics) //, FovData fovData)
    {
        return id;
        // var mapPos = id.GetComponent<MapPosComponent>(tactics).MapPos;
        // return tactics.CameraData.CameraPosInfo.Pos;
        // return fovData.PlayerView.Contains(mapPos) ? id : cameraData.CameraPosInfo.Pos;
    }
}
