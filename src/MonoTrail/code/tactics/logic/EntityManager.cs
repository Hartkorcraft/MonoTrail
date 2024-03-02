
using MonoTrail.code.ext;
using TrailCore.tactics.data;
using TrailCore.tactics.logic.fov;

namespace MonoTrail.code.tactics.logic;

public static class EntityManager
{
  public static void ResetEntity(this EntityID e, data.TacticsSceneData tactics)
  {
    tactics.ResetScene();
  }

  public static void ResetScene(this data.TacticsSceneData tactics)
  {
    // TODO foreach id reset
    if (CameraExtensions.IsMovingCamera() is false)
      tactics.CameraData.CameraPosInfo.Offset = Vec2.Zero;
    FovManager.ResetFov(tactics);
  }
}
