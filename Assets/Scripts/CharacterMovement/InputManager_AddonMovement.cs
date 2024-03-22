using System;
using UnityEngine;

namespace InputManagerController
{
    #region MovementInputMain

    #region Report
    //Made by DarkAlejoxD, Camilo Londoño

    //Current State: WIP
    //Last checked: March 2024
    //Last modification: March 2024

    //Direct dependencies of classes if imported file by file:
    //  -   Input System package
    //  -   UtilsComplements.ISingleton<T>.cs

    //Commentaries:
    //  -   Original script had more inputs and methods implemented, but this class is meant to be a 
    //      template.
    //  -   This inputManager Depends on Input system package 1.7.0 in Unity 2022.3.16f1. If gives any
    //      problem, consider rewrite it.
    //  -   Meant to be for a Single Player Input based.
    #endregion

    /// <summary>
    /// Base/Template for an InputManager.
    /// </summary>
    public partial class InputManager : MonoBehaviour
    {
        private enum MovementStyles
        {
            CLASIC, POINTNCLICK
        }

        [Header("Movement Attributes")]
        [SerializeField] private MovementStyles _movementStyles = MovementStyles.CLASIC;
        private bool _moveByInputs;

        [Header("Actions/Influencers")]
        public Action<Vector2> OnMove;
        public Action<bool> OnSprint;

        public Action OnClick;
        public Action<bool> OnChange;

        #region Unity Logic
        partial void MovementUpdate()
        {
            Move();
            Sprint();
        }
        #endregion

        #region Public Methods
        public void Invalidate()
        {
            Destroy(gameObject);
        }
        #endregion

        #region Private Methods

        private partial Vector2 MovementInput();
        private partial bool SprintInput();

        private void Move()
        {
            if (!_playerInput.Player.enabled)
                return;

            Vector2 movement = MovementInput();

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
            bool buttonPressed = SprintInput();
            OnSprint?.Invoke(buttonPressed);
        }
        #endregion
    }
    #endregion

    #region InputChose / One of both has to be uncommented

    #region By New Input System

    public partial class InputManager
    {
        private partial Vector2 MovementInput() => _playerInput.Player.Move.ReadValue<Vector2>();
        private partial bool SprintInput() => _playerInput.Player.Sprint.IsPressed();
    }

    #endregion

    #region By Old Input System
    /*
    public partial class InputManager
    {
        private const KeyCode upKey = KeyCode.W;
        private const KeyCode downKey = KeyCode.S;
        private const KeyCode rightKey = KeyCode.D;
        private const KeyCode leftKey = KeyCode.A;

        private const KeyCode sprintKey = KeyCode.LeftShift;

        private partial Vector2 MovementInput()
        {
            Vector2 inputDir = Vector3.zero;

            if (Input.GetKey(upKey)) inputDir = Vector2.up;

            if (Input.GetKey(downKey)) inputDir -= Vector2.up;

            if (Input.GetKey(rightKey)) inputDir += Vector2.right;

            if (Input.GetKey(leftKey)) inputDir -= Vector2.right;

            return inputDir.normalized;
        }

        private partial bool SprintInput() => Input.GetKey(sprintKey);
    }
    */
    #endregion

    #endregion
}