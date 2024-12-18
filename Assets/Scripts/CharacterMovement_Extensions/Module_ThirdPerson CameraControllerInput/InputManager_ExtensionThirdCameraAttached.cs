﻿using System;
using UnityEngine;

namespace InputManagerController
{
    #region Report
    //Made by DarkAlejoxD

    //Partial Role: Branch
    //Current State: PartialEn
    //Last checked: March 2024
    //Last modification: March 2024

    //Commentaries:
    //  -   This part of the InputManager makes the function of the CameraMovement
    #endregion

    #region Main
    public partial class InputManager : MonoBehaviour
    {
        [Header("Actions")]
        public Action<Vector2> OnThirdCameraMove;

        #region Partial Methods
        private partial Vector2 GetThirdCameraInput();
        #endregion

        #region UnityLogic

        partial void ThirdCameraAttatchedUpdate()
        {
            OnThirdCameraMove?.Invoke(GetThirdCameraInput());
        }

        #endregion
    }
    #endregion

    #region CHOOSE INPUTS, one has to be uncommented

    #region Old Inputs
    //public partial class InputManager : MonoBehaviour
    //{
    //    private partial Vector2 GetThirdCameraInput()
    //    {
    //        //Controlled by the mouse Only
    //        float horizontalMovement = Input.GetAxis("Mouse X");
    //        float verticalMovement = Input.GetAxis("Mouse Y");

    //        return new(horizontalMovement, verticalMovement);
    //    }
    //}
    #endregion

    #region New Input System
    public partial class InputManager : MonoBehaviour
    {
        private partial Vector2 GetThirdCameraInput()
        {
            Vector2 movement = _playerInput.ThirdCamera.MoveCamera.ReadValue<Vector2>();
            return movement;
        }
    }
    #endregion

    #endregion
}
