using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NetworkClient : NetworkBehaviour
{
    // ======================================================
    #region Initialization
    [SerializeField] private InputActionReference m_TestSendAction;
    [SerializeField] private Transform m_mainCameraTransform;
    [SerializeField] private Transform m_sharedCoordinateAnchor;
    private Transform m_demoBallTransform;
    private TMP_Text m_debugText;

    private float m_sendDelay = .05f;
    private float m_lastSendTime = 0f;


    void OnEnable()
    {
        m_TestSendAction.action.Enable();
        m_TestSendAction.action.performed += OnTestSend;
    }

    void OnDisable()
    {
        m_TestSendAction.action.Disable();
        m_TestSendAction.action.performed -= OnTestSend;
    }

    void Start()
    {
        if (!IsOwner) return;

        Debug.Log($"Client {OwnerClientId} is ready");

        m_mainCameraTransform = GameObject.FindWithTag("MainCamera").transform;
        m_demoBallTransform = GameObject.FindWithTag("DemoBall").transform;
        // m_sharedCoordinateAnchor = GameObject.FindGameObjectWithTag("SharedAnchor").transform;
        m_sharedCoordinateAnchor = GameObject.FindWithTag("SharedAnchor").transform;
    }

    void Update()
    {
        if (!IsOwner) return;

        m_lastSendTime += Time.deltaTime;
        if (m_lastSendTime > m_sendDelay)
        {
            m_lastSendTime = 0f;
            // SubmitLocationServerRpc(m_mainCameraTransform.localToWorldMatrix, m_sharedCoordinateAnchor.localToWorldMatrix);
            SubmitLocationServerRpc(m_mainCameraTransform.position, m_mainCameraTransform.rotation, m_sharedCoordinateAnchor.position, m_sharedCoordinateAnchor.rotation);
        }
    }

    #endregion
    // ======================================================
    #region Input Handling

    void OnTestSend(InputAction.CallbackContext context)
    {
        SendSpeechToServer("Hello from HMD client!");
    }

    [ContextMenu("Test Send")]
    public void TestSend()
    {
        SendSpeechToServer("Hello from Host client!");
    }

    // TODO: Add voice input handling and call SendSpeechToServer
    // with transcribed text

    public void SendSpeechToServer(string text)
    {
        if (!IsOwner) return;

        SubmitSpeechServerRpc(text);
    }

    #endregion
    // ======================================================
    #region Networking RPCs

    [ServerRpc]
    void SubmitSpeechServerRpc(string text, ServerRpcParams rpcParams = default)
    {
        Debug.Log($"Client {rpcParams.Receive.SenderClientId} said: {text}");

        BallServer.Instance.ProcessUserInput(text, rpcParams.Receive.SenderClientId);
    }

    [ServerRpc]
    void SubmitLocationServerRpc(Vector3 gPosition, Quaternion gRotation, Vector3 aPosition, Quaternion aRotation, ServerRpcParams rpcParams = default)
    {
        ulong senderId = rpcParams.Receive.SenderClientId;
        // TODO refactor later to not send same anchor position
        BallServer.Instance.UpdateClientLocation(senderId, gPosition, gRotation, aPosition, aRotation);
    }


    [ClientRpc]
    public void ReceiveAIResponseClientRpc(string response)
    {
        if (!IsOwner) return;

        Debug.Log($"OwnerClientId: {OwnerClientId} received AI response: {response}");

        // TODO:
        // - Play TTS audio
        // - Animate AI agent
    }

    [ClientRpc]
    public void ReceiveDemoBallClientRpc(Vector3 transform)
    {
        if (!IsOwner) return;

        // Debug.Log($"OwnerClientId: {OwnerClientId} moving ball to: {transform}");
        SendSpeechToServer($"moving ball to {transform} in local space");
        m_demoBallTransform.position = transform;
    }

    #endregion
    // ======================================================
    #region AI Integration

    // Response handling 
    // TODO:
    // - Play TTS audio
    // - Animate AI agent

    #endregion
}