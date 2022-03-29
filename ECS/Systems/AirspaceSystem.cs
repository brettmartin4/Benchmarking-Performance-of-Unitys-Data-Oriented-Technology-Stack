using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

//[DisableAutoCreation]
public partial class AirspaceSystem : SystemBase
{

    protected override void OnUpdate()
    {
        // Delta and elapsed times
        float deltaTime = Time.DeltaTime;
        float elapsedTime = (float)Time.ElapsedTime;

        // Update all entities
        Entities
            .WithAny<NPCTag>()
            .ForEach((ref Translation translation, ref Rotation rotation, ref AircraftData aircraftData) => {
                aircraftData.radian = aircraftData.speed * elapsedTime;
                translation.Value.x = aircraftData.centerX + aircraftData.radius * Mathf.Cos(aircraftData.radian);  // TODO: Create Float3 and assign translation value once
                translation.Value.z = aircraftData.centerZ + aircraftData.radius * Mathf.Sin(aircraftData.radian);
                translation.Value.y = aircraftData.height;
                rotation.Value = math.mul(rotation.Value, quaternion.RotateZ(-aircraftData.speed * deltaTime));
            }).ScheduleParallel();

    }

}
