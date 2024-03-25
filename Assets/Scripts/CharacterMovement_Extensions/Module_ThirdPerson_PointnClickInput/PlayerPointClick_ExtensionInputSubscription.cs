using UnityEngine;
using InputManagerController;
using UtilsComplements;

namespace CharacterMovement.ThirdPerson.FreeCamera
{
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
