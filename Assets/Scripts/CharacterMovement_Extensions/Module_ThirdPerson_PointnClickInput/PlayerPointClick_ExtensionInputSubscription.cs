using UnityEngine;
using InputManagerController;

namespace CharacterMovement.ThirdPerson.FreeCamera
{
    #region Report
    //Made by DarkAlejoxD, Camilo Londoño

    //Partial Role: Branch
    //Current State: PartialEnd
    //Last checked: {Date}
    //Last modification: {Date}

    //Direct dependencies of classes if imported file by file:
    //  -   InputManagerController Folder
    #endregion

    [RequireComponent(typeof(InputManager))]
    public partial class PlayerPointClick : MonoBehaviour
    {
        partial void InputManagerSubscription()
        {
            if(!TryGetComponent<InputManager>(out var input))
                return;

            input.OnClick += OnClick;
            input.OnChange += OnChange;
        }

        partial void InputManagerUnsubcription()
        {
            if (!TryGetComponent<InputManager>(out var input))
                return;

            input.OnClick -= OnClick;
            input.OnChange -= OnChange;
        }
    }
}
