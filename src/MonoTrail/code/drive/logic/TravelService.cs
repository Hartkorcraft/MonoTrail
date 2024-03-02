using TrailCore.data.story_data;
using TrailCore.drive.logic;

namespace MonoTrail.code.drive.logic;

public delegate void TraveledStep(TravelData travelData);

public static class TravelService
{
  public static event TraveledStep TraveledStepEvent;

  public static TravelData TravelStep(TravelData travelData)
  {
    var data = TravelManager.TravelStep(travelData);
    TraveledStepEvent.Invoke(data);
    return data;
  }
}
