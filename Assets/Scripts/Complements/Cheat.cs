using UnityEngine;

namespace UtilsComplements
{
    #region Report
    //Made by DarkAlejoxD, Camilo Londoño

    //Last checked: March 2024
    //Last modification: March 2024
    #endregion

    /// <summary> Allows create GTA San Andreas like cheats </summary>
    public abstract class Cheat : MonoBehaviour
    {
        protected bool _cheatActivated;

        private int _cheatIndex = 0;
        
        protected abstract string CHEAT_NAME { get; }
        protected virtual bool DEBUG_debug => false;

        protected virtual void Awake()
        {
            _cheatActivated = false;
        }

        protected virtual void Update()
        {
            if (_cheatActivated)
                return;

            if (!Input.anyKeyDown)
                return;

            string cheat = CHEAT_NAME;
            char correctKey = cheat[_cheatIndex];
            if (DEBUG_debug) Debug.Log(correctKey.ToString().ToLower());

            if (Input.GetKeyDown(correctKey.ToString().ToLower()))
            {
                if (_cheatIndex == CHEAT_NAME.Length - 1)
                {
                    OnCheat();
                    _cheatActivated = true;
                }
                _cheatIndex++;

                if (DEBUG_debug) Debug.Log("Correct key");
            }
            else { _cheatIndex = 0; }
        }

        protected abstract void OnCheat();
    }
}