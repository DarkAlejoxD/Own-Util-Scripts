using InputManagerController;
using UnityEngine;

namespace CharacterMovement.ThirdPerson.Attached
{
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
