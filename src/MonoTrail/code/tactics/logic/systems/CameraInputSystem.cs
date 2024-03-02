using Microsoft.Xna.Framework;
using MonoTrail.code.ext;
using MonoTrail.code.input;

namespace MonoTrail.code.tactics.logic.systems;

public class CameraInputSystem : ISystem
{
  public void Update(GameState gameState, GameTime gameTime)
  {
    var tactics = gameState.Scene.Left;
    var (posInfo, zoomInfo) = tactics.CameraData;
    var delta = gameTime.ToDelta();

    var zoomChangeAmmount =
        Inputs.CAMERA_ZOOM_IN.Pressing(zoomInfo.ChangeSpeed, out var plus) ? plus :
        Inputs.CAMERA_ZOOM_OUT.Pressing(-zoomInfo.ChangeSpeed, out var minus) ? minus :
        default;

    zoomInfo.Target = MathHelper.Clamp(
        zoomInfo.Target + (zoomChangeAmmount * delta),
        zoomInfo.Min,
        zoomInfo.Max);

    if (zoomChangeAmmount == default)
      zoomInfo.Target = zoomInfo.Target.Lerp(zoomInfo.Zoom, zoomInfo.StopSmoothing);

    var cameraChangeOffsetDir = Vector2.Zero;
    cameraChangeOffsetDir += Inputs.CAMERA_OFFSET_UP.Pressing(Vec2Extensions.Up, out var up) ? up : Vector2.Zero;
    cameraChangeOffsetDir += Inputs.CAMERA_OFFSET_DOWN.Pressing(Vec2Extensions.Down, out var down) ? down : Vector2.Zero;
    cameraChangeOffsetDir += Inputs.CAMERA_OFFSET_LEFT.Pressing(Vec2Extensions.Left, out var left) ? left : Vector2.Zero;
    cameraChangeOffsetDir += Inputs.CAMERA_OFFSET_RIGHT.Pressing(Vec2Extensions.Right, out var right) ? right : Vector2.Zero;

    var targetOffset = posInfo.Offset + (cameraChangeOffsetDir * posInfo.SpeedMove * delta);

    posInfo.Offset = posInfo.Offset
        .Lerp(targetOffset, posInfo.SpeedWeight);
  }
}
