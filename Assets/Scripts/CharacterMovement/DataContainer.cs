using UnityEngine;

namespace CharacterMovement
{
    /// <summary>
    /// The component that will contain the data of the game.
    /// aka PlayerBlackboard.
    /// </summary>
    public class DataContainer : MonoBehaviour
    {
        [SerializeField] private GameValues _gameData;

        public GameValues GameData => _gameData;

        public void SetNewData(GameValues data)
        {
            _gameData = data;
        }
    }
}