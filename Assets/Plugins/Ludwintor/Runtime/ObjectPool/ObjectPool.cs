using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludwintor.Tools
{
    [AddComponentMenu("Ludwintor/Object Pool")]
    public class ObjectPool : MonoBehaviour
    {
        public GameObject prefabToPool;
        [SerializeField] private int initialSize;
        [SerializeField] private bool recycleInSeparateContainer;

        private Queue<GameObject> pool;
        private GameObject inactiveContainer;


        private void Awake()
        {
            CreateInactiveContainer();
            PopulateInitialPool();
        }

        public GameObject Claim()
        {
            if (pool.Count == 0)
            {
                Create();
            }

            return pool.Dequeue();
        }

        public T Claim<T>() where T : Component
        {
            return Claim().GetComponent<T>();
        }

        public void Recycle(GameObject objectToPool)
        {
            objectToPool.SetActive(false);
            if (recycleInSeparateContainer)
                objectToPool.transform.SetParent(inactiveContainer.transform, false);

            pool.Enqueue(objectToPool);
        }

        public void Clear()
        {
            if (recycleInSeparateContainer)
            {
                foreach (Transform child in inactiveContainer.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            pool.Clear();
        }

        private void PopulateInitialPool()
        {
            pool = new Queue<GameObject>(initialSize);

            for (int i = 0; i < initialSize; i++)
            {
                Create();
            }
        }

        private void Create()
        {
            GameObject newObject = Instantiate(prefabToPool);
            Recycle(newObject);
        }

        private void CreateInactiveContainer()
        {
            if (recycleInSeparateContainer)
                inactiveContainer = new GameObject("[Object Pool] " + prefabToPool.name);
        }
    }
}