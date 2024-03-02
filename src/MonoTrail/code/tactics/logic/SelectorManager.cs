using FuNK;
using MonoTrail.code.tactics.data;

namespace MonoTrail.code.tactics.logic;

public static class SelectorService
{
  public static GridPos MoveSelectorBy(this SelectorData selector, GridPos moveBy, int tileSize)
    => selector.MoveSelectorStepWithOffset(moveBy, tileSize);

  public static GridPos MoveSelectorTo(this SelectorData selector, GridPos moveTo, int tileSize)
    => selector.MoveSelectorWithOffset(moveTo, tileSize);

  public static GridPos TeleportSelectorTo(this SelectorData selector, GridPos moveTo)
    => selector.MoveSelector(moveTo);

  static GridPos MoveSelectorWithOffset(this SelectorData selector, GridPos moveTo, int tileSize)
  {
    _ = selector.AdjustSelectorOffset(moveTo, tileSize);
    return MoveSelector(selector, moveTo);
  }

  static GridPos MoveSelectorStepWithOffset(this SelectorData selector, GridPos moveBy, int tileSize)
    => MoveSelectorWithOffset(selector, selector.GridPos.Add(moveBy), tileSize);

  static GridPos MoveSelector(this SelectorData selector, GridPos moveTo)
    => selector.GridPos = moveTo;

  static Vec2 AdjustSelectorOffset(this SelectorData selector, GridPos moveTo, int tileSize)
    => selector.Offset = selector.GridPos.Subtract(moveTo).ToVec2() * tileSize;
}

