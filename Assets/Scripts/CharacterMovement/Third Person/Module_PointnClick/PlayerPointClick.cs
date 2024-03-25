using UnityEngine;
using UnityEngine.AI;
using UtilsComplements;

namespace CharacterMovement.ThirdPerson.FreeCamera
{
    #region Report
    //Made by DarkAlejoxD, Camilo Londoño
    //Ft. Skfod, Alex Morillas

    //Partial Role: Main
    //Current State: PartialEnd
    //Last checked: March 2024
    //Last modification: March 2024

    //Direct dependencies of classes if imported file by file:
    //  -   NavMesh AI package
    //  -   UtilsComplements folder
    #endregion

    [RequireComponent(typeof(NavMeshAgent), typeof(PlayerMovement), typeof(DataContainer))]
    public partial class PlayerPointClick : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LayerMask _layerMask;
        private NavMeshAgent _aiAgent;
        private PlayerMovement _playerMovement;
        private DataContainer _dataContainer;

        [Header("Attributes")]
        private bool _isUsingMouse;

        private GameValues GameData => _dataContainer.GameData;
        private float DistanceToSlowDown => GameData.RadiusToSlowDown;
        private float DistanceToStop => GameData.RadiusToStop;

        #region Partial Methods
        partial void InputManagerSubscription();
        partial void InputManagerUnsubcription();

        #endregion

        #region Unity Logic
        private void Awake()
        {
            _dataContainer = GetComponent<DataContainer>();
            _aiAgent = GetComponent<NavMeshAgent>();
            _playerMovement = GetComponent<PlayerMovement>();
            _aiAgent.isStopped = true;
        }

        private void OnEnable()
        {
            InputManagerSubscription();
        }

        private void OnDisable()
        {
            InputManagerUnsubcription();
        }

        private void FixedUpdate()
        {
            PathFinding();
        }

        private void PathFinding()
        {
            if (!_isUsingMouse)
                return;

            Vector3[] list = _aiAgent.path.corners;
            if (list.Length > 1)
            {
                Vector3 nextPos = list[1];
                float intensity = 1;

                if (list.Length > 2)
                {
                    nextPos = list[2];
                }
                else
                {
                    Vector3 distanceTargetPlayer = transform.position - nextPos;

                    if (distanceTargetPlayer.magnitude < DistanceToStop)
                    {
                        intensity = 0;
                    }
                    else
                    {
                        Vector3 virtualPos = nextPos + distanceTargetPlayer.normalized * DistanceToStop;
                        float distanceToVirtualPos = Vector3.Distance(transform.position, virtualPos);
                        float distanceToStop = DistanceToSlowDown - DistanceToStop;

                        intensity = Mathf.Clamp01(distanceToVirtualPos / distanceToStop);
                    }
                }

                Vector3 direction = nextPos - transform.position;
                direction.y = 0f;

                Vector3 newDirection = direction.normalized * intensity;

                _playerMovement.MoveFixedDT(newDirection);
            }
        }

        #endregion

        #region Public Methods

        public void Teleport(Transform dest)
        {
            //_aiAgent.destination = dest.position;
            //_aiAgent.isStopped = true;
            _aiAgent.enabled = false;
            transform.rotation = dest.rotation;
            _aiAgent.Warp(dest.position);
            _aiAgent.enabled = true;
        }

        #endregion

        #region Private Methods
        private void OnClick()
        {
            //Debug.Log("OnClick()");
            if (_isUsingMouse)
            {
                //Debug.Log("_isUsingMouse");
                Vector3 mousePos = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(mousePos);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask))
                {
                    //Debug.Log("hitwithsomtg");
                    _aiAgent.destination = hit.point;
#if UNITY_EDITOR
                    DEBUG_Position = hit.point;
#endif
                    return;
                }
                _aiAgent.destination = transform.position;
            }
        }

        private void OnChange(bool moveByInputs)
        {
            if (moveByInputs)
            {
                _isUsingMouse = false;
                _aiAgent.isStopped = true;
                _aiAgent.enabled = false;
            }
            else
            {
                _isUsingMouse = true;
                _aiAgent.enabled = true;
                _aiAgent.isStopped = true;
            }
        }
        #endregion

        #region DEBUG
#if UNITY_EDITOR

        [Header("DEBUG")]
        private Vector3 DEBUG_Position;
        private bool DEBUG_DrawNextLocationAI => GameData.DEBUG_DrawNextLocation;
        private bool DEBUG_DrawRadiusToSlowDown => GameData.DEBUG_DrawSlowDown;
        private bool DEBUG_DrawRadiusToStop => GameData.DEBUG_DrawStop;

        private void OnDrawGizmos()
        {
            if (_dataContainer == null)
                _dataContainer = GetComponent<DataContainer>();

            GizmosUtilities.DrawSphere(DEBUG_Position, Color.red, DEBUG_DrawNextLocationAI);

            GizmosUtilities.DrawCircularZone(transform, DistanceToSlowDown, Color.blue,
                                            DEBUG_DrawRadiusToSlowDown);
            GizmosUtilities.DrawCircularZone(transform, DistanceToStop, Color.blue,
                                            DEBUG_DrawRadiusToStop);
        }
#endif
        #endregion
    }
}