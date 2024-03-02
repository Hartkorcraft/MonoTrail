using ChosenScene = FUnK.OneOf.OneOf<TrailCore.tactics.data.TacticsSceneData, TrailCore.drive.data.DriveData>;
namespace TrailCore.logic;

public record GameState()
{
  public required ChosenScene Scene { get; set; }
}
