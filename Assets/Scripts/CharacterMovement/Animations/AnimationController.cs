using System.Collections.Generic;
using UnityEngine;
using UtilsComplements;

namespace CharacterMovement.Animations
{
    [RequireComponent(typeof(PlayerMovement))]
    public partial class AnimationController : MonoBehaviour
    {
        //Change when you have an animator and a blend tree for movement
        private const string ANIMATOR_SPEED_NAME = "Speed";
        private const float ANIMATOR_WALK_VALUE = 0.5f;

        /// <summary>
        /// Int: Number of variation
        /// float: Value in the BlendTree of the Variation
        /// It's just a test, can be deleted
        /// </summary>
        private static Dictionary<int, float> _indexValuePair;

        [Header("References")]
        private PlayerMovement _playerMovement;
        private Verify<Animator> _verifyAnimator;
        private DataContainer _dataContainer;

        [Header("Dynamimc Attributes")]
        [SerializeField] private float _timeToChangeAnimations = 5;
        private int _animIndex;
        private float _currentValue;

        private GameValues GameData => _dataContainer.GameData;
        private float MinSpeedToMove => GameData.MinSpeedToMove;
        private float MaxWalkSpeed => GameData.MaxSpeed;
        private float MaxSprintSpeed => GameData.SprintSpeed;

        static AnimationController()
        {
            _indexValuePair = new Dictionary<int, float>();
            _indexValuePair.Add(1, -1);
            _indexValuePair.Add(2, 0);
            _indexValuePair.Add(3, 1);
        }

        private void Awake()
        {
            _verifyAnimator = new Verify<Animator>(this.transform, true);
            _playerMovement = GetComponent<PlayerMovement>();
            _dataContainer = GetComponent<DataContainer>();
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

            if(actualSpeed >= threshold05)
            {
                float max = threshold1 - threshold05;
                float current = actualSpeed - threshold05;

                animValue = Mathf.Lerp(ANIMATOR_WALK_VALUE, 1, Mathf.Clamp01(current / max));
            }
            else
            {
                float max = threshold05 - threshold0;
                float current = actualSpeed;

                animValue = Mathf.Lerp(0, ANIMATOR_WALK_VALUE, Mathf.Clamp01(current/max));
            }

            animator.SetFloat(ANIMATOR_SPEED_NAME, Mathf.Clamp01(animValue));
        }
    }
}