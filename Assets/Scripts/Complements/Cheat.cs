using UnityEngine;

namespace UtilsComplements
{
    /// <summary> Allows create GTA San Andreas like cheats </summary>
    public abstract class Cheat : MonoBehaviour
    {
        private int _cheatIndex = 0;
        protected abstract string CHEAT_NAME { get; }
        protected bool _cheatActivated;
        protected virtual bool DEBUG_debug => false;

        protected virtual void Awake()
        {
            _cheatActivated = false;
        }

        protected virtual void Update()
        {
            if (_cheatActivated)
                return;

            if (Input.anyKeyDown)
            {
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
                    if (DEBUG_debug) Debug.Log("Correct key");
                    _cheatIndex++;
                }
                else
                {
                    _cheatIndex = 0;
                }
            }
        }

        protected abstract void OnCheat();
    }
}