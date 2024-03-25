using UnityEngine;

namespace CharacterMovement
{
    public partial class GameValues : ScriptableObject
    {
        [Foldout("Player Auto Attributes (PointnClick)", styled = DEFAULT_STYLED)]
        [Header("Automatic Movement")]
        [SerializeField] private float _radiusToStop = 2;
        [SerializeField] private float _radiusToSlowDown = 5;

        [Header("PointnClick DEBUG")]
        [SerializeField] private bool DEBUG_drawNextLocation;
        [SerializeField] private bool DEBUG_drawSlowDownDistance;
        [SerializeField] private bool DEBUG_drawStopDistance;


        public float RadiusToStop => _radiusToStop;
        public float RadiusToSlowDown => _radiusToSlowDown;

        public bool DEBUG_DrawNextLocation => DEBUG_drawNextLocation;
        public bool DEBUG_DrawSlowDown => DEBUG_drawSlowDownDistance;
        public bool DEBUG_DrawStop => DEBUG_drawStopDistance;
    }
}