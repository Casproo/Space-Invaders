using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class MissilePool : MonoBehaviour
    {
        public static MissilePool Instance;

        private List<GameObject> _pooledObjects = new List<GameObject>();
        private int _amountToPool = 2;

        [SerializeField] private GameObject missilePrefab;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            for (int i = 0; i < _amountToPool; i++)
            {
                GameObject gameObject = Instantiate(missilePrefab);
                gameObject.SetActive(false);
                _pooledObjects.Add(gameObject);
            }
        }

        public GameObject GetPooledGameObject()
        {
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].activeInHierarchy)
                {
                    return _pooledObjects[i];
                }
            }

            return null;
        }
    }
}