using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

//[DisableAutoCreation]
public partial class InputSystem : SystemBase
{

    public void reset_scene()
    {
        Entities
            .WithAll<PlayerTag>()// Instead of using reference in "for each" lambda arguments
            .ForEach((ref Translation translation, ref Rotation rotation, ref State state) => {
                translation.Value = new float3(0.0f, 0.0f, 0.0f);
                state.time = 0.0f;
                state.q0 = 0.0f;
                state.q1 = 0.0f;
                state.q2 = 0.0f;
                state.q3 = 0.0f;
                state.q4 = 0.0f;
                state.q5 = 0.0f;
                state.bank = 0.0f;
                state.alpha = 4.0f;
                state.throttle = 0.0f;
                state.flap = 0.0f;
            }).WithoutBurst().Run();
    }


    protected override void OnUpdate()
    {

        Entities.ForEach((ref State state, in InputData inputData) =>
        {
            bool isRightPressed = Input.GetKeyDown(inputData.rightKey); // Right : Bank right
            bool isLeftPressed = Input.GetKeyDown(inputData.leftKey);   // Left  : Bank left
            bool isUpPressed = Input.GetKeyDown(inputData.upKey);       // Up    : Alpha up
            bool isDownPressed = Input.GetKeyDown(inputData.downKey);   // Down  : Alpha down
            bool isEPressed = Input.GetKeyDown(inputData.eKey);         // E     : Throttle up
            bool isDPressed = Input.GetKeyDown(inputData.dKey);         // D     : Throttle down
            bool isLPressed = Input.GetKeyDown(inputData.lKey);         // L     : Flap down
            bool isKPressed = Input.GetKeyDown(inputData.kKey);         // K     : Flap up
            bool isQPressed = Input.GetKeyDown(inputData.qKey);         // Q     : Quit

            state.bank = (isRightPressed) ? Mathf.Clamp(state.bank - 1.0f, -20.0f, 20.0f) : state.bank;
            state.bank = (isLeftPressed) ? Mathf.Clamp(state.bank + 1.0f, -20.0f, 20.0f) : state.bank;
            state.alpha = (isUpPressed) ? Mathf.Clamp(state.alpha + 1.0f, 0.0f, 20.0f) : state.alpha;
            state.alpha = (isDownPressed) ? Mathf.Clamp(state.alpha - 1.0f, 0.0f, 20.0f) : state.alpha;
            state.throttle = (isEPressed) ? Mathf.Clamp(state.throttle + 0.1f, 0.0f, 1.0f) : state.throttle;
            state.throttle = (isDPressed) ? Mathf.Clamp(state.throttle - 0.1f, 0.0f, 1.0f) : state.throttle;
            state.flap = (isLPressed) ? Mathf.Clamp(state.flap - 1.0f, 0.0f, 40.0f) : state.flap;
            state.flap = (isKPressed) ? Mathf.Clamp(state.flap + 1.0f, 0.0f, 40.0f) : state.flap;

            if (isQPressed || state.q5 < -1.0f) { reset_scene(); }
        }).WithoutBurst().Run();   // Input needs to occur on main thread (not worker threads), so needs to call RUN, not SCHEDULE
    }

}
