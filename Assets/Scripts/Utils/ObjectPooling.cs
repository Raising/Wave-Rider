using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour {
    public static ObjectPooling current;
    public GameObject poolObject;
    public int poolSize = 50;
    public bool willGrow = true;
    public List<GameObject> poolObjects;

    private void Awake() {
        current = this;
    }

    // Use this for initialization
    void Start () {
        poolObjects = new List<GameObject>();
        for(int i = 0; i < poolSize; i++) {
            AddObjectToPool();
        }
	}

    public GameObject AddObjectToPool() {
        GameObject obj = (GameObject)Instantiate(poolObject);
        obj.SetActive(false);
        poolObjects.Add(obj);
        return obj;
    }

    public GameObject GetPooledObject() {
        for(int i = 0; i < poolObjects.Count; i++) {
            if(!poolObjects[i].activeInHierarchy) {
                return poolObjects[i];
            }
        }

        if(willGrow) {
            GameObject obj = AddObjectToPool();
            poolSize++;
            obj.SetActive(true);
            return obj;
        }

        return null;
    }
}
