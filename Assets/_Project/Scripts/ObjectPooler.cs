using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectPooler
{
    private static Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();

    public static void CreatePool(GameObject prefab, Transform parent = null, int count = 10)
    {
        if (prefab == null)
        {
            Debug.LogError("Pool Prefab is null");
            return;
        }

        int hashCodeOfPrefab = prefab.GetHashCode();
        if (poolDictionary.ContainsKey(hashCodeOfPrefab))
        {
            Debug.LogError("Pool prefab already exists in the Dictionary");
            return;
        }

        Queue<GameObject> pooledObjectQueue = new Queue<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject obj = null;
            if (parent != null)
            {
                obj = Object.Instantiate(prefab, parent);
            }

            else
            {
                obj = Object.Instantiate(prefab);
            }
            
            obj.SetActive(false);
            pooledObjectQueue.Enqueue(obj);
        }
        
        poolDictionary.Add(hashCodeOfPrefab, pooledObjectQueue);
    }

    public static GameObject InstantiateFromPool(GameObject prefab, Vector3 position, Quaternion rotation, 
        Transform parent)
    {
        if (prefab == null)
        {
            Debug.LogError("Pool Prefab is null");
            return null;
        }
        
        int hashCodeOfPrefab = prefab.GetHashCode();
        if (!poolDictionary.ContainsKey(hashCodeOfPrefab))
        {
            Debug.LogError("Pool not initialized yet");
            return null;
        }

        GameObject pooledObject;
        if (poolDictionary[hashCodeOfPrefab].Count == 0)
        {
            pooledObject = Object.Instantiate(prefab, position, rotation, parent);
            return pooledObject;
        }
        pooledObject = poolDictionary[hashCodeOfPrefab].Dequeue();
        pooledObject.transform.position = position;
        pooledObject.transform.rotation = rotation;
        pooledObject.transform.SetParent(parent);
        pooledObject.SetActive(true);
        return pooledObject;
    }
    
    public static GameObject InstantiateFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogError("Pool Prefab is null");
            return null;
        }
        
        int hashCodeOfPrefab = prefab.GetHashCode();
        if (!poolDictionary.ContainsKey(hashCodeOfPrefab))
        {
            Debug.LogError("Pool not initialized yet");
            return null;
        }

        GameObject pooledObject;
        if (poolDictionary[hashCodeOfPrefab].Count == 0)
        {
            pooledObject = Object.Instantiate(prefab, position, rotation);
            return pooledObject;
        }
        pooledObject = poolDictionary[hashCodeOfPrefab].Dequeue();
        pooledObject.transform.position = position;
        pooledObject.transform.rotation = rotation;
        pooledObject.SetActive(true);
        return pooledObject;
    }

    public static void BackToPool(GameObject prefab, GameObject obj)
    {
        if (prefab == null || obj == null)
        {
            Debug.LogError("Pool Prefab is null or pool object is null");
            return;
        }

        int hashCodeOfPrefab = prefab.GetHashCode();
        if (!poolDictionary.ContainsKey(hashCodeOfPrefab))
        {
            Debug.LogError("Pool is not initialized");
            return;
        }
        
        obj.SetActive(false);
        poolDictionary[hashCodeOfPrefab].Enqueue(obj);
    }
}
