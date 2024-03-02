using Microsoft.Xna.Framework;
using MonoTrail.code.ext;
using MonoTrail.code.tactics.data;
using MonoTrail.code.tactics.data.components;

namespace MonoTrail.code.tactics.logic.systems;

public class AdjustOffsetsSystem : ISystem
{
  const float TOLERANCE = 0.01f;
  public const float DEFAULT_MOVE_LERP_AMOUNT = 30f;
  public const float NPC_MOVE_LERP_AMOUNT = 25f;
  public const float BUMP_LERP_AMOUNT = 10f;
  public const float HIT_LERP_AMOUNT = 10f;
  public const float RECOIL_LERP_AMOUNT = 10f;
  public const float BUMP_AMMOUNT = 2;
  public const float HIT_AMMOUNT = 1.5f;
  public const float SELECTOR_LERP_AMOUNT = 30;

  public void Update(GameState gameState, GameTime gameTime)
  {
    var tactics = gameState.Scene.Left;
    var components = tactics.EntityData.ComponentStorage;
    var delta = gameTime.ToDelta();

    var moveLerpAmmount = tactics.TurnData.CurrentTurnType == TurnType.Player
        ? DEFAULT_MOVE_LERP_AMOUNT
        : NPC_MOVE_LERP_AMOUNT;

    AdjustSelectorOffset(tactics.SelectorData, delta);

    foreach (var (_, offsetComponent) in components.GetComponents<OffsetComponent>())
    {
      AdjustPosOffset(OffsetPos.Move, moveLerpAmmount, offsetComponent, delta);
      AdjustPosOffset(OffsetPos.Bump, BUMP_LERP_AMOUNT, offsetComponent, delta);
      AdjustPosOffset(OffsetPos.Hit, HIT_LERP_AMOUNT, offsetComponent, delta);
      AdjustPosOffset(OffsetPos.Recoil, RECOIL_LERP_AMOUNT, offsetComponent, delta);
    }
  }

  static void AdjustPosOffset(OffsetPos offsetType, float offsetLerpAmmount, OffsetComponent offsetComponent, float delta)
  {
    if (offsetComponent.Offsets.TryGetValue(offsetType, out Vec2 offset) is false)
      return;

    offsetComponent.Offsets[offsetType] = Vec2
        .Lerp(offset, Vec2.Zero, offsetLerpAmmount * delta)
        .Snap(Vec2.Zero, TOLERANCE);

    if (offsetComponent.Offsets[offsetType] == Vec2.Zero)
      _ = offsetComponent.Offsets.Remove(offsetType);
  }

  static void AdjustSelectorOffset(SelectorData selector, float delta)
  {
    selector.Offset = Vec2
       .Lerp(selector.Offset, Vec2.Zero, SELECTOR_LERP_AMOUNT * delta)
       .Snap(Vec2.Zero, TOLERANCE);
  }
}
