using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct InputData : IComponentData
{
    public KeyCode upKey, downKey, rightKey, leftKey, eKey, dKey, lKey, kKey, qKey;
}
