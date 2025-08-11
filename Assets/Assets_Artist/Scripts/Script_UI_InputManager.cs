using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gets inputs from PlayerInput for navigation and menu interaction.
/// </summary>
public class Script_UI_InputManager : MonoBehaviour
{
    public static Script_UI_InputManager instance;

    public Vector2 NavigationInput {get; set; }
    public bool SubmitInput { get; set; }
    public bool CancelInput { get; set; }
    public bool RightBumperInput { get; set; }
    public bool LeftBumperInput { get; set; }
    public bool HoldActionInput {  get; set; }

    private InputAction _navigationAction;
    private InputAction _SubmitAction;
    private InputAction _CancelAction;
    private InputAction _HeaderNavigateRightAction;
    private InputAction _HeaderNavigateLeftAction;
    public InputAction _HoldAction;

    public static PlayerInput PlayerInput {get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(this);
        //Debug.Log("Script_UI_InputManager: " + gameObject.name);

        PlayerInput = GetComponent<PlayerInput>();
        _navigationAction = PlayerInput.actions["Navigate"];

        _SubmitAction = PlayerInput.actions["Submit"];
        _CancelAction = PlayerInput.actions["Cancel"];

        _HeaderNavigateRightAction = PlayerInput.actions["HeaderRightBumper"];
        _HeaderNavigateLeftAction = PlayerInput.actions["HeaderLeftBumper"];

        _HoldAction = PlayerInput.actions["HoldAction"];
    }

    /// <summary>
    /// Get input from controls. UImanager/currentSCreen reads the values set here for ui interaction.
    /// </summary>
    private void Update()
    {
        //get controller/keyboard inputs
        NavigationInput = _navigationAction.ReadValue<Vector2>();
        SubmitInput = _SubmitAction.triggered;
        CancelInput = _CancelAction.triggered;
        RightBumperInput = _HeaderNavigateRightAction.triggered;
        LeftBumperInput = _HeaderNavigateLeftAction.triggered;

        HoldActionInput = _HoldAction.IsPressed();
    }

}
