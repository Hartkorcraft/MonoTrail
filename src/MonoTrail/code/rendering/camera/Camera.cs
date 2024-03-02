using Microsoft.Xna.Framework;
using MonoTrail.code.global;
using MonoTrail.code.rendering.data.camera;

namespace MonoTrail.code.rendering.camera;

public class Camera
{
  public Matrix Transform { get; private set; }

  public void CalculateTranslation(CameraData cameraData)
  {
    var (posInfo, zoomInfo) = cameraData;
    var dx = (Globals.WindowSize.X / (2 * zoomInfo.Zoom)) - posInfo.Pos.X;
    var dy = (Globals.WindowSize.Y / (2 * zoomInfo.Zoom)) - posInfo.Pos.Y;
    Transform = Matrix.CreateTranslation(dx, dy, 0) * Matrix.CreateScale(zoomInfo.Zoom);
  }

  public Vector2 ScreenToWorld(Vector2 screen)
  {
    Matrix invertedMatrix = Matrix.Invert(Transform);
    return Vector2.Transform(screen, invertedMatrix);
  }
}
