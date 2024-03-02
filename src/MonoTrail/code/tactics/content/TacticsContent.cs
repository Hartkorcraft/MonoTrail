using System;
using FuNK.collections;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTrail.code.tactics.content;

public class TacticsContent(
    IServiceProvider serviceProvider,
    string rootDirectory) : ContentManager(serviceProvider, rootDirectory)
{
  public SpritesTextures Textures { get; } = [];
  // public Texture2D GroundTilemapTex => SpriteTextures[MapData.TileSetTexName];
  // public Texture2D TileSelectorTexture { get; private set; }
  // public Texture2D BulletDefaultTexture { get; private set; }
  // readonly Camera camera = new();

  public void Load()
  {
    // BulletDefaultTexture = Load<Texture2D>(Bullet.DEFAULT_BULLET_NAME);
    // TileSelectorTexture = Load<Texture2D>(TileSelectorData.TileSelectorTextureName);
    TextureNames.Names.Foreach(x => Textures[x] = Load<Texture2D>(x));
  }
}
