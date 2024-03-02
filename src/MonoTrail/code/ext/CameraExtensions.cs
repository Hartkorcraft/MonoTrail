using MonoTrail.code.input;
using MonoTrail.code.tactics.data;

namespace MonoTrail.code.ext;

public static class CameraExtensions
{
  public static bool IsMovingCamera()
    => Inputs.CAMERA_OFFSET_UP.IsPressed() ||
       Inputs.CAMERA_OFFSET_DOWN.IsPressed() ||
       Inputs.CAMERA_OFFSET_LEFT.IsPressed() ||
       Inputs.CAMERA_OFFSET_RIGHT.IsPressed();

  public static void ResetCameraOffset(this TacticsSceneData tactics)
    => tactics.CameraData.CameraPosInfo.Offset = Vec2.Zero;
}
