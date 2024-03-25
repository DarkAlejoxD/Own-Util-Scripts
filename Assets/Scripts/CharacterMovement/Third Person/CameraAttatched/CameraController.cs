using System.Collections;
using UnityEngine;
using UtilsComplements;

namespace CharacterMovement.ThirdPerson.Attached
{
    #region Report
    //Made in Class
    //Edited By DarkAlejoxD, Camilo Londoño

    //Partial Role: PartialEnd
    //Current State: PartialEnd
    //Last checked: March 2024
    //Last modification: March 2024

    //Direct dependencies of classes if imported file by file:
    //  -   None, works by itself

    //Commentaries:
    //  -   It works weird, i'm too lazy to rework it.

    //TODO: 
    //  -   Rework it
    //  -   Data Base by its own?? or maybe make a partial of the GameValues
    #endregion

    /// <summary>
    /// Functionality that makes the Camera orbitates the Player
    /// </summary>
    public partial class CameraController : MonoBehaviour
    {
        #region Fields
        [Header("References")]
        [SerializeField] private Transform _lookAt;
        private InitialTransform _initialTransform;

        [Header("Collision Attributes")]
        [SerializeField] private LayerMask _avoidObstaclesMask;
        [SerializeField] private float _offsetAvoidObstacles = 0.1f;
        [SerializeField] private float _minDistance = 4.0f;
        [SerializeField] private float _maxDistance = 8.0f;
        #endregion

        #region Partial Methods
        partial void GetDataContainer();

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
            GetDataContainer();
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
                yawSpeed = PcYawSpeed;
                pitchSpeed = PcPitchSpeed;
            }
            else
            {
                yawSpeed = GamePadYawSpeed;
                pitchSpeed = GamePadPitchSpeed;
            }

            float mouseX = cameraMov.x;
            float mouseY = cameraMov.y;

            yaw += mouseX * (yawSpeed * Mathf.Deg2Rad) * Time.deltaTime;
            float l_InversedPitchControl = PitchInversed ? -1 : 1;
            pitch += mouseY * (pitchSpeed * Mathf.Deg2Rad) * Time.deltaTime * l_InversedPitchControl;
            pitch = Mathf.Clamp(pitch, MinPitch * Mathf.Deg2Rad, MaxPitch * Mathf.Deg2Rad);

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

        private const KeyCode DEBUG_showMouseKey = KeyCode.O;
        private const float DEBUG_toogleCD = 0.5f;

        private void DEBUG_ToogleMouseVision()
        {
#if UNITY_EDITOR
            if (!DEBUG_canToogle)
                return;

            if (!Input.GetKeyDown(DEBUG_showMouseKey))
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

    #region CHOOSE DATA STYLE (one of both has to be uncommented)

    #region By Same Class
    //public partial class CameraController : MonoBehaviour
    //{
    //    #region Angular Attributes
    //    [Header("Attributes (In Degrees)")]
    //    [SerializeField] private float _minPitch = -40;
    //    [SerializeField] private float _maxPitch = 70;
    //    [SerializeField] private bool _pitchInversed = true;

    //    private float MinPitch => _minPitch;
    //    private float MaxPitch => _maxPitch;
    //    private bool PitchInversed => _pitchInversed;
    //    #endregion

    //    #region Device Attributes
    //    [Header("Attributes PC")]
    //    [SerializeField] private float _pcYawSpeed = 20;
    //    [SerializeField] private float _pcPitchSpeed = 20;

    //    [Header("Attributes Gamepad")]
    //    [SerializeField] private float _gamePadYawSpeed = 360;
    //    [SerializeField] private float _gamePadPitchSpeed = 360;

    //    private float PcYawSpeed => _pcYawSpeed;
    //    private float PcPitchSpeed => _pcPitchSpeed;

    //    private float GamePadPitchSpeed => _gamePadPitchSpeed;
    //    private float GamePadYawSpeed => _gamePadYawSpeed;
    //    #endregion
    //}
    #endregion

    #region By Game Data
    [RequireComponent(typeof(DataContainer))]
    public partial class CameraController : MonoBehaviour
    {

        private DataContainer _dataContainer;
        private GameValues GameData => _dataContainer.GameData;

        private float MinPitch => GameData.MinPitch;
        private float MaxPitch => GameData.MaxPitch;
        private bool PitchInversed => GameData.PitchInversed;


        private float PcYawSpeed => GameData.PcYawSpeed;
        private float PcPitchSpeed => GameData.PcPitchSpeed;

        private float GamePadPitchSpeed => GameData.GamePadPitchSpeed;
        private float GamePadYawSpeed => GameData.GamePadYawSpeed;


        partial void GetDataContainer() => _dataContainer = GetComponent<DataContainer>();
    }
    #endregion

    #endregion
}