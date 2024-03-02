using Microsoft.Xna.Framework;
using MonoTrail.code.data;

namespace MonoTrail.code;

public delegate void RequestedSceneChange(IScene scene);

public interface IScene
{
  event RequestedSceneChange RequestedSceneChangeEvent;
  void Init(GameServiceContainer gameServiceContainer);
  void Update(GameState gameState, GameTime gameTime);
  void Draw(GameState gameState, GameTime gameTime);
}
