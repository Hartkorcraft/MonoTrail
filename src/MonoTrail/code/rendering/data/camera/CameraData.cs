using System.Reflection;
using FUnK.OneOf;
using TrailCore.tactics.data;

namespace MonoTrail.code.rendering.data.camera;

public record CameraData(
    CameraPosInfo CameraPosInfo,
    CameraZoomInfo CameraZoomInfo);

public record CameraPosInfo(
    Vec2 Pos,
    OneOf<EntityID, Vec2> Target,
    Vec2 Offset,
    float SpeedMax = 5,
    float SpeedMove = 2500,
    float SpeedWeight = 0.08f,
    float SpeedStopSmoothing = 0.5f)
{
  public Vec2 Pos { get; set; } = Pos;
  public OneOf<EntityID, Vec2> Target { get; set; } = Target;
  public Vec2 Offset { get; set; } = Offset;
}

public record CameraZoomInfo(
    float Zoom = 1,
    float Min = 1.5f,
    float Max = 8f,
    float Target = 4f,
    float ZoomWeight = 0.05f,
    float StopSmoothing = 0.1f,
    float ChangeSpeed = 10f)
{
  public float Zoom { get; set; } = Zoom;
  public float Target { get; set; } = Target;
}
