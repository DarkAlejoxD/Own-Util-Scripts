using UnityEngine;

namespace UtilsComplements
{
    //DarkAlejoxD
    /// <summary>
    /// Testing imlemented and static methods inside an interface
    /// It gives the functionality of a Singleton
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISingleton<T> where T : class, ISingleton<T>
    {
        #region Singleton Values
        protected static T _Singleton; //The singleton instance
        public ISingleton<T> Instance { get; } //The interface instance, to have acces to non-static implemented methods
        public T Value { get; } //The instance of the class
        #endregion

        //#region Delegates
        //public delegate T InstanceDelegate();
        //public static InstanceDelegate IfInstanceNotExist => default;
        //#endregion

        #region Static Methods
        //Call them from ISingleton<T>.

        public static bool Exists()
        {
            if (_Singleton == null || _Singleton == default)
                return false;

            return true;
        }

        public static bool TryGetInstance(out T instance)
        {
            if (_Singleton != null)
            {
                instance = _Singleton;
                return true;
            }
            else
            {
                Debug.LogError("Instance not found");
                instance = default;
                return false;
            }
        }

        public static T GetInstance()
        {
            if (_Singleton != null)
            {
                return _Singleton;
            }
            else
            {
                string format = string.Format("Instance of type {0} not found", typeof(T).ToString());
                Debug.Log(format);
                return default;//IfInstanceNotExist();
            }
        }

        public static void DEBUG_PrintInstance()
        {
            string text = string.Format("[{0}, {1}]",
                typeof(T).ToString(), GetInstance().ToString());
            Debug.Log(text);
        }
        #endregion

        #region Instance Methods
        /// <summary>
        /// Assign the instance as a Singleton
        /// Use it in Constructor() or Awake()
        /// </summary>
        public void Instantiate()
        {
            if (_Singleton == null)
                _Singleton = Value;

            else
                Invalidate();
        }

        /// <summary>
        /// The action the interface should make if already exist an instance as a Singleton
        /// Normally: Destroy(this.gameobject)
        /// </summary>
        public void Invalidate();

        //void OnDestroy();

        /// <summary>
        /// Deletes the singleton instance
        /// It should be in OnDestroy()
        /// </summary>
        public void RemoveInstance()
        {
            if (_Singleton.Equals(Value))
            {
                _Singleton = null;
            }
        }
        #endregion
    }
}