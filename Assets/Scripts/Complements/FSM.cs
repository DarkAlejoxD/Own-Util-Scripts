using System;
using System.Collections.Generic;

namespace UtilsComplements
{
    public class State
    {
        public Action OnEnter;

        public Action OnStay;

        public Action OnExit;
    }

    #region Report
    //Last checked: March 2024
    //Last modification: idk, maybe 2022
    #endregion

    /// <summary>
    /// Easy Way to make Finite State Machine by enums
    /// </summary>
    public class FSM<T> where T : Enum
    {
        public T CurrentState => ActualState;

        protected T ActualState;

        protected Dictionary<T, State> States;

        public FSM(T initState)
        {
            States = new Dictionary<T, State>();

            foreach (T estate in Enum.GetValues(typeof(T)))
            {
                States.Add(estate, new State());
            }

            ActualState = initState;
        }

        public void Update()
        {
            States[ActualState].OnStay?.Invoke();
        }

        public void ChangeState(T newState)
        {
            if (newState == null)
                return;

            States[ActualState].OnExit?.Invoke();
            States[newState].OnEnter?.Invoke();

            ActualState = newState;
        }

        public void SetOnStay(T state, Action f)
        {
            States[state].OnStay = f;
        }

        public void SetOnEnter(T state, Action f)
        {
            States[state].OnEnter = f;
        }

        public void SetOnExit(T state, Action f)
        {
            States[state].OnExit = f;
        }
    }

    #region Report
    //Last checked: March 2024
    //Last modification: idk, maybe 2023
    #endregion

    /// <summary>
    /// Addition to an old Script to
    /// Make FSM as Observer Pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FSM_Extended<T> : FSM<T> where T : Enum
    {
        public FSM_Extended(T initState) : base(initState)
        {
        }        

        public void AddOnStay(T state, Action f)
        {
            States[state].OnStay += f;
        }

        public void AddOnEnter(T state, Action f)
        {
            States[state].OnEnter += f;
        }

        public void AddOnExit(T state, Action f)
        {
            States[state].OnExit += f;
        }


        public void SubstractOnStay(T state, Action f)
        {
            States[state].OnStay -= f;
        }

        public void SubstractOnEnter(T state, Action f)
        {
            States[state].OnEnter -= f;
        }

        public void SubstractOnExit(T state, Action f)
        {
            States[state].OnExit -= f;
        }
    }
}