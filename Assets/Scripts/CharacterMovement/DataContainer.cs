using UnityEngine;

namespace CharacterMovement
{
    #region Report
    //Last checked: March 2024
    //Last modification: February 2024
    #endregion

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