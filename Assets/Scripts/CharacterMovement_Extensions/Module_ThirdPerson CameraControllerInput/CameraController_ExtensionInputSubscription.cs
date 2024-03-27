using InputManagerController;
using UnityEngine;

namespace CharacterMovement.ThirdPerson.Attached
{
    #region Report
    //Made by DarkAlejoxD, Camilo Londoño

    //Partial Role: Branch
    //Current State: PartialEnd
    //Last checked: March 2024
    //Last modification: March 2024

    //Commentaries:
    //  -   This part of the partial class task is to subscribe the inputmanager
    #endregion

    public partial class CameraController : MonoBehaviour
    {
        [Header("Player Reference")]
        [SerializeField] private InputManager _inputManager;

        partial void SubscribeToMouse()
        {
            if (!_inputManager)
                return;

            _inputManager.OnThirdCameraMove += RotationUpdate;
        }

        partial void UnsubscribeToMouse()
        {
            if (!_inputManager)
                return;

            _inputManager.OnThirdCameraMove -= RotationUpdate;
        }
    }
}
