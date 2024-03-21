using UnityEngine;

namespace UtilsComplements
{
    public class Verify<T> where T : Component
    {
        public T Value;
        public bool Valid;

        public Verify(Transform evaluated)
        {
            Value = evaluated.GetComponent<T>();
            Valid = Value != null;
        }
    }
}