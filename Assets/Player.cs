using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private const float MoveSpeed = 5f;

    private NetworkCharacterController _cc;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        if (_cc == null)
        {
            // Debug.LogError("NetworkCharacterController not found on player!", gameObject);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            // Debug.LogWarning($"GetInput for player {Object.InputAuthority}", gameObject);
            data.Direction.Normalize();
            _cc.Move(MoveSpeed * data.Direction * Runner.DeltaTime);
        }
        else
        {
            // Debug.LogWarning($"GetInput failed for player {Object.InputAuthority}", gameObject);
        }
    }
}