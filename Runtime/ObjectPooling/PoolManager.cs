// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.ObjectPooling : PoolManager.cs
//
// All Rights Reserved

using System.Collections.Generic;
using UnityEngine;

namespace GibFrame.ObjectPooling
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        [SerializeField] private List<PoolCategory> poolsCategory = null;

        public GameObject Spawn(string categoryName, string poolTag, Vector3 position, Quaternion rotation)
        {
            PoolCategory poolCategory = GetCategory(categoryName);
            if (poolCategory != null)
            {
                return poolCategory.SpawnFromPool(poolTag, position, rotation);
            }
            return null;
        }

        public GameObject Spawn(string categoryName, Vector3 position, Quaternion rotation)
        {
            return Spawn(categoryName, "", position, rotation);
        }

        public int AddCategory(params PoolCategory[] categories)
        {
            int count = 0;
            foreach (PoolCategory category in categories)
            {
                if (!ContainsCategory(category))
                {
                    poolsCategory.Add(category);
                    category.Instantiate(transform.position);
                    count++;
                }
            }
            return count;
        }

        public bool AddCategory(string categoryName, params Pool[] pools)
        {
            return AddCategory(new PoolCategory(categoryName, pools)) > 0;
        }

        public int AddPool(string categoryName, params Pool[] pools)
        {
            int count = 0;
            PoolCategory category = GetCategory(categoryName);
            if (category == null)
            {
                category = new PoolCategory(categoryName, pools);
            }
            foreach (Pool pool in pools)
            {
                if (!category.ContainsPool(pool.Tag))
                {
                    category.AddPool(pools);
                    count++;
                }
                AddCategory(category);
            }
            return count;
        }

        public bool ContainsCategory(string name)
        {
            return poolsCategory.FindAll((c) => c.Name.Equals(name)).Count > 0;
        }

        public bool ContainsCategory(PoolCategory category)
        {
            return ContainsCategory(category.Name);
        }

        public PoolCategory GetCategory(string name)
        {
            return poolsCategory.Find((c) => c.Name.Equals(name));
        }

        public string GetRandomPoolTag(string categoryName)
        {
            PoolCategory poolCategory = GetCategory(categoryName);
            if (poolCategory != null)
            {
                return poolCategory.GetRandomPoolTag();
            }
            return null;
        }

        public void DeactivateObject(GameObject objectToDeactivate)
        {
            objectToDeactivate.SetActive(false);
        }

        public GameObject GetPrefab(string categoryName, string poolTag)
        {
            PoolCategory poolCategory = GetCategory(categoryName);
            if (poolCategory != null)
            {
                Pool pool = poolCategory.GetPool(poolTag);
                if (pool != null)
                {
                    return pool.Prefab;
                }
            }
            return null;
        }

        protected override void Awake()
        {
            base.Awake();
            if (poolsCategory == null)
            {
                poolsCategory = new List<PoolCategory>();
            }
            int length = poolsCategory.Count;
            for (int i = 0; i < length; i++)
            {
                poolsCategory[i].Instantiate(transform.position);
            }
        }
    }
}
