
using TrailCore.data.story_data;

namespace TrailCore.drive.logic;

public static class TravelManager
{
  const int TIME_STEP = 5;

  public static TravelData TravelStep(TravelData travelData)
  {
    travelData.TraveledDistanceInMeters += CalculateDistanceInMeters(travelData.VehicleSpeedKmPerH);
    travelData.DriveTime += TIME_STEP;
    return travelData;
  }

  static int CalculateDistanceInMeters(int kmPerH)
    => kmPerH * 1000 / (60 / TIME_STEP);

}
