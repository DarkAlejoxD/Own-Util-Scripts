using System;
using UnityEngine;

namespace InputManagerController
{
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

    #region ChooseInputs

    public partial class InputManager : MonoBehaviour
    {
        private partial Vector2 GetThirdCameraInput()
        {
            //Controlled by the mouse Only
            float horizontalMovement = Input.GetAxis("Mouse X");
            float verticalMovement = Input.GetAxis("Mouse Y");

            return new(horizontalMovement, verticalMovement);
        }
    }

    //TODO: ADD it to the Player Map

    #endregion
}
