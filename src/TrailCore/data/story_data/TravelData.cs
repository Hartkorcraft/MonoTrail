namespace TrailCore.data.story_data;

public record TravelData
{
  public required int VehicleSpeedKmPerH { get; set; }
  public required int NonDriveTime { get; set; }
  public required int DriveTime { get; set; }
  public required int TraveledDistanceInMeters { get; set; }
  public required List<DriveEventData> EventsOnRoad { get; set; }
}


