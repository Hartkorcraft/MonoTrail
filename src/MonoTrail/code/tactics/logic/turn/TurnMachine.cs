using System;
using Microsoft.Xna.Framework;
using MonoTrail.code.input;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.rendering.data.camera;
using MonoTrail.code.tactics.content;
using MonoTrail.code.tactics.logic.turn.moves;

namespace MonoTrail.code.tactics.logic.turn;

public class TurnMachine(Camera camera, TacticsContent tacticsContent, int tileSize)
{
  readonly CameraRenderer[] sceneRenderers = [];

  readonly Turn PlayerTurn = new(
      WanderCommands: [
        new(Inputs.NEXT_CHARACTER,(args)=> new NextCharacter()),
        new(Inputs.MOVE_UP,(args) => new MoveStep(args.EntityID,(0,-1))),
        new(Inputs.MOVE_DOWN,(args) => new MoveStep(args.EntityID,(0,1))),
        new(Inputs.MOVE_LEFT,(args) => new MoveStep(args.EntityID,(-1,0))),
        new(Inputs.MOVE_RIGHT,(args) => new MoveStep(args.EntityID,(1,0))),
        new(Inputs.AIM,(args)=> new ChangeChooseState(data.turn.ChooseState.Aim))],
      AimCommands: [
        new(Inputs.AIM,(args)=> new ChangeChooseState(data.turn.ChooseState.Wander)),
        new(Inputs.MOVE_UP, (args) => new MoveSelector((0, -1),tileSize)),
        new(Inputs.MOVE_DOWN, (args) => new MoveSelector((0, 1),tileSize)),
        new(Inputs.MOVE_LEFT, (args) => new MoveSelector((-1, 0),tileSize)),
        new(Inputs.MOVE_RIGHT, (args) => new MoveSelector((1, 0),tileSize))]);

  readonly Turn NpcTurn = new(
      WanderCommands: [],
      AimCommands: []);

  public void Init(GameState gameState) { }

  public void Update(GameState gameState, GameTime gameTime)
  {
    _ = gameState.Scene.Left.TurnData.CurrentTurnType switch
    {
      data.TurnType.Player => PlayerTurn.Execute(gameState, gameTime),
      data.TurnType.Enemy => NpcTurn.Execute(gameState, gameTime),
      _ => throw new NotImplementedException()
    };
  }

  public void Draw(GameState gameState, GameTime gameTime)
  {
    var renderParams = new RenderParams(
        gameTime,
        camera,
        tacticsContent.Textures);

    for (int i = 0; i < sceneRenderers.Length; i++)
      sceneRenderers[i].Render(renderParams);
  }
}
