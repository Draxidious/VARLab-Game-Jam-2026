using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
// using Unity.Services.Core;
// using Unity.Services.Authentication;
// using Unity.Services.Relay;
using System.Threading.Tasks;

public class NetworkSetup : MonoBehaviour
{
    // // ======================================================
    // #region Fields
    // [SerializeField] private NetworkManager m_NetworkManager;
    // [SerializeField] private GameObject m_AIPrefab;
    // [SerializeField] private GameObject m_DemoBallManager;

    // [SerializeField] private TMPro.TMP_InputField joinCodeInput;
    // [SerializeField] private TMPro.TMP_Text joinCodeDisplay;
    // [SerializeField] private GameObject m_KeyboardUI;

    // #endregion
    // // ======================================================
    // #region Initialization

    // async void Awake()
    // {
    //     await UnityServices.InitializeAsync();
    //     await AuthenticationService.Instance.SignInAnonymouslyAsync();
    // }

    // void Start()
    // {

    // }

    // #endregion
    // // ======================================================
    // #region UI Interaction

    // public async void StartHost()
    // {
    //     await StartHostRelay();
    //     gameObject.SetActive(false);
    //     m_KeyboardUI.SetActive(false);
    // }

    // public async void StartClient()
    // {
    //     await JoinRelay();
    //     gameObject.SetActive(false);
    //     m_KeyboardUI.SetActive(false);
    // }

    // #endregion
    // // ======================================================
    // #region Relay Operations

    // [ContextMenu("Start Host Relay")]
    // public async Task<string> StartHostRelay()
    // {
    //     var allocation = await RelayService.Instance.CreateAllocationAsync(2);

    //     var joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

    //     var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

    //     transport.SetRelayServerData(
    //         allocation.RelayServer.IpV4,
    //         (ushort)allocation.RelayServer.Port,
    //         allocation.AllocationIdBytes,
    //         allocation.Key,
    //         allocation.ConnectionData
    //     );

    //     NetworkManager.Singleton.StartHost();
    //     joinCodeDisplay.text = $"Join Code: {joinCode}";
    //     Debug.Log("Join Code: " + joinCode);

    //     GameObject aiInstance = Instantiate(m_AIPrefab);
    //     aiInstance.GetComponent<NetworkObject>().Spawn();

    //     GameObject m_DemoBallManagerInstance = Instantiate(m_DemoBallManager);
    //     m_DemoBallManagerInstance.GetComponent<NetworkObject>().Spawn();

    //     return joinCode;
    // }

    // [ContextMenu("Join Host Relay")]
    // public async Task JoinRelay()
    // {
    //     string joinCode = joinCodeInput.text;

    //     var allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

    //     var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

    //     transport.SetRelayServerData(
    //         allocation.RelayServer.IpV4,
    //         (ushort)allocation.RelayServer.Port,
    //         allocation.AllocationIdBytes,
    //         allocation.Key,
    //         allocation.ConnectionData,
    //         allocation.HostConnectionData
    //     );

    //     NetworkManager.Singleton.StartClient();
    // }

    // #endregion
}
