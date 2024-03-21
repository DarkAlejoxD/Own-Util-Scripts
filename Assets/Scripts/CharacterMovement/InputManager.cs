using System;
using UnityEngine;
using UtilsComplements;

#region Report
//Made by DarkAlejoxD, Camilo Londoño
//Last checked: March 2024
//Last modification: March 2024

//Commentaries:
//  -   Original script had more inputs and methods implemented, but this class is meant to be a 
//      template.
//  -   This inputManager Depends on Input system package 1.7.0 in Unity 2022.3.16f1. If gives any
//      problem, consider rewrite it.
//  -   Add it to CharacterMovement namespace (??????
#endregion

/// <summary>
/// Base/Template for an InputManager.
/// </summary>
public class InputManager : MonoBehaviour, ISingleton<InputManager>
{
    private enum MovementStyles
    {
        CLASIC, POINTNCLICK
    }

    [Header("ActionReferences")]
    [SerializeField] private PlayerMap _playerInput;

    [Header("Attributes")]
    [SerializeField] private MovementStyles _movementStyles = MovementStyles.CLASIC;
    //private bool _moveByInputs;

    [Header("Actions/Influencers")]
    public static Action<Vector2> OnMove;
    public static Action<bool> OnSprint;

    public static Action OnClick;
    public static Action<bool> OnChange;

    public ISingleton<InputManager> Instance => this;
    public InputManager Value => this;

    #region Unity Logic
    private void Awake()
    {
        Instance.Instantiate();

        _playerInput = new PlayerMap();
        _playerInput.Player.Enable();
    }

    private void Update()
    {
        Move();
        Sprint();
    }

    private void OnDestroy()
    {
        Instance.RemoveInstance();
    }
    #endregion

    #region Public Methods
    public void Invalidate()
    {
        Destroy(gameObject);
    }
    #endregion

    #region Private Methods
    private void Move()
    {
        if (!_playerInput.Player.enabled)
            return;

        Vector2 movement = _playerInput.Player.Move.ReadValue<Vector2>();

        if (_movementStyles == MovementStyles.POINTNCLICK)
        {
            if (movement == Vector2.zero)
                return;

            _movementStyles = MovementStyles.CLASIC;
        }
            OnMove?.Invoke(movement);
    }

    private void Sprint()
    {
        bool buttonPressed = _playerInput.Player.Sprint.IsPressed();
        OnSprint?.Invoke(buttonPressed);
    }
    #endregion
}