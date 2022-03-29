using Unity.Entities;

[GenerateAuthoringComponent]
public struct AircraftData : IComponentData
{
    public float speed, radian, radius, height, centerX, centerZ;
}
