using UnityEngine;

namespace CharacterMovement
{
    #region Report
    //Made by DarkAlejoxD, Camilo Londoño

    //Last checked: March 2024
    //Last modification: February 2024

    ////Direct dependencies of classes if imported file by file:
    //  -   Foldout Utility (Included in Complements Folder)
    #endregion

    /// <summary>
    /// Player/Game Blackboard, can create various and test different scenarios
    /// </summary>
    [CreateAssetMenu(fileName = "New Game DataBase", menuName = "GameData/New Data Base", order = 0)]
    public partial class GameValues : ScriptableObject
    {
        private const bool DEFAULT_STYLED = true;

        #region Player Attributes
        [Foldout("Player Attributes", foldEverything = true, styled = DEFAULT_STYLED)]
        [Header("Movement")]
        [SerializeField] private float _minSpeedToMove = 2;
        [SerializeField] private float _maxSpeed = 5;
        [SerializeField] private float _sprintMaxSpeed = 10;
        [SerializeField, Tooltip("Aka Linear Drag")] private float _linealAcceleration = 20;
        [SerializeField] private float _linealDeceleration = 10;
        [SerializeField] private bool _useAcceleratedMovement = true;
        [SerializeField] private bool _canSprint = true;
        [SerializeField, Range(0, 1)] private float _rotationLerp = 0.01f;

        [Header("Movement DEBUG")]
        [SerializeField] private bool DEBUG_drawCone;        

        #region Player Properties
        public float MinSpeedToMove => _minSpeedToMove;

        /// <summary>
        /// If want to modify the base maxSpeed, you should look for the GameData
        /// ScriptableObjects that contains it.
        /// </summary>
        public float MaxSpeed
        {
            get { return _maxSpeed; }
            set { _maxSpeed = value; }
        }
        public bool SprintEnabled => _canSprint;
        public float SprintSpeed => _sprintMaxSpeed;
        public bool UseAccelerationMovement => _useAcceleratedMovement;
        /// <summary>
        /// If want to modify the base linealAcceleration, you should look for the GameData
        /// ScriptableObjects that contains it.
        /// </summary>
        public float LinealAcceleration
        {
            get { return _linealAcceleration; }
            set { _linealAcceleration = value; }
        }
        public float LinealDeceleration => _linealDeceleration;
        public float RotationLerp => _rotationLerp;
        public bool DEBUG_DrawCone => DEBUG_drawCone;        
        #endregion

        #endregion
    }
}