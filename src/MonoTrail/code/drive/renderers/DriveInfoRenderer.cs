using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTrail.code.drive.logic;
using MonoTrail.code.global;
using MonoTrail.code.rendering.camera;
using MonoTrail.code.rendering.data.camera;
using TrailCore.data.story_data;

namespace MonoTrail.code.drive.renderers;

public class DriveInfoRenderer : CameraRenderer, IDisposable
{
  TravelData travelData;

  public DriveInfoRenderer() : base(Globals.CreateNewSpriteBatch(), SpriteBatchEffect: null)
  {
    TravelService.TraveledStepEvent += UpdateTravelData;
  }

  public void Dispose()
  {
    TravelService.TraveledStepEvent -= UpdateTravelData;
    GC.SuppressFinalize(this);
  }

  protected override void Draw(RenderParams renderParams)
  {
    var distanceTraveledText = "Distance traveled " + (travelData.TraveledDistanceInMeters / 1000) + " km";
    var timeTraveledText = "Time traveled " + travelData.DriveTime + " m";
    var timeNonDrivenText = "Time not traveled " + travelData.NonDriveTime + " m";
    var toDraw = string.Join(Environment.NewLine, distanceTraveledText, timeTraveledText, timeNonDrivenText);

    var pos = new Vec2(
        -GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 10.1f,
        -GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 10.5f);

    spriteBatch.DrawString(
        spriteFont: Globals.GlobalContent.PixelFont,
        text: toDraw,
        position: pos,
        color: Color.White);
  }

  void UpdateTravelData(TravelData travelData) => this.travelData = travelData;
}
