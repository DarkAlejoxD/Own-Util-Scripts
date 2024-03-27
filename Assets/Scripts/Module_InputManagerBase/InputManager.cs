using UnityEngine;
using UtilsComplements;

namespace InputManagerController
{
    #region Report
    //Made by DarkAlejoxD, Camilo Londoño

    //Partial Role: Main
    //Current State: WIP
    //Last checked: March 2024
    //Last modification: March 2024

    //Direct dependencies of classes if imported file by file:
    //  -   UtilsComplements Folder -> UtilsComplements.ISIngleton<T>.cs
    //      -   If don't want, delete SinglePlayer Region.
    //  -   New Input System Package
    //      -   If don't want, delete New Input System Map and rewrite the logic of the class and change
    //          the input style of some Branches.
    #endregion


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
            ThirdCameraAttatchedUpdate();
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
        partial void ThirdCameraAttatchedUpdate();
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
            SetPlayerMapActive(true);
            SetThirdCameraActive(true);
        }

        public void SetPlayerMapActive(bool state)
        {
            if (state) _playerInput.Player.Enable();
            else _playerInput.Player.Disable();
        }

        public void SetThirdCameraActive(bool state)
        {
            if (state) _playerInput.ThirdCamera.Enable();
            else _playerInput.ThirdCamera.Disable();
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