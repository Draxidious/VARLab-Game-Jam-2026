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

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_UpdateHeadTransform(Vector3 position, Quaternion rotation)
    {
        HeadPosition = position;
        HeadRotation = rotation;
    }


    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority)
        {
            if (_headTransform == null)
                _headTransform = Camera.main?.transform;

            if (_headTransform != null)
            {
                RPC_UpdateHeadTransform(_headTransform.position, _headTransform.rotation);
            }
        }
    }

    public override void Render()
    {
        transform.position = HeadPosition;
        transform.rotation = HeadRotation;
    }
}