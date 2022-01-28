using System;
using UnityEngine;
using UnityEngine.Pool;

namespace MaxHelpers
{
    public abstract class PoolerBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        private ObjectPool<T> _pool;
        private T _prefab;

        private ObjectPool<T> Pool
        {
            get
            {
                if (_pool == null) throw new InvalidOperationException("You need to call InitPool before using it.");
                return _pool;
            }
            set => _pool = value;
        }

        protected void InitPool(T prefab, int initial = 10, int max = 20, bool collectionChecks = false)
        {
            _prefab = prefab;
            Pool = new ObjectPool<T>(
                CreateSetup,
                GetSetup,
                ReleaseSetup,
                DestroySetup,
                collectionChecks,
                initial,
                max);
        }

        #region Overrides

        protected virtual T CreateSetup()
        {
            return Instantiate(_prefab);
        }

        protected virtual void GetSetup(T obj)
        {
            obj.gameObject.SetActive(true);
        }

        protected virtual void ReleaseSetup(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        protected virtual void DestroySetup(T obj)
        {
            Destroy(obj);
        }

        #endregion

        #region Getters

        public T Get()
        {
            return Pool.Get();
        }

        public void Release(T obj)
        {
            Pool.Release(obj);
        }

        #endregion
    }
}