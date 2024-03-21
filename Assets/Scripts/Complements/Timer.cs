using System;
using UnityEngine;

namespace UtilsComplements
{
    #region Report
    //Last checked: idk
    //Last modification: idk

    //Commentaries:
    //  -   Used to easy create timers in games. Is useful to implement a timer when you need to
    //      control the _currentTime.
    //  -   It's kinda messy when you need to initialize the class before using it and put the 
    //      Update(dt) method in an Update() Monobehaviour class.
    //  -   If some extern is reading this, i prefer to use coroutines if I want a puntual timer in
    //      my games, and implement myself a timer if a need it, but you can use it anyway, that's 
    //      why it is in this namespace, maybe it's useful for some spawner if you activate loop.
    #endregion

    /// <summary>
    /// Easy create timers
    /// </summary>
    public class Timer
    {
        private const int DEFAULT_TIME_TO_DIE = 5;

        private float _duration;
        private float _currentTime = 0;

        private bool _loop = false;
        private bool _hasFinished = false;

        public Action OnTime;

        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }

        public bool Loop
        {
            get => _loop;
            set => _loop = value;
        }

        public float CurrentTime => _currentTime;
        public float CurrentTimeFraction => _currentTime / _duration;
        public bool HasFinished => _hasFinished;   

        public float TimeLeft => _duration - _currentTime;

        public int MinutesLeft => Mathf.FloorToInt(TimeLeft / 60);
        public int SecondsLeft => Mathf.FloorToInt(TimeLeft - (MinutesLeft * 60));

        public Timer()
        {
            _duration = DEFAULT_TIME_TO_DIE;
        }

        public Timer(float duration)
        {
            _duration = duration;
        }

        public Timer(float duration, Action action)
        {
            _duration = duration;
            OnTime = action;
        }

        public Timer(float duration, Action action, bool loop)
        {
            _duration = duration;
            OnTime = action;
            _loop = loop;
        }


        public void Update(float dt)
        {
            if (!_hasFinished)
            {
                _currentTime += dt;

                if (_currentTime >= _duration)
                {
                    if (OnTime != null)
                        OnTime?.Invoke();

                    if (_loop)
                        ResetTimer();
                    else
                        _hasFinished = true;
                }
            }
        }

        public void End()
        {
            if (OnTime != null)
                OnTime?.Invoke();

            if (_loop)
                ResetTimer();
            else
                _hasFinished = true;
        }

        public void ResetTimer()
        {
            _hasFinished = false;
            _currentTime = 0.0f;
        }

        public void ResetTimer(float newDuration)
        {
            _hasFinished = false;
            _currentTime = 0.0f;
            _duration = newDuration;
        }
    }
}