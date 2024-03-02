using Microsoft.Xna.Framework;
using MonoTrail.code.rendering.camera;

namespace MonoTrail.code.data;

public record struct UpdateParams(
GameState GameState,
GameTime GameTime,
Camera Camera);
