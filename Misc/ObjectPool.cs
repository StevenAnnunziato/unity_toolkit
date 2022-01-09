using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    [Header("Pool Settings")]
    public GameObject objectToPool;
    public int amountToPool;

    // private member variables
    private static ObjectPool SharedInstance;
    private GameObject[] pooledObjects;
    private int currentIndex;

    private void Awake()
    {
        // ObjectPool is a singleton
        if (SharedInstance != null && SharedInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            SharedInstance = this;
        }
    }

    void Start()
    {
        pooledObjects = new GameObject[amountToPool];
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects[i] = obj;
        }
    }

    public GameObject GetPooledObject()
    {
        GameObject ob = pooledObjects[currentIndex++];
        if (currentIndex >= amountToPool)
            currentIndex = 0;
        ob.SetActive(true);
        ob.transform.position = new Vector3(0f, 0, 0f);
        return ob;
    }

}
