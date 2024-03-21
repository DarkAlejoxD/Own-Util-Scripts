using UnityEngine;
using UtilsComplements;

namespace CharacterMovement
{
    #region Report
    //Made by DarkAlejoxD, Camilo Londoño
    //Last checked: March 2024
    //Last modification: March 2024            
    #endregion

    /// <summary>
    /// Calculates the rotation from the movement direction
    /// </summary>
    [RequireComponent(typeof(PlayerMovement), typeof(DataContainer))]
    public class PlayerRotation : MonoBehaviour
    {
        [Header("References")]
        private DataContainer _dataContainer;
        private PlayerMovement _playerMovement;

        private GameValues GameData => _dataContainer.GameData;
        private Vector3 Velocity => _playerMovement.Velocity;
        private float RotationLerp => GameData.RotationLerp;

        #region Unity Logic
        private void Awake()
        {
            _dataContainer = GetComponent<DataContainer>();
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void FixedUpdate()
        {
            Rotation();
        }
        #endregion

        #region Private Methods
        private void Rotation()
        {
            if (Velocity.magnitude > GameData.MinSpeedToMove)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(Velocity.normalized);
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, RotationLerp);
            }
        }
        #endregion

        #region DEBUG
#if UNITY_EDITOR
        private Transform DEBUG_surrogate;
        private const float DEBUG_height = 1.5f;

        private void OnDrawGizmos()
        {
            DEBUG_DrawCone();
        }

        private void DEBUG_DrawCone()
        {
            if (_dataContainer != null)
            {
                if (!DEBUG_surrogate)
                {
                    DEBUG_surrogate = new GameObject("DEBUG_eyesPos").transform;
                    DEBUG_surrogate.position = transform.position + Vector3.up * DEBUG_height;
                    DEBUG_surrogate.rotation = transform.rotation;
                    DEBUG_surrogate.SetParent(transform);
                }
                GizmosUtilities.DrawFOVCone(DEBUG_surrogate, Color.green, GameData.DEBUG_DrawCone);
            }
        }
#endif
        #endregion
    }
}