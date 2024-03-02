// using System.Collections.Generic;
// using Microsoft.Xna.Framework;
// using MonoXTrail.src.extensions.collections;

// namespace MonoXTrail.src.main.scenes.tactics.data.components;

// public enum OffsetPos
// {
//     Bump, Move, World, Hit, Recoil
// }

// public record class OffsetPosComponent : IComponent
// {
//     public const float DEFAULT_MOVE_LERP_AMOUNT = 30f;
//     public const float NPC_MOVE_LERP_AMOUNT = 25f;

//     public const float BUMP_LERP_AMOUNT = 10f;
//     public const float HIT_LERP_AMOUNT = 10f;
//     public const float RECOIL_LERP_AMOUNT = 10f;
//     public const float BUMP_AMMOUNT = 2;
//     public const float HIT_AMMOUNT = 1.5f;

//     public ValueDictionary<OffsetPos, Vector2> Offsets { get; set; } = [];
//     public bool OriginCenter { get; set; } = false;
//     public bool IgnoreFov { get; init; } = false;
// }
