using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static event Action OnMoveLeft;
    public static event Action OnMoveRight;
    public static event Action OnPause;
    private PlayerControls _controls;

    private void Awake()
    {
        _controls = new PlayerControls();

        _controls.Gameplay.MoveLeft.performed += ctx => OnMoveLeft?.Invoke();
        _controls.Gameplay.MoveRight.performed += ctx => OnMoveRight?.Invoke();
        _controls.Gameplay.Pause.performed += ctx => OnPause?.Invoke();
    }

    private void OnEnable() => _controls.Enable();
    private void OnDisable() => _controls.Disable();
}