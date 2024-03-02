using Microsoft.Xna.Framework.Graphics;

namespace MonoTrail.code.global;

public class Globals
{
  public static GlobalContent GlobalContent { get; private set; }

  public static Vec2 WindowSize => new(
      Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
      Game.GraphicsDevice.PresentationParameters.BackBufferHeight);

  public static MonoTrailGame Game { get; private set; }

  public Globals(MonoTrailGame game, GlobalContent globalContent)
  {
    Game = game;
    GlobalContent = globalContent;
  }

  public static SpriteBatch CreateNewSpriteBatch() => new(Game.GraphicsDevice);
}
