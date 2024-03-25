using System;
using UnityEngine;
using UtilsComplements;

namespace CharacterMovement.Animations
{
    #region Report
    //Made by DarkAlejoxD, Camilo Londoño

    //Partial Role: Main
    //Current State: PartialEnd
    //Last checked: March 2024
    //Last modification: March 2024

    //Direct dependencies of classes if imported file by file:
    //  -   Base3DMovement Folder
    //  -   UtilsComponents.Verify<T>.cs

    //Commentaries:
    //  -   It's parcial to try make it modular and plug & play.
    //      -   Basically, if your player Jumps, can have a partial version of this class destined to the JumpLogic.
    //          If not, just don't import the jump animations and the class will remain the same (Testing).
    #endregion
    [RequireComponent(typeof(PlayerMovement))]
    public partial class AnimationController : MonoBehaviour
    {
        //Change when you have an animator and a blend tree for movement
        private const string ANIMATOR_SPEED_NAME = "Speed";
        private const string ANIMATOR_RANDOM_NAME = "Random";
        private const float ANIMATOR_WALK_VALUE = 0.5f;

        /// <summary>
        /// Int: Number of variation
        /// float: Value in the BlendTree of the Variation
        /// It's just a test, can be deleted
        /// </summary>
        private static float[] _differentAnimValues =
        {
            1, 2, 3
        };

        [Header("References")]
        private PlayerMovement _playerMovement;
        private Verify<Animator> _verifyAnimator;
        private DataContainer _dataContainer;

        [Header("Dynamimc Attributes")]
        [SerializeField] private float _minTimeToChangeAnimations = 5;
        [SerializeField] private float _maxTimeToChangeAnimations = 5;
        private float _timeToChangeAnim;
        [SerializeField, Range(0.001f, 1)] private float _changeAnimSmooth = 0.2f;
        private float _changeTimerControl;
        private int _animIndex;
        private float _currentValue;

        private GameValues GameData => _dataContainer.GameData;
        private float MinSpeedToMove => GameData.MinSpeedToMove;
        private float MaxWalkSpeed => GameData.MaxSpeed;
        private float MaxSprintSpeed => GameData.SprintSpeed;

        private void Awake()
        {
            _verifyAnimator = new Verify<Animator>(this.transform, true);
            _playerMovement = GetComponent<PlayerMovement>();
            _dataContainer = GetComponent<DataContainer>();

            _timeToChangeAnim = UnityEngine.Random.Range(_minTimeToChangeAnimations, _maxTimeToChangeAnimations);
            _changeTimerControl = 0;
            _animIndex = 0;
            _currentValue = _differentAnimValues[_animIndex];
        }

        private void FixedUpdate()
        {
            if (!_verifyAnimator.Valid)
                return;

            UpdateTimer();      //Comment this if don't have multiple animations
            UpdateAnimations(); //Comment this if don't have multiple animations
        }        

        private void Update()
        {
            if (!_verifyAnimator.Valid)
                return;

            UpdateSpeedValue();
        }

        private void UpdateSpeedValue()
        {
            Animator animator = _verifyAnimator.Value;
            float actualSpeed = _playerMovement.Velocity.magnitude;

            float animValue = 0;

            if (actualSpeed < MinSpeedToMove)
            {
                animator.SetFloat(ANIMATOR_SPEED_NAME, animValue);
                return;
            }

            float threshold0 = MinSpeedToMove;  //Should be the speed when the player begins motion.
            float threshold05 = MaxWalkSpeed;   //Should be the max Speed when walks.
            float threshold1 = MaxSprintSpeed;   //Should be the max Speed when runs.

            if (actualSpeed >= threshold05)
            {
                float max = threshold1 - threshold05;
                float current = actualSpeed - threshold05;

                animValue = Mathf.Lerp(ANIMATOR_WALK_VALUE, 1, Mathf.Clamp01(current / max));
            }
            else
            {
                float max = threshold05 - threshold0;
                float current = actualSpeed;

                animValue = Mathf.Lerp(0, ANIMATOR_WALK_VALUE, Mathf.Clamp01(current / max));
            }

            animator.SetFloat(ANIMATOR_SPEED_NAME, Mathf.Clamp01(animValue));
        }

        private void UpdateTimer()
        {
            float actualSpeed = _playerMovement.Velocity.magnitude;

            if (actualSpeed > MinSpeedToMove)
            {
                _changeTimerControl = 0;
                return;
            }

            _changeTimerControl += Time.fixedDeltaTime;

            if (_changeTimerControl > _timeToChangeAnim)
            {
                _changeTimerControl = 0;
                ChangeAnimation();
            }
        }

        private void UpdateAnimations()
        {
            Animator animator = _verifyAnimator.Value;
            float actualValue = animator.GetFloat(ANIMATOR_RANDOM_NAME);
            float nextValue = Mathf.Lerp(actualValue, _currentValue, _changeAnimSmooth);
            _timeToChangeAnim = UnityEngine.Random.Range(_minTimeToChangeAnimations, _maxTimeToChangeAnimations);

            animator.SetFloat(ANIMATOR_RANDOM_NAME, nextValue);
        }

        private void ChangeAnimation()
        {
            int max = _differentAnimValues.Length;
            int value = UnityEngine.Random.Range(0, max);
            _currentValue = _differentAnimValues[value];
            Debug.Log("ChangeAnimattions");
        }
    }
}