// Copyright (c) 2020 Matteo Beltrame

using GibFrame.Patterns;
using System;
using UnityEngine;

namespace GibFrame.ObjectPooling
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        [SerializeField] private PoolCategory[] poolsCategory = null;

        /// <summary>
        ///   Spawn from the specified category and pool
        /// </summary>
        /// <param name="categoryName"> </param>
        /// <param name="poolTag"> </param>
        /// <param name="position"> </param>
        /// <param name="rotation"> </param>
        /// <returns> The spawned instance </returns>
        public GameObject Spawn(string categoryName, string poolTag, Vector3 position, Quaternion rotation)
        {
            PoolCategory poolCategory = Array.Find(poolsCategory, category => category.name == categoryName);
            if (poolCategory != null)
            {
                return poolCategory.SpawnFromPool(poolTag, position, rotation);
            }
            return null;
        }

        /// <summary>
        ///   Spawn from a category from a random pool selected based on its probability
        /// </summary>
        /// <param name="categoryName"> </param>
        /// <param name="position"> </param>
        /// <param name="rotation"> </param>
        /// <returns> The spawned instance </returns>
        public GameObject Spawn(string categoryName, Vector3 position, Quaternion rotation)
        {
            PoolCategory poolCategory = Array.Find(poolsCategory, category => category.name == categoryName);
            if (poolCategory != null)
            {
                return poolCategory.SpawnFromPool(null, position, rotation);
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="categoryName"> </param>
        /// <returns> A random pool tag inside a specified category </returns>
        public string GetRandomCategoryPoolTag(string categoryName)
        {
            PoolCategory poolCategory = Array.Find(poolsCategory, category => category.name == categoryName);
            if (poolCategory != null)
            {
                return poolCategory.GetRandomPoolTag();
            }
            return null;
        }

        /// <summary>
        ///   Deactivate a game object
        /// </summary>
        /// <param name="objectToDeactivate"> </param>
        public void DeactivateObject(GameObject objectToDeactivate)
        {
            objectToDeactivate.SetActive(false);
        }

        /// <summary>
        /// </summary>
        /// <param name="categoryName"> </param>
        /// <param name="poolTag"> </param>
        /// <returns> The prefab instance </returns>
        public GameObject GetPrefab(string categoryName, string poolTag)
        {
            PoolCategory poolCategory = Array.Find(poolsCategory, category => category.name == categoryName);
            Pool pool = Array.Find(poolCategory.pools, p => p.tag == poolTag);
            return pool?.prefab;
        }

        protected override void Awake()
        {
            base.Awake();
            int length = poolsCategory.Length;
            for (int i = 0; i < length; i++)
            {
                poolsCategory[i].InitializePools(transform.position);
            }
        }
    }
}