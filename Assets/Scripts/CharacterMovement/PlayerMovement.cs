using UnityEngine;
using UtilsComplements;

namespace CharacterMovement
{
    #region Report
    //Made by DarkAlejoxD, Camilo Londoño

    //Partial Role: Main
    //Current State: PartialEnd
    //Last checked: March 2024
    //Last modification: March 2024

    //Direct dependencies of classes if imported file by file:
    //  -   GameValues.cs
    //  -   DataContainer.cs
    //  -   UtilsComplements.Verify<T>.cs
    //  -   (Optional) PlayerMovement_ExtensionSubscription.cs 
    //      -   It's not a dependency, but helps to declare a subscription to an InputManager.

    //Commentaries:
    //  -   Consider make an alternative script for Animations if there are a lot of animations
    //      to control.???
    //  -   This scripts do not implement input-read. You should implement by yourself.
    //      -   Consider using Observer Pattern where you use the private methods OnMove(Vector2) and OnSprint(bool).
    //      -   Also consider doing it in another file to mantain this file only for the logic of the movement.
    //          (Partial)
    //          -   You can use PlayerMovement_ExtensionSubscription.cs
    //  -   If wanna calculte the direction independent from the camera, calculate it by yourself and pass
    //      the final direction through MoveNormalDT(direction) or MoveFixedDT(direction)
    //  -   Partial Modifier is a test, it should not give any problems because when the program is compiled, it
    //      will mix all the parts of the class.
    #endregion

    /// <summary>
    /// Enough movement base in a 3D World, independent of the type of the Camera
    /// </summary>
    [SelectionBase]
    [RequireComponent(typeof(CharacterController), typeof(DataContainer))]
    public partial class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        private DataContainer _dataContainer;
        private CharacterController _characterController;

        [Header("Attributes")]
        private Vector3 _velocity = Vector3.zero;
        private float _velocityY;

        #region Attributes Getters
        public Transform CurrentCamera => Camera.main.transform;
        private GameValues GameData => _dataContainer.GameData;

        public bool IsSprinting { get; private set; }
        public Vector3 Velocity => _velocity;
        private float MinSpeedToMove => GameData.MinSpeedToMove;
        private float MaxSpeed => IsSprinting ? GameData.SprintSpeed : GameData.MaxSpeed;
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
        }

        //Meant to be in other script, but don't delete, or edit by your own risk fixing all the issues
        //Check the PlayerMovement_AddonSubscription.cs
        #region Observer/Subscriber
        partial void OnSubscribeInputManager();
        partial void OnUnsubscribeInputManager();
        #endregion

        private void OnEnable()
        {
            OnSubscribeInputManager();
        }

        private void OnDisable()
        {
            OnUnsubscribeInputManager();
        }

        private void Start()
        {
            _velocityY = 0;
        }

        private void FixedUpdate()
        {
            GravityUpdate(); //Comment this if another script controls gravity
            DescelerateUpdate();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Moves player toward a direction.
        /// </summary>
        /// <param name="motion"> the direction the player should move.</param>
        public void MoveFixedDT(Vector3 motion)
        {
            float dt = Time.fixedDeltaTime;
            if (MoveByAcceleration)
                LinearAcceleratedMovement(motion, dt);
            else
                LinearMovement(motion, dt);
        }

        /// <summary>
        /// Moves player toward a direction.
        /// </summary>
        /// <param name="motion"> the direction the player should move.</param>
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

        public void SetSprintValue(bool value)
        {
            OnSprint(value);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// All-in-One method that calculates the relative direction of the Main Camera and executes
        /// motion.
        /// </summary>
        /// <param name="input"></param>
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

            MoveNormalDT(movement);
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
                IsSprinting = isSprint;
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