using UnityEngine;
using UtilsComplements;

namespace InputManagerController
{
    #region Initialize&Main
    public partial class InputManager : MonoBehaviour
    {
        #region UnityLogic
        private void Awake()
        {
            CreateSinglePlayer();
            InitializePlayerMap();
        }

        private void Update()
        {
            MovementUpdate();
        }

        private void OnDestroy()
        {
            DeleteSinglePlayer();
        }
        #endregion

        #region Possible Functionalities
        partial void CreateSinglePlayer();  //Creates a Singleton
        partial void DeleteSinglePlayer();  //Removes the Singleton
        partial void InitializePlayerMap(); //Initialize PlayerScript 

        partial void MovementUpdate();
        #endregion
    }
    #endregion

    #region New Input System Map
    public partial class InputManager : MonoBehaviour
    {
        private PlayerMap _playerInput;

        partial void InitializePlayerMap()
        {
            _playerInput = new PlayerMap();
            _playerInput.Player.Enable();
        }
    }
    #endregion

    //If want local multiplayer, delete or comment this, even though I dont know how to create a Multiplayer xdxd
    #region SinglePlayer Utils
    public partial class InputManager : ISingleton<InputManager>
    {
        public ISingleton<InputManager> Instance => this;
        public InputManager Value => this;

        partial void CreateSinglePlayer()
        {
            Instance.Instantiate();
        }

        partial void DeleteSinglePlayer()
        {
            Instance.RemoveInstance();
        }
    }
    #endregion
}