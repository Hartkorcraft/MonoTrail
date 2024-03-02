using Microsoft.Xna.Framework;
using MonoTrail.code.input;
using MonoTrail.code.rendering.camera;

namespace MonoTrail.code.logic
{
  public static class CameraManager
  {
    public static Vec2 CameraMousePos(this Camera camera)
      => camera.ScreenToWorld(MouseManager.MousePos.ToVector2());

    public static bool ContainsMouse(this Rectangle rect, Camera camera)
      => rect.Contains(camera.CameraMousePos().ToPoint());
  }
}
