using MonoTrail.code.rendering.data.camera;

namespace MonoTrail.code.drive.data;

public record DriveData(
    CameraData CameraData
    ) : TrailCore.drive.data.DriveData;
