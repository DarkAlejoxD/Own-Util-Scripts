using System;
using UnityEngine;

namespace UtilsComplements
{
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

            //if (_loop)
            //    ResetTimer();
            //else
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