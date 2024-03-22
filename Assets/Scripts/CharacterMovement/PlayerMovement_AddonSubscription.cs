using UnityEngine;

namespace CharacterMovement
{
    #region Report
    //Made by DarkAlejoxD, Camilo Londoño

    //Partial Role: Branch/Addon
    //Current State: WIP
    //Last checked: March 2024
    //Last modification: March 2024

    //Direct dependencies of classes if imported file by file:
    //  -   PlayerMovement.cs

    //Commentaries:
    //  -   Subscribe to the Input Manager if Any
    #endregion

    //This part of the class' task is to subscribe to the InputManager
    public partial class PlayerMovement : MonoBehaviour
    {
        partial void OnEnable() //Prepared to Observer Pattern and an extern InputManager Script
        {
            CharacterInputManager.OnMove += OnMove;
            CharacterInputManager.OnSprint += OnSprint;
        }
        partial void OnDisable() //Prepared to Observer Pattern and an extern InputManager Script
        {
            CharacterInputManager.OnMove -= OnMove;
            CharacterInputManager.OnSprint -= OnSprint;
        }
    }
}