using UnityEngine;
//using UtilsComplements;

namespace CharacterMovement
{
    //Made by DarkAlejoxD, Camilo Londoño
    /// <summary>
    /// Enough movement base in a 3D World
    /// </summary>
    [SelectionBase]
    [RequireComponent(typeof(CharacterController), typeof(DataContainer))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        private DataContainer _dataContainer;
        private CharacterController _characterController;

        [Header("Attributes")]
        private bool _isSprinting = false;
        private Vector3 _velocity = Vector3.zero;
        private float _velocityY;

        private Animator _animator;

        #region Attributes Getters
        public Transform CurrentCamera => Camera.main.transform; 

        private GameValues GameData => _dataContainer.GameData;
        public Vector3 Velocity => _velocity;
        private float MinSpeedToMove => GameData.MinSpeedToMove;
        private float MaxSpeed => _isSprinting ? GameData.SprintSpeed : GameData.MaxSpeed;
        private bool CanSprint => GameData.SprintEnabled;
        private float LinealAcceleration => GameData.LinealAcceleration;
        private float LinealDeceleration => GameData.LinealDeceleration;
        private bool MoveByAcceleration => GameData.UseAccelerationMovement;
        private float Gravity => Physics.gravity.y;
        #endregion

        #region Unity Logic

        private void Awake()
        {
            _dataContainer = GetComponent<DataContainer>();
            _characterController = GetComponent<CharacterController>();
            //_cameraManager = ISingleton<CameraManager>.GetInstance();
            _animator = GetComponentInChildren<Animator>();
        }
        //private void OnEnable() //Prepared to Observer Pattern and an extern InputManager Script
        //{
        //    var input = ISingleton<InputManager>.GetInstance();
        //    input.OnMove += OnMove;
        //    input.OnSprint += OnSprint;
        //}
        //private void OnDisable() //Prepared to Observer Pattern and an extern InputManager Script
        //{
        //    var input = ISingleton<InputManager>.GetInstance();
        //    input.OnMove -= OnMove;
        //    input.OnSprint -= OnSprint;
        //}

        private void Start()
        {
            _velocityY = 0;
        }

        private void FixedUpdate()
        {
            GravityUpdate(); //Comment this if another script controls gravity
            DescelerateUpdate();
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            if (_animator.GetBool("isFishing")) return;
            if (_animator.GetBool("isDrumming")) return;
            if (_animator.GetBool("isBoarding")) return;

            float animValue = 0;
            if (_velocity.magnitude < MinSpeedToMove)
            {
                _animator.SetFloat("Speed", animValue);
                return;
            }

            float magnitude = _velocity.magnitude - MinSpeedToMove;
            float maxSpeed = MaxSpeed - MinSpeedToMove;
            animValue = magnitude / maxSpeed;

            _animator.SetFloat("Speed", Mathf.Clamp01(animValue));
        }
        #endregion

        #region Public Methods

        public void MoveFixedDT(Vector3 motion)
        {
            float dt = Time.fixedDeltaTime;
            if (MoveByAcceleration)
                LinearAcceleratedMovement(motion, dt);
            else
                LinearMovement(motion, dt);
        }

        public void MoveNormalDT(Vector3 motion)
        {
            float dt = Time.deltaTime;
            if (MoveByAcceleration)
                LinearAcceleratedMovement(motion, dt);
            else
                LinearMovement(motion, dt);
        }

        public Vector3 CalculateForward()
        {
            Vector3 forward = CurrentCamera.forward;
            forward.y = 0;
            forward.Normalize();
            return forward;
        }

        public Vector3 CalculateRight()
        {
            Vector3 right = CurrentCamera.right;
            right.y = 0;
            right.Normalize();
            return right;
        }

        #endregion

        #region Private Methods

        private void OnMove(Vector2 input) //Where x is <> and y ^v^
        {
            Vector3 forward = CalculateForward();
            Vector3 right = CalculateRight();

            if (input.magnitude > 1)
                input.Normalize();

            Vector3 movement;

            movement = input.x * right;
            movement += input.y * forward;

            if (input.magnitude == 0)
                movement = Vector3.zero;

            if (movement.magnitude > 1)
                movement.Normalize();

            MoveFixedDT(movement);
        }

        private void LinearMovement(Vector3 movement, float dt) //MRU
        {
            _velocity = MaxSpeed * movement;
            Vector3 motion = _velocity * dt;

            if (motion.magnitude < MinSpeedToMove * dt)
                motion = Vector3.zero;

            _characterController.Move(motion);
            //_animator.SetFloat("Speed", motion.magnitude * 10);
        }

        private void LinearAcceleratedMovement(Vector3 movement, float dt) //MRUA
        {
            //Calculate Deceleration First
            Vector3 motion;

            //Calculates acceleration
            if (_velocity.magnitude < MaxSpeed)
                _velocity += LinealAcceleration * dt * movement;

            //Calculates motion
            motion = dt * _velocity;

            if (_velocity.magnitude < MinSpeedToMove)
            {
                motion = Vector3.zero;
            }

            _characterController.Move(motion);
            //_animator.SetFloat("Speed", motion.magnitude * 10);
        }

        private void OnSprint(bool isSprint)
        {
            if (CanSprint)
                _isSprinting = isSprint;
        }

        private void DescelerateUpdate()
        {
            if (!MoveByAcceleration)
                return;

            float dt = Time.fixedDeltaTime;
            if (_velocity.magnitude > 0)
                _velocity -= LinealDeceleration * dt * (_velocity.normalized);
        }

        private void GravityUpdate()
        {
            float dt = Time.fixedDeltaTime;
            _velocityY += Gravity * dt;

            Vector3 motion = new(0, _velocityY * Time.fixedDeltaTime, 0);
            CollisionFlags collisionFlags = _characterController.Move(motion);

            if (((collisionFlags & CollisionFlags.Below) != 0)                              //Checks the floor
                || ((collisionFlags & CollisionFlags.Above) != 0) && (_velocityY > 0))  //Checks the roof
            {
                _velocityY = 0;
            }
        }
        #endregion
    }
}