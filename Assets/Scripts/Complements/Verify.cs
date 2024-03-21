using UnityEngine;

namespace UtilsComplements
{
    #region Report
    //Last checked: Not Checked (New)
    //Last modification: March 2024

    //Commentaries:
    //  -   Meant to easy check if a class contains another class to prevent errors.
    //  -   An example could be in PlayerMovement, check if has an Animator... etc
    #endregion

    /// <summary>
    /// Verifies if the object has a component.
    /// Use it at Start() and Get "Valid" after that
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Verify<T> where T : Component
    {
        public T Value;
        public bool Valid;

        public Verify(Transform evaluated, bool checkInChildren = false)
        {
            Value = evaluated.GetComponent<T>();
            
            if(Value == null && checkInChildren)
                Value = evaluated.GetComponentInChildren<T>();

            Valid = Value != null;
        }

        //public Verify(Component evaluated)
        //{
        //    Value = evaluated.GetComponent<T>();
        //    Valid = Value != null;
        //}

        //public Verify(GameObject evaluated)
        //{
        //    Value = evaluated.GetComponent<T>();
        //    Valid = Value != null;
        //}
    }
}