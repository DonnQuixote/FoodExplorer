  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
public class InputMessage : MonoBehaviour
{

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";
    private const string PLAYER_PREFS_BINDINGS_MOVE_ORIGION = "InputBindingsOrigion";
    public static InputMessage Instance { get; private set; }
    private PlayerInputAction playerInputAction;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteract_CutAction;
    public event EventHandler OnPause;
    public event EventHandler OnRush;
    public event EventHandler OnThrow;
    public event EventHandler OnExtinguishFire;
    public event EventHandler OnSetGunQuicklypPerformed;
    public event EventHandler OnSetGunQuicklypCancled;
    public event EventHandler<OnFireGunChangingArgs> OnFireGunChanging;

    public class OnFireGunChangingArgs : EventArgs
    {
        public Quaternion playerRotation;
    }


    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interacet,
        InteractAlternate,
        Pause,
        UpSpeed,
        ExtinguishFire,
        FireChanging,
    }

    private void Awake()
    {
        Instance = this;
        playerInputAction = new PlayerInputAction();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)){
            playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS_MOVE_ORIGION, playerInputAction.SaveBindingOverridesAsJson());
        PlayerPrefs.Save();


        playerInputAction.Player.Enable();

        //Debug.Log(playerInputAction.Player.Move.SaveBindingOverridesAsJson());

        playerInputAction.Player.Interact.performed += Interact_performed;
        playerInputAction.Player.Interact_Cut.performed += Interact_Cut_performed;
        playerInputAction.Player.Pause.performed += Pause_performed;
        playerInputAction.Player.Rush.performed += Rush_performed;
        playerInputAction.Player.Throw.performed += Throw_performed;
        playerInputAction.Player.ExtinguishFire.performed += ExtinguishFire_performed;
        playerInputAction.Player.FireChanding.performed += FireChanding_performed;
        playerInputAction.Player.SetGunQuickly.performed += SetGunQuickly_performed;
        playerInputAction.Player.SetGunQuickly.canceled += SetGunQuickly_canceled;
        //Debug.Log(GetBindingText(Binding.Move_Up));

        
    }

    private void SetGunQuickly_canceled(InputAction.CallbackContext obj)
    {
        OnSetGunQuicklypCancled?.Invoke(this, EventArgs.Empty);

    }

    private void SetGunQuickly_performed(InputAction.CallbackContext obj)
    {
        OnSetGunQuicklypPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void FireChanding_performed(InputAction.CallbackContext obj)
    {
        OnFireGunChanging?.Invoke(this, new OnFireGunChangingArgs
        {
            playerRotation = Player.Instance.transform.rotation
        }); 
    }

    private void ExtinguishFire_performed(InputAction.CallbackContext obj)
    {
        OnExtinguishFire?.Invoke(this, EventArgs.Empty);
    }

    private void Throw_performed(InputAction.CallbackContext obj)
    {
        OnThrow?.Invoke(this, EventArgs.Empty);
    }

    private void Rush_performed(InputAction.CallbackContext obj)
    {
        OnRush?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        playerInputAction.Player.Interact.performed -= Interact_performed;
        playerInputAction.Player.Interact_Cut.performed -= Interact_Cut_performed;
        playerInputAction.Player.Pause.performed -= Pause_performed;

        playerInputAction.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_Cut_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteract_CutAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 InputMovementNormalized()
    {

        Vector2 inputData = playerInputAction.Player.Move.ReadValue<Vector2>();
        /*Vector2 inputData = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            inputData.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputData.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputData.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputData.x += 1;
        }*/

        inputData = inputData.normalized;
        return inputData;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return playerInputAction.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:                              
                return playerInputAction.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:                              
                return playerInputAction.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:                             
                return playerInputAction.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interacet:
                return playerInputAction.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputAction.Player.Interact_Cut.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputAction.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding,Action OnBinding)
    {
        playerInputAction.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 1;
                Debug.Log(inputAction.bindings[1].ToString());
                break;
            case Binding.Move_Down:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 2;
                break;
        }



        inputAction.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            //.WithControlsHavingToMatchPath("<Keyborad>")
            .OnComplete(callback =>
            {
                callback.Dispose();
                playerInputAction.Player.Enable();
                OnBinding();
                Debug.Log(playerInputAction.SaveBindingOverridesAsJson());

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputAction.SaveBindingOverridesAsJson());
            })
            .Start();

        
    }

    public void PlayerPrefsMappingSave()
    {
        PlayerPrefs.Save();
    }

    public void PlayerPrefsMappingReset()
    {
        //Debug.Log("in PlayerPrefsMappingReset");
        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS_MOVE_ORIGION ));
        PlayerPrefs.Save();
        playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        KeyMappingUI.Instance.UpdateVisual();
    }
}
