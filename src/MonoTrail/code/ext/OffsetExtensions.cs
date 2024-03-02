using System.Collections.Generic;
using System.Linq;
using MonoTrail.code.tactics.data.components;

namespace MonoTrail.code.ext;

public static class OffsetExtensions
{
  public static Vec2 AggregateOffsets(this Dictionary<OffsetPos, Vec2> offsets)
    => offsets.Count != 0
        ? offsets.Values.Aggregate((prev, next) => prev + next)
        : Vec2.Zero;
}
