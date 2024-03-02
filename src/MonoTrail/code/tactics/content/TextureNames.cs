namespace MonoTrail.code.tactics.content;

public static class TextureNames
{
  public const string PistolTex = "pistol";
  public const string FistTex = @"sprites\weapons\fist";
  public const string DziadersTex = @"characters\dziaders";
  public const string ZombieTex = @"characters\zombie";
  public const string TileSelectorTex = "TileSelector";
  public const string TileSetTexName = "ground";

  public const string PlayerTex = "player";
  public const string WallTex = "wall";
  public const string DEFAULT_BULLET_NAME = "bulletDefault";
  public const string DefaultMouseCursor = "mouse";

  public static readonly string[] Names = [
    PlayerTex,
    WallTex,
    TileSetTexName,
    TileSelectorTex,
        //SpriteComponent.DEFAULT_BULLET_NAME,
    DefaultMouseCursor,
    PistolTex,
    DziadersTex,
    ZombieTex,
    FistTex
  ];
}
