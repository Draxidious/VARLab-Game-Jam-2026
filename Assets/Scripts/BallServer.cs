using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;
using System.Collections.Generic;

public class BallServer : NetworkBehaviour
{
    // ======================================================
    #region Initialization

    public static BallServer Instance;

    // TODO Convert to arrays later
    public Dictionary<ulong, Vector3> m_clientWorldSpacePositionTable = new Dictionary<ulong, Vector3>();
    public Dictionary<ulong, Quaternion> m_clientWorldSpaceRotationTable = new Dictionary<ulong, Quaternion>();
    public Dictionary<ulong, Vector3> m_clientAnchorPositionTable = new Dictionary<ulong, Vector3>();
    public Dictionary<ulong, Quaternion> m_clientAnchorRotationTable = new Dictionary<ulong, Quaternion>();

    void Awake()
    {
        Instance = this;
    }

    #endregion
    // ======================================================
    #region AI Processing

    public async void ProcessUserInput(string text, ulong senderClientId)
    {
        if (!IsServer) return;

        Debug.Log($"Client: {senderClientId} input: {text}");
        string response = await CallIntentionalAI(text);
        SendResponseToClients(response);
    }

    async Task<string> CallIntentionalAI(string input)
    {
        // simulate latency
        await Task.Delay(500);
        return $"AI Response: {input}";
    }

    #endregion
    // ======================================================
    #region Networking

    void SendResponseToClients(string response)
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var player = client.PlayerObject.GetComponent<NetworkClient>();
            player.ReceiveAIResponseClientRpc(response);
        }
    }

    public void UpdateClientLocation(ulong clientId, Vector3 gPosition, Quaternion gRotation, Vector3 aPosition, Quaternion aRotation)
    {
        if (!IsServer) return;

        m_clientWorldSpacePositionTable[clientId] = gPosition;
        m_clientWorldSpaceRotationTable[clientId] = gRotation;

        m_clientAnchorPositionTable[clientId] = aPosition;
        m_clientAnchorRotationTable[clientId] = aRotation;

        Debug.Log($"Updated location for client {clientId}: WorldPos {gPosition}, WorldRot {gRotation}, AnchorPos {aPosition}, AnchorRot {aRotation}");
    }

    #endregion
}
