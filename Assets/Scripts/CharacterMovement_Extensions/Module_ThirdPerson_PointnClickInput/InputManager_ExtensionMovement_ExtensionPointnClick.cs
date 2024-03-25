using UnityEngine;

namespace InputManagerController
{
    partial class InputManager : MonoBehaviour
    {
        private partial bool PointClickInput();

        partial void UpdateTypeOfInput()
        {
            if (_movementStyles == MovementStyles.CLASIC)
                _moveByInputs = true;

            else
                _moveByInputs = false;

            OnChange?.Invoke(_moveByInputs);
        }

        partial void PointnClickUpdate()
        {
            if (!_playerInput.Player.enabled) //Im not sure how to translate this to old input System
                return;

            bool trigered = PointClickInput();

            if (_movementStyles == MovementStyles.CLASIC)
            {
                if (!trigered)
                    return;

                _movementStyles = MovementStyles.POINTNCLICK;
                UpdateTypeOfInput();
            }

            if (trigered)
                OnClick?.Invoke();
        }
    }

    #region InputChoose / One of both has to be uncommented

    #region By New Input System

    public partial class InputManager
    {
        private partial bool PointClickInput() => _playerInput.Player.PointnClick.triggered;
    }

    #endregion

    #region By Old Input System
    //public partial class InputManager
    //{
    //    private partial bool PointClickInput() => Input.GetMouseButtonDown(0);
    //}
    #endregion

    #endregion
}