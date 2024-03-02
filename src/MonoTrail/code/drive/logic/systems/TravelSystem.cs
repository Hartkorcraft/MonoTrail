using Microsoft.Xna.Framework;

namespace MonoTrail.code.drive.logic.systems;

public class TravelSystem : ISystem
{
  const double cooldown = 1;
  double endOfCooldown;

  public void Update(GameState gameState, GameTime gameTime)
  {
    var curTime = gameTime.TotalGameTime.TotalSeconds;

    if (endOfCooldown >= curTime)
      return;

    endOfCooldown += cooldown;

    var newTravelData = TravelService.TravelStep(gameState.StoryData.TravelData);
  }
}
