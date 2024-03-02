using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoTrail.code.global;

namespace MonoTrail.code;

public class MonoTrailGame : Game
{
  Main main;
  Color backgroundColor = new(16, 24, 32);
  readonly GraphicsDeviceManager graphicsDeviceManager;

  public MonoTrailGame()
  {
    graphicsDeviceManager = new GraphicsDeviceManager(this);
    Content.RootDirectory = "Content";
    IsMouseVisible = true;
    Window.AllowUserResizing = true;
  }

  protected override void Initialize()
  {
    var globalContent = new GlobalContent(Services, Content.RootDirectory);
    globalContent.Load(graphicsDeviceManager.GraphicsDevice);

    _ = new Globals(this, globalContent);
    IsMouseVisible = false;
    main = new(Services);
    base.Initialize();
  }

  protected override void LoadContent()
  {

    base.LoadContent();
  }

  protected override void Update(GameTime gameTime)
  {
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
      Exit();

    main.Update(gameTime);

    base.Update(gameTime);
  }

  protected override void Draw(GameTime gameTime)
  {
    GraphicsDevice.Clear(backgroundColor);

    main.Draw(gameTime);

    base.Draw(gameTime);
  }
}
