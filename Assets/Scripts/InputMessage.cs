using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InputMessage : MonoBehaviour
{
    private PlayerInputAction playerInputAction;
    public event EventHandler OnInteractAction;
    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();

        playerInputAction.Player.Interact.performed += Interact_performed;
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
}
