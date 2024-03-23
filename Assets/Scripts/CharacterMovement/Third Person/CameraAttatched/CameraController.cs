using System.Collections;
using UnityEngine;
using UtilsComplements;

namespace CharacterMovement.ThirdPerson.Attached
{
    public partial class CameraController : MonoBehaviour
    {
        #region Fields
        [Header("References")]
        [SerializeField] private Transform _lookAt;

        [Header("Attributes (In Degrees)")]
        [SerializeField] private float _minPitch = -40;
        [SerializeField] private float _maxPitch = 70;
        [SerializeField] private bool _pitchInversed = true;
        private InitialTransform _initialTransform;

        [Header("Attributes PC")]
        [SerializeField] private float _pcYawSpeed = 20;
        [SerializeField] private float _pcPitchSpeed = 30;

        [Header("Attributes Gamepad")]
        [SerializeField] private float _gamePadYawSpeed = 360;
        [SerializeField] private float _gamePadPitchSpeed = 360;

        [Header("Collision Attributes")]
        [SerializeField] private LayerMask _avoidObstaclesMask;
        [SerializeField] private float _offsetAvoidObstacles = 0.1f;
        [SerializeField] private float _minDistance = 4.0f;
        [SerializeField] private float _maxDistance = 8.0f;
        #endregion

        #region Partial Methods

        partial void SubscribeToMouse();
        partial void UnsubscribeToMouse();

        partial void FSMInit();
        partial void FSMUpdate();
        #endregion

        #region UnityLogic
        private void Awake()
        {
            _initialTransform.SetInitialValues(transform);
            FSMInit();
        }

        private void OnEnable()
        {
            SubscribeToMouse();            
        }

        private void OnDisable()
        {
            UnsubscribeToMouse();
        }

        private void LateUpdate()
        {
            FSMUpdate();

            DEBUG_ToogleMouseVision();
        }        
        #endregion

        #region Public Methods
        public void RestartObject()
        {
            _initialTransform.ResetGameObject();
            FSMInit();
        }
        #endregion

        #region Private Methods        
        private void RotationUpdate(Vector2 input)
        {
            #region Rotation
            transform.LookAt(_lookAt);
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            float distance = Vector3.Distance(transform.position, _lookAt.position);

            float yaw = eulerAngles.y * Mathf.Deg2Rad;
            float pitch = eulerAngles.x * Mathf.Deg2Rad;
            if (pitch > Mathf.PI)
                pitch -= 2 * Mathf.PI;

            Vector2 cameraMov = input;

            float yawSpeed = 0;
            float pitchSpeed = 0;

            if (cameraMov.magnitude > 1)
            {
                yawSpeed = _pcYawSpeed;
                pitchSpeed = _pcPitchSpeed;
            }
            else
            {
                yawSpeed = _gamePadYawSpeed;
                pitchSpeed = _gamePadPitchSpeed;
            }

            float mouseX = cameraMov.x;
            float mouseY = cameraMov.y;

            yaw += mouseX * (yawSpeed * Mathf.Deg2Rad) * Time.deltaTime;
            float l_InversedPitchControl = _pitchInversed ? -1 : 1;
            pitch += mouseY * (pitchSpeed * Mathf.Deg2Rad) * Time.deltaTime * l_InversedPitchControl;
            pitch = Mathf.Clamp(pitch, _minPitch * Mathf.Deg2Rad, _maxPitch * Mathf.Deg2Rad);

            Vector3 forward = new Vector3(Mathf.Sin(yaw) * Mathf.Cos(-pitch),     //x
                                            Mathf.Sin(-pitch),                        //y
                                            Mathf.Cos(yaw) * Mathf.Cos(-pitch));    //z
            #endregion

            #region Avoid Obstacles
            distance = Mathf.Clamp(distance, _minDistance, _maxDistance);
            Vector3 desiredPos = _lookAt.position - forward * distance;

            Ray ray = new(_lookAt.position, -forward);

            if (Physics.Raycast(ray, out RaycastHit l_RayHitInfo, distance, _avoidObstaclesMask.value))
                desiredPos = l_RayHitInfo.point + forward * _offsetAvoidObstacles;
            #endregion

            transform.position = desiredPos;
            transform.LookAt(_lookAt);
        }
        #endregion

        #region DEBUG

        private bool DEBUG_canToogle = true;
        private bool DEBUG_toogleState;

        private const KeyCode DEBUG_showMouseKey = KeyCode.M;
        private const float DEBUG_toogleCD = 0.5f;

        private void DEBUG_ToogleMouseVision()
        {
#if UNITY_EDITOR
            if(!DEBUG_canToogle) 
                return;

            if(!Input.GetKeyDown(DEBUG_showMouseKey)) 
                return;

            if (DEBUG_toogleState)
            {
                DEBUG_toogleState = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                DEBUG_toogleState = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            StartCoroutine(DEBUG_mouseCDCoroutine());
#endif
        }

        private IEnumerator DEBUG_mouseCDCoroutine()
        {
            DEBUG_canToogle = false;
            yield return new WaitForSeconds(DEBUG_toogleCD);
            DEBUG_canToogle = true;
        }

        #endregion
    }
}