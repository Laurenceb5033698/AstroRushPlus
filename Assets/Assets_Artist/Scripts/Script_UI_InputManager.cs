using UnityEngine;
using UnityEngine.InputSystem;

public class Script_UI_InputManager : MonoBehaviour
{
    public static Script_UI_InputManager instance;

    public Vector2 NavigationInput {get; set; }

    private InputAction _navigationAction;

    public static PlayerInput PlayerInput {get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }       

        PlayerInput = GetComponent<PlayerInput>();
    _navigationAction = PlayerInput.actions["Navigate"];
    }

    private void Update()
    {
        NavigationInput = _navigationAction.ReadValue<Vector2>();
    }

}
