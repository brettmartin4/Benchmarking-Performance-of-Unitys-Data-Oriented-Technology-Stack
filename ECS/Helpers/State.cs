using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct State : IComponentData
{

    public float time;          // time
    public float q0, q1, q2, q3, q4, q5;    // ODE results

    public float bank;          // roll angle
    public float alpha;         // pitch angle
    public float throttle;      // throttle percentage
    public float flap;          // flap deflection
    
}
