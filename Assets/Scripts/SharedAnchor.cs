using UnityEngine;
using UnityEngine.InputSystem;

public class SharedAnchor : MonoBehaviour
{
    [SerializeField] private Renderer[] m_renderersToToggle;

    [SerializeField] private Transform m_leftControllerTransform;
    [SerializeField] private InputActionReference m_ToggleCallibration;
    public bool Callibrating = false;

    void OnEnable()
    {
        m_ToggleCallibration.action.Enable();
        m_ToggleCallibration.action.performed += ToggleCallibration;
    }

    void OnDisable()
    {
        m_ToggleCallibration.action.Disable();
        m_ToggleCallibration.action.performed -= ToggleCallibration;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Callibrating)
        {
            gameObject.transform.position = m_leftControllerTransform.position;
            gameObject.transform.rotation = m_leftControllerTransform.rotation;
        }
    }

    public void ToggleCallibration(InputAction.CallbackContext context)
    {
        Callibrating = !Callibrating;
        // foreach (var renderer in m_renderersToToggle)
        // {
        //     renderer.enabled = !Callibrating;
        // }
    }

}
