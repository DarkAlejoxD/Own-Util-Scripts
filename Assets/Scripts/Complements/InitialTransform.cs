using UnityEngine;

namespace UtilsComplements
{
    public struct InitialTransform
    {
        public Transform _transform;
        public Vector3 _initialPos;
        public Quaternion _initialRotation;
        public Vector3 _initialScale;

        public void SetInitialValues(Transform transform)
        {
            _transform = transform;
            _initialPos = transform.position;
            _initialRotation = transform.rotation;
            _initialScale = transform.localScale;
        }

        public void ResetGameObject()
        {
            _transform.SetPositionAndRotation(_initialPos, _initialRotation);
            _transform.localScale = _initialScale;
        }
    }
}