using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoTrail.code.tactics.content;

namespace MonoTrail.code.drive.content;

public class DriveContent(
    IServiceProvider serviceProvider,
    string rootDirectory) : ContentManager(serviceProvider, rootDirectory)
{
  public SpritesTextures Textures { get; } = [];

  public void Load()
  {
    TextureNames.Names.Foreach(x => Textures[x] = Load<Texture2D>(x));
  }
}
