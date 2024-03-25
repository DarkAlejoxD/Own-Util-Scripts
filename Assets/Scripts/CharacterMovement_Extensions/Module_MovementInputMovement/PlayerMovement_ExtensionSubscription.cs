using UnityEngine;
using InputManagerController;

namespace CharacterMovement
{
    #region Report
    //Made by DarkAlejoxD, Camilo Londoño

    //Partial Role: Branch/Extension
    //Current State: PartialEnd
    //Last checked: March 2024
    //Last modification: March 2024

    //Direct dependencies of classes if imported file by file:
    //  -   PlayerMovement.cs
    //  -   InputManager.cs
    //      -   It's Optional because you can create a new InputManager from scratch and rewrite
    //          OnEnable/OnDisable functions

    //Commentaries:
    //  -   Subscribe to the Input Manager
    #endregion

    //This part of the class' task is to subscribe to the InputManager
    [RequireComponent(typeof(InputManager))]
    public partial class PlayerMovement : MonoBehaviour
    {
        partial void OnSubscribeInputManager() //Prepared to Observer Pattern and an extern InputManager Script
        {
            InputManager inputManager = GetComponent<InputManager>();

            inputManager.OnMove += OnMove;
            inputManager.OnSprint += OnSprint;
        }
        partial void OnUnsubscribeInputManager() //Prepared to Observer Pattern and an extern InputManager Script
        {
            InputManager inputManager = GetComponent<InputManager>();

            inputManager.OnMove -= OnMove;
            inputManager.OnSprint -= OnSprint;
        }
    }
}