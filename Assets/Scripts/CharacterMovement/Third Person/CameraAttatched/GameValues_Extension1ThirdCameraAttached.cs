using UnityEngine;

namespace CharacterMovement
{
    public partial class GameValues : ScriptableObject
    {
        [Foldout("Third Person Camera Attached Attribues", styled = true)]
        [Header("Attributes (In Degrees)")]
        [SerializeField] private float _minPitch = -40;
        [SerializeField] private float _maxPitch = 70;
        [SerializeField] private bool _pitchInversed = true;

        [Header("Attributes PC")]
        [SerializeField] private float _pcYawSpeed = 20;
        [SerializeField] private float _pcPitchSpeed = 20;

        [Header("Attributes Gamepad")]
        [SerializeField] private float _gamePadYawSpeed = 360;
        [SerializeField] private float _gamePadPitchSpeed = 360;

        public float MinPitch => _minPitch;
        public float MaxPitch => _maxPitch;
        public bool PitchInversed => _pitchInversed;


        public float PcYawSpeed => _pcYawSpeed;
        public float PcPitchSpeed => _pcPitchSpeed;

        public float GamePadPitchSpeed => _gamePadPitchSpeed;
        public float GamePadYawSpeed => _gamePadYawSpeed;
    }
}
