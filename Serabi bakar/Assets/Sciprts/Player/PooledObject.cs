using System.Collections.Generic;
using UnityEngine;

public class PooledObjects : MonoBehaviour
{
    public static PooledObjects Instance; 

    [Header("Pengaturan Pool")]
    public GameObject objectToPool;
    
    private List<GameObject> pooledObjects = new List<GameObject>();

    void Awake() 
    {
        if (Instance == null) {
            Instance = this;
        }
    }

    public GameObject GetPooledObject()
    {
        // Cari peluru yang sedang tidak aktif di Hierarchy
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i]; 
            }
        }

        // Jika semua peluru sedang melayang (aktif), buat peluru ekstra
        GameObject obj = Instantiate(objectToPool);
        obj.SetActive(false); 
        pooledObjects.Add(obj); 
        
        return obj; 
    }
}