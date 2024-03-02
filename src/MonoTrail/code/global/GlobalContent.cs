using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoTrail.code.tactics.content;

namespace MonoTrail.code.global;

public class GlobalContent(
    IServiceProvider serviceProvider,
    string rootDirectory) : ContentManager(serviceProvider, rootDirectory)
{
  const string PixelFontPath = "fonts/pixelfont";

  public SpriteFont PixelFont { get; private set; }
  public Effect TestEffect { get; }
  public SpritesTextures Textures { get; private set; } = [];
  public Texture2D PixelRectangle { get; private set; }

  public void Load(GraphicsDevice graphicsDevice)
  {
    Textures[TextureNames.DefaultMouseCursor] = Load<Texture2D>(TextureNames.DefaultMouseCursor);
    // TestEffect = Load<Effect>(@"effects\testSpriteEffect");
    PixelFont = Load<SpriteFont>(PixelFontPath);

    PixelRectangle = new Texture2D(graphicsDevice, 1, 1);
    PixelRectangle.SetData(new[] { Color.White });
  }
}
