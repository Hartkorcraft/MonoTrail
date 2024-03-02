using Microsoft.Xna.Framework;
using MonoTrail.code.rendering.camera;

namespace MonoTrail.code.rendering.data.camera;

public record RenderParams(
    // GameState GameState,
    GameTime GameTime,
    Camera RenderingCamera,
    SpritesTextures Textures);
