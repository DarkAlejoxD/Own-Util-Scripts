using UnityEngine;

namespace InputManagerController
{
    #region Report
    //Made by DarkAlejoxD

    //Partial Role: Branch
    //Last checked: March 2024
    //Last modification: March 2024

    //Direct dependencies of classes if imported file by file:
    //  -   Input Manager Controller Folder

    //Commentaries:
    //  -   Techniocally it can be used with ol Inputs, but there is a comprobation im too lazy to change
    //      and I dont really know how to sustitute it.

    //TODO: 
    //  -   Redo the block Inputs for Old Inputs(??
    #endregion

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