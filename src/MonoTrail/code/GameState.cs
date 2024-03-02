using FUnK.OneOf;
using MonoTrail.code.drive.data;
using MonoTrail.code.story;
using MonoTrail.code.tactics.data;
using TrailCore.data.story_data;

namespace MonoTrail.code;

public record GameState
{
  public required StoryData StoryData { get; set; }
  public required OneOf<TacticsSceneData, DriveData> Scene { get; set; }
  public Option<IViewSystem> View { get; set; } = None;
}
