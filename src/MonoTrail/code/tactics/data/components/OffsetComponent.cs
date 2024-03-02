using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TrailCore.data.tactics;

namespace MonoTrail.code.tactics.data.components;

public record OffsetComponent(
  Dictionary<OffsetPos, Vector2> Offsets) : IComponent;

public enum OffsetPos
{
    Bump, Move, World, Hit, Recoil
}
