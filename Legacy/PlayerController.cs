using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Properties prop = new Properties(
            16.2f,          // wing area
            10.9f,          // wing span
            2.0f,           // tail area
            0.0889f,        // slope of Cl-alpha curve
            0.178f,         // intercept of Cl-alpha curve
            -0.1f,          // post-stall slope of Cl-alpha curve
            3.2f,           // post-stall intercept of Cl-alpha curve
            16.0f,          // alpha when Cl=Clmax
            0.034f,         // parasite drag coefficient
            0.77f,          // induced drag efficiency coefficient
            1114.0f,        // mass
            119310.0f,      // engine power
            40.0f,          // revolutions per second
            1.905f,         // propeller diameter
            1.83f,          // propeller efficiency coefficient
            -1.32f          // propeller efficiency coefficient
            );

    public PlayerState playerState = new PlayerState(
            0f,             // time
            0f,             // EOM values
            0f,
            0f,
            0f,
            0f,
            0f,
            0f,             // bank
            4.0f,           // alpha
            0f,             // throttle
            0f              // flap
            );


    private void CheckKeys()
    {
        bool isRightPressed = Input.GetKeyDown(KeyCode.RightArrow); // Right : Bank right
        bool isLeftPressed = Input.GetKeyDown(KeyCode.LeftArrow);   // Left  : Bank left
        bool isUpPressed = Input.GetKeyDown(KeyCode.UpArrow);       // Up    : Alpha up
        bool isDownPressed = Input.GetKeyDown(KeyCode.DownArrow);   // Down  : Alpha down
        bool isEPressed = Input.GetKeyDown(KeyCode.E);         // E     : Throttle up
        bool isDPressed = Input.GetKeyDown(KeyCode.D);         // D     : Throttle down
        bool isLPressed = Input.GetKeyDown(KeyCode.L);         // L     : Flap down
        bool isKPressed = Input.GetKeyDown(KeyCode.K);         // K     : Flap up

        playerState.bank = (isRightPressed) ? Mathf.Clamp(playerState.bank - 1.0f, -20.0f, 20.0f) : playerState.bank;
        playerState.bank = (isLeftPressed) ? Mathf.Clamp(playerState.bank + 1.0f, -20.0f, 20.0f) : playerState.bank;
        playerState.alpha = (isUpPressed) ? Mathf.Clamp(playerState.alpha + 1.0f, 0.0f, 20.0f) : playerState.alpha;
        playerState.alpha = (isDownPressed) ? Mathf.Clamp(playerState.alpha - 1.0f, 0.0f, 20.0f) : playerState.alpha;
        playerState.throttle = (isEPressed) ? Mathf.Clamp(playerState.throttle + 0.1f, 0.0f, 1.0f) : playerState.throttle;
        playerState.throttle = (isDPressed) ? Mathf.Clamp(playerState.throttle - 0.1f, 0.0f, 1.0f) : playerState.throttle;
        playerState.flap = (isLPressed) ? Mathf.Clamp(playerState.flap - 1.0f, 0.0f, 40.0f) : playerState.flap;
        playerState.flap = (isKPressed) ? Mathf.Clamp(playerState.flap + 1.0f, 0.0f, 40.0f) : playerState.flap;
    }


    private void MovePlayer()
    {
        Vector3 oldPosition = new Vector3(playerState.q1, playerState.q5, playerState.q3);
        OldEOM.eom(prop, ref playerState, Time.deltaTime);
        Vector3 newPosition = new Vector3(playerState.q1, playerState.q5, playerState.q3);

        // velocity information
        float vx = playerState.q0;
        float vy = playerState.q4;
        float vz = playerState.q2;
        // position information
        float x = playerState.q1;
        float y = playerState.q5;
        float z = playerState.q3;

        float bank = -90.0f + playerState.bank;
        float alpha = playerState.alpha;
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

        //transform.LookAt(oldPosition + newPosition);
        transform.rotation = newRot;
        transform.position = newPosition;
    }


    private void CheckBounds()
    {
        //if()
    }


    // Start is called before the first frame update
    void Start()
    {
        // Nothing yet
    }


    // Update is called once per frame
    void Update()
    {
        CheckKeys();
        MovePlayer();
        CheckBounds();
    }
}
