using UnityEngine;

namespace UtilsComplements
{
    [RequireComponent(typeof(Canvas))]
    public class UILookAtCamera : MonoBehaviour
    {
        private Camera _camera;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _camera = Camera.main;
            _rectTransform = GetComponent<RectTransform>();
        }

        private void FixedUpdate()
        {
            _rectTransform.LookAt(_camera.transform.position, new(0, 1, 0));
        }
    }
}