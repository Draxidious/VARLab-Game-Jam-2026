using UnityEngine;
using Unity.Netcode;
using Unity.XR.CoreUtils;

public class DemoBall : NetworkBehaviour
{
    // TODO rename to server simulator
    public float speed = 2f;
    private BallServer m_ballManager;

    void Start()
    {
        if (!IsServer) return;

        m_ballManager = BallServer.Instance;
    }

    void Update()
    {
        if (!IsServer) return;

        // this basically only lerps between the two clients' head positions
        // if theres two clients connected
        if (m_ballManager != null && m_ballManager.m_clientAnchorPositionTable.Count == 2)
        {
            Vector3 p1AnchorPos = m_ballManager.m_clientAnchorPositionTable[0];
            Vector3 p2AnchorPos = m_ballManager.m_clientAnchorPositionTable[1];
            Quaternion p1AnchorRot = m_ballManager.m_clientAnchorRotationTable[0];
            Quaternion p2AnchorRot = m_ballManager.m_clientAnchorRotationTable[1];

            Matrix4x4 p1Anchor = Matrix4x4.TRS(p1AnchorPos, p1AnchorRot, Vector3.one);
            Matrix4x4 p2Anchor = Matrix4x4.TRS(p2AnchorPos, p2AnchorRot, Vector3.one);

            Matrix4x4 p1InverseAnchor = p1Anchor.inverse;
            Matrix4x4 p2InverseAnchor = p2Anchor.inverse;

            Matrix4x4 p1World = Matrix4x4.TRS(
                                            m_ballManager.m_clientWorldSpacePositionTable[0],
                                            m_ballManager.m_clientWorldSpaceRotationTable[0],
                                            Vector3.one);

            Matrix4x4 p2World = Matrix4x4.TRS(
                                            m_ballManager.m_clientWorldSpacePositionTable[1],
                                            m_ballManager.m_clientWorldSpaceRotationTable[1],
                                            Vector3.one);

            Matrix4x4 p1Prime = p1InverseAnchor * p1World;
            Matrix4x4 p2Prime = p2InverseAnchor * p2World;

            Vector3 trueBallPosV3 = Vector3.Lerp(p1Prime.GetPosition(), p2Prime.GetPosition(), Mathf.Sin(Time.time) * .5f + .5f);
            Vector4 trueBallPos = new Vector4(trueBallPosV3.x, trueBallPosV3.y, trueBallPosV3.z, 1.0f);

            Vector4 p1Ball = p1Anchor * trueBallPos;
            Vector4 p2Ball = p2Anchor * trueBallPos;

            Vector3 p1BallV3 = new Vector3(p1Ball.x, p1Ball.y, p1Ball.z);
            Vector3 p2BallV3 = new Vector3(p2Ball.x, p2Ball.y, p2Ball.z);

            NetworkManager.Singleton.ConnectedClientsList[0].PlayerObject.GetComponent<NetworkClient>().ReceiveDemoBallClientRpc(p1BallV3);
            NetworkManager.Singleton.ConnectedClientsList[1].PlayerObject.GetComponent<NetworkClient>().ReceiveDemoBallClientRpc(p2BallV3);
        }

    }
}