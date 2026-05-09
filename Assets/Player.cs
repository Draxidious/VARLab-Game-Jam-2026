using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [Networked] public Vector3 HeadPosition { get; set; }
    [Networked] public Quaternion HeadRotation { get; set; }

    private Transform _headTransform; // your XR camera/head transform

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            // Assign your XR head (Camera.main or a direct reference)
            _headTransform = Camera.main.transform;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority)
        {
            HeadPosition = _headTransform.position;
            HeadRotation = _headTransform.rotation;
        }
    }

    public override void Render()
    {
        transform.position = HeadPosition;
        transform.rotation = HeadRotation;
    }
}