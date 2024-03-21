using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UtilsComplements
{
    #region Report
    //Made by DarkAlejoxD, Camilo Londoño
    //Last checked: November 2023
    //Last modification: idk, probably September or smtg

    //Commentaries:
    //  -   Unknown how affects to performance or memory due to working with a pair of lists

    //TODO: Profile this class and fix if any perfromance error.
    //TODO: Check if pool works normally.
    #endregion

    /// <summary>
    /// Pool of any Component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pool<T> where T : Component
    {
        private readonly bool _catchObjectFromActiveList;

        private readonly int _defaultObjects;
        private readonly int _maxObjects;

        private readonly GameObject _prefab;
        private readonly Transform _folder;

        private readonly List<T> _releasedObjects;
        private readonly List<T> _activeObjects;

        private readonly Transform _parent;

        public Transform PoolParent => _folder;

        public Pool(GameObject prefab, int defaultObjects = 10, int maxObjects = 30,
                    string nameOfFolder = "NewPool", bool catchObjectFromActiveList = false,
                    Transform parent = null)
        {
            _prefab = prefab;

            _defaultObjects = defaultObjects;
            _maxObjects = Mathf.Max(defaultObjects, maxObjects);

            _releasedObjects = new List<T>();
            _activeObjects = new List<T>();

            _folder = new GameObject(nameOfFolder).transform;

            if (parent != null)
            {
                _folder.SetParent(parent);
                _parent = parent;

            }

            CreateNewInstances();
            _catchObjectFromActiveList = catchObjectFromActiveList;
        }

        public static Pool<T> ClonPool(Pool<T> instance)
        {
            if (instance != null)
            {
                instance.DestroyPool();
            }
            return new Pool<T>(instance._prefab, instance._defaultObjects, instance._maxObjects,
                               instance._folder.name, instance._catchObjectFromActiveList, instance._parent);
        }

        public T GetNewObject()
        {
            if (_releasedObjects.Count <= 0)
            {
                CreateNewInstances();
            }

            if (_activeObjects.Count >= _maxObjects)
            {
                if (_catchObjectFromActiveList)
                {
                    T newObj = _activeObjects.First();
                    _activeObjects.Remove(newObj);
                    newObj.gameObject.SetActive(false);
                    newObj.gameObject.SetActive(true);
                    _activeObjects.Add(newObj);
                    return newObj;
                }
                else
                {
                    Debug.Log("Max Capacity on loop reached, consider expand the capacity of the pool");
                    return null;
                }
            }

            T obj = _releasedObjects.First();
            _releasedObjects.Remove(obj);
            _activeObjects.Add(obj);
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void ReleaseObject(T obj)
        {
            if (!_activeObjects.Contains(obj))
            {
                Debug.LogError("Try to release an object that is not part of the pool");
                return;
            }
            _activeObjects.Remove(obj);
            _releasedObjects.Add(obj);
            obj.gameObject.SetActive(false);
        }

        private void DestroyPool()
        {
            foreach (T item in _activeObjects)
            {
                if (item != null)
                    GameObject.Destroy(item.gameObject);
            }
            foreach (T item in _releasedObjects)
            {
                if (item != null)
                    GameObject.Destroy(item.gameObject);
            }

            _activeObjects.Clear();
            _releasedObjects.Clear();
        }

        private void CreateNewInstances()
        {
            int capacity = _defaultObjects;
            int spaceUsed = _releasedObjects.Count + _activeObjects.Count;

            if (spaceUsed > _maxObjects)
            {
                int difference = spaceUsed - _maxObjects;
                DeleteInstances(difference);
                return;
            }

            if (spaceUsed == _maxObjects)
            {
                capacity = 0;
            }
            else if (spaceUsed < _maxObjects)
            {
                int difference = _maxObjects - spaceUsed;
                capacity = Mathf.Min(difference, _defaultObjects);
            }

            for (int i = 0; i < capacity; i++)
            {
                var obj = GameObject.Instantiate(_prefab);

                if (!obj.TryGetComponent(out T comp))
                {
                    Debug.Log("The Prefab has no " + comp.GetType() + " , check it", obj);
                }
                else
                {
                    comp.gameObject.SetActive(false);
                    obj.transform.SetParent(_folder);
                    _releasedObjects.Add(comp);
                }
            }
        }

        private void DeleteInstances(int number)
        {
            for (int i = 0; i < number; i++)
            {
                var a = _releasedObjects.First();
                a.gameObject.SetActive(false);
                _releasedObjects.Remove(a);
                GameObject.Destroy(a.gameObject);
            }
        }
    }

    //public class ObjPool
    //{
    //    List<GameObject> _gameObjectList;
    //    int _index;

    //    public ObjPool(int elementCount, GameObject prefab, Transform parent)
    //    {
    //        _gameObjectList = new(elementCount);
    //        _index = 0;
    //    }

    //    public GameObject GetNewObject()
    //    {
    //        return _gameObjectList[_index];
    //    }


    //}
}