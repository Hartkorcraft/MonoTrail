using Microsoft.Xna.Framework;
using MonoTrail.code.data;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.story;

namespace MonoTrail.code.drive.logic.systems;

public class VisitLocationSystem(Camera camera) : ISystem
{
  public void Update(GameState gameState, GameTime gameTime)
  {
    var distance = gameState.StoryData.TravelData.TraveledDistanceInMeters;

    var events = gameState.StoryData.TravelData.EventsOnRoad;

    if (events.FirstOrNone(x => x.Distance < distance).TryMatch(out var e) && events.Remove(e))
    {
      gameState.View = new StoryBoxView([new(
        StoryEventManager.StoryEvent.CLOSE_VIEW,
        "OK",
        new Rectangle(0, 0, 40, 20))],
        camera);
    }
  }
}
