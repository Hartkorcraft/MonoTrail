using FuNK;
using Microsoft.Xna.Framework;
using MonoTrail.code.tactics.data.components;
using MonoTrail.code.tactics.logic.systems;
using TrailCore.extensions;
using TrailCore.tactics.data;
using TrailCore.tactics.logic;

namespace MonoTrail.code.tactics.logic.turn.moves;

public record MoveStep(EntityID Entity, GridPos MoveBy) : MoveCommand
{
  public override bool Execute(GameState gameState, GameTime gameTime)
  {
    var tactics = gameState.Scene.Left;
    var moved = Entity.MoveStep(MoveBy, gameState.Scene.Left);

    if (moved is false)
    {
      Bump(Entity.GetRelativeGridPos((0, 0), tactics), MoveBy, AdjustOffsetsSystem.BUMP_AMMOUNT, tactics);
      Bump(Entity.GetRelativeGridPos(MoveBy, tactics), MoveBy, AdjustOffsetsSystem.BUMP_AMMOUNT / 1.2f, tactics);
      Bump(Entity.GetRelativeGridPos(MoveBy.Multiply(2), tactics), MoveBy, AdjustOffsetsSystem.BUMP_AMMOUNT / 4, tactics);
      return false;
    }

    tactics.EntityData.ComponentStorage
      .GetMaybe<OffsetComponent>(Entity)
      .MatchEffect(c => c.Offsets[OffsetPos.Move] = -MoveBy.ToVec2() * tactics.TileSetData.TileSize);

    Entity.ResetEntity(tactics);
    return moved;
  }

  static void Bump(GridPos atPos, GridPos dir, float ammount, TacticsSceneData tactics)
  {
    if (atPos.IsPosOnMapOccupied(tactics.MapData) is false)
      return;

    tactics
      .MapData
      .Tiles[atPos.PosToTileIndex(tactics.MapData)]
      .Occupying
      .Bind(o => o.GetComponent<OffsetComponent>(tactics))
      .MatchEffect(c => c.Offsets[OffsetPos.Bump] = dir.ToVec2() * ammount);
  }
}
