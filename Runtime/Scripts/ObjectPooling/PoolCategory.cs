// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.ObjectPooling : PoolCategory.cs
//
// All Rights Reserved

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GibFrame.ObjectPooling
{
    [Serializable]
    public class PoolCategory : IEquatable<PoolCategory>
    {
        [SerializeField] private string name;
        [SerializeField] private List<Pool> pools;
        private Dictionary<string, Queue<GameObject>> poolsDictionary = null;
        private Vector3 startPosition;

        public string Name => name;

        public PoolCategory(string name, params Pool[] pools)
        {
            this.name = name;
            this.pools = new List<Pool>();
            this.pools.AddRange(pools);
        }

        /// <summary> <summary> Initialize the pools </summary> <param name="position"> </param>
        public void Instantiate(Vector3 position)
        {
            startPosition = position;
            int length = pools.Count;
            for (int i = 0; i < length; i++)
            {
                InstantiatePool(pools[i]);
            }
            pools.NormalizeProbabilities();
        }

        public Pool GetPool(string tag)
        {
            return pools.Find((p) => p.Tag.Equals(tag));
        }

        public int AddPool(params Pool[] pools)
        {
            int count = 0;
            foreach (Pool pool in pools)
            {
                if (!ContainsPool(pool))
                {
                    this.pools.Add(pool);
                    InstantiatePool(pool);
                    count++;
                }
            }
            return count;
        }

        public void InstantiatePool(Pool pool)
        {
            poolsDictionary ??= new Dictionary<string, Queue<GameObject>>();
            if (poolsDictionary.ContainsKey(pool.Tag)) return;
            Queue<GameObject> objectPool = new Queue<GameObject>();
            int poolDim = pool.Size;
            for (int j = 0; j < poolDim; j++)
            {
                GameObject obj = UnityEngine.Object.Instantiate(pool.Prefab, startPosition, Quaternion.identity);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolsDictionary.Add(pool.Tag, objectPool);
        }

        public bool ContainsPool(string name)
        {
            return pools.FindAll((c) => c.Tag.Equals(name)).Count > 0;
        }

        public bool ContainsPool(Pool pool)
        {
            return ContainsPool(pool.Tag);
        }

        public GameObject SpawnFromPool(Vector3 position, Quaternion rotation)
        {
            return SpawnFromPool("", position, rotation);
        }

        public GameObject SpawnFromPool(string poolTag, Vector3 position, Quaternion rotation)
        {
            if (poolTag.Equals(""))
            {
                poolTag = GetRandomPoolTag();
            }
            if (poolsDictionary[poolTag] == null)
            {
                return null;
            }
            if (!poolsDictionary.ContainsKey(poolTag))
            {
                return null;
            }

            IPooledObject[] poolInterfaces;
            GameObject objectToSpawn = poolsDictionary[poolTag].Dequeue();
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            poolInterfaces = objectToSpawn.GetComponentsInChildren<IPooledObject>();
            objectToSpawn.SetActive(true);
            if (poolInterfaces != null)
            {
                int length = poolInterfaces.Length;
                for (int i = 0; i < length; i++)
                {
                    poolInterfaces[i].OnObjectSpawn();
                }
            }
            poolsDictionary[poolTag].Enqueue(objectToSpawn);
            return objectToSpawn;
        }

        /// <summary>
        /// </summary>
        /// <returns> A random pool tag based on pools probabilities </returns>
        public string GetRandomPoolTag()
        {
            pools.NormalizeProbabilities();
            return pools.SelectWithProbability().Tag;
        }

        public bool Equals(PoolCategory other)
        {
            return other.Name.Equals(Name);
        }
    }
}