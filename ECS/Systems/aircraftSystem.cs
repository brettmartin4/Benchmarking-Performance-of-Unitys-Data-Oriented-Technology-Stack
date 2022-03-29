using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

//[DisableAutoCreation]
public partial class aircraftSystem : SystemBase
{

    protected override void OnUpdate()
    {
        // Main loop
        float deltaTime = Time.DeltaTime;

        Entities
            .WithAll<PlayerTag>()
            .ForEach((ref Translation translation, ref Rotation rotation, ref State state) => {
                EOM.eom(GameManager.main.prop, ref state, deltaTime);

                // velocity information
                float vx = state.q0;
                float vy = state.q4;
                float vz = state.q2;
                // position information
                float x = state.q1;
                float y = state.q5;
                float z = state.q3;

                float3 position = new float3(x, y, z);
                float bank = -90.0f + state.bank;
                float alpha = state.alpha;
                float heading = Mathf.Atan2(vz, vx);

                // calculate Quaternion value for new rotation
                float roll = bank * Mathf.Deg2Rad;
                float pitch = heading;
                float yaw = alpha * Mathf.Deg2Rad;

                float croll = Mathf.Cos(roll * 0.5f);
                float cpitch = Mathf.Cos(pitch * 0.5f);
                float cyaw = Mathf.Cos(yaw * 0.5f);

                float sroll = Mathf.Sin(roll * 0.5f);
                float spitch = Mathf.Sin(pitch * 0.5f);
                float syaw = Mathf.Sin(yaw * 0.5f);

                float cyawcpitch = cyaw * cpitch;
                float syawspitch = syaw * spitch;
                float cyawspitch = cyaw * spitch;
                float syawcpitch = syaw * cpitch;

                Quaternion newRot = new Quaternion((cyawcpitch * sroll - syawspitch * croll),
                    (cyawspitch * croll + syawcpitch * sroll),
                    (syawcpitch * croll - cyawspitch * sroll),
                    (cyawcpitch * croll + syawspitch * sroll));

                translation.Value = position;
                rotation.Value = newRot;

                GameManager.main.cameraFollowObj.transform.position = position;
        }).WithoutBurst().Run();

    }

}
