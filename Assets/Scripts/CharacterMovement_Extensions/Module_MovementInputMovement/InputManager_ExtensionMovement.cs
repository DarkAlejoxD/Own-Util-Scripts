using System;
using UnityEngine;

namespace InputManagerController
{
    #region MovementInputMain

    #region Report
    //Made by DarkAlejoxD, Camilo Londoño

    //Partial Role: Branch/Extension
    //Current State: WIP
    //Last checked: March 2024
    //Last modification: March 2024

    //Direct dependencies of classes if imported file by file:
    //  -   Input System package
    //  -   UtilsComplements.ISingleton<T>.cs
    #endregion

    //Part of the InputManager that implements the movement of the Player
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

        #region Partial Methods

        private partial Vector2 MovementInput();
        private partial bool SprintInput();

        partial void UpdateTypeOfInput();
        partial void PointnClickUpdate();

        #endregion

        #region Unity Logic
        partial void MovementUpdate()
        {
            Move();
            Sprint();
            PointnClickUpdate();
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

            Vector2 movement = MovementInput();

            if (_movementStyles == MovementStyles.POINTNCLICK)
            {
                if (movement == Vector2.zero)
                    return;

                _movementStyles = MovementStyles.CLASIC;
                UpdateTypeOfInput();
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

    #region InputChoose / One of both has to be uncommented

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