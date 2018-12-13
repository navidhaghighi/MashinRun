#region nameSpaces
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion
public class ObjectPooler:MonoBehaviour
{
    #region variables
    public GameManager gameManager;
    Dictionary<string, GameObject> lastItems;
    private int indexCounter = 0;
    public  Dictionary<string , Queue<GameObject>> poolsDictionary;
    public int howManyActiveObjects;
    GameObject[] poolGameObjects;
    #endregion
    #region events_and_delegates
#endregion
    #region monobehaviour_events
    void Awake()
    {
        poolsDictionary = new Dictionary<string, Queue<GameObject>>();
        lastItems = new Dictionary<string, GameObject>();
        //SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }
    #endregion
    #region methods
    public void DestroyAllPools()
    {
        foreach (string key in poolsDictionary.Keys)
        {
            foreach (GameObject go in poolsDictionary[key])
            {
                Destroy(go);
            }
        }
        foreach (string key in lastItems. Keys)
        {
                Destroy(lastItems[key]);
        }
        poolsDictionary.Clear();
        lastItems.Clear();
    }
    public Queue<GameObject> GetPool(string tag)
    {
        if ((poolsDictionary != null) &&
            (poolsDictionary[tag] != null))
            return poolsDictionary[tag];
        else return null;
    }
   public void CreatePoolFromResources
       (string folderName, int howManyObjects)
   {
       GameObject go;
       Queue<GameObject> queue = new Queue<GameObject>();
       List<GameObject> gameObjects = DataManager.GetAllDataFromResourcesFolder
           <GameObject>(folderName);
       poolsDictionary.Add(folderName, queue);
       for (int i = 0; i < howManyObjects; i++)
       {
           
               go = Instantiate(gameObjects[UnityEngine
                   .Random.Range(0,gameObjects.Count)]);
               PoolObject(folderName, go);
               go.SetActive(false);
           
       }     
   }
   public void PoolObject(string tag,GameObject go)
    {
        poolsDictionary[tag].Enqueue(go);

        if (lastItems.ContainsKey(tag))
        {
            if((lastItems[tag])&&(lastItems[tag].GetComponent<PooledObject>()))
                indexCounter = (lastItems[tag].GetComponent<PooledObject>
                    ().index) + 1;
        }
        else
        {
            indexCounter = 0;
            lastItems.Add(tag, go);
        }
        if (go.GetComponent<PooledObject>())
            go.GetComponent<PooledObject>().index = indexCounter;
        lastItems[tag] = go;
    }

    public GameObject GetFirstItemFromPool(string tag)
    {
        if (!(poolsDictionary.ContainsKey(tag)))
            return null;
        return poolsDictionary[tag].Peek();
    }
    public GameObject GetLastItemFromPool(string tag)
    {
        if (lastItems == null)
        {
            Debug.LogWarning("the last item of " + this +
               " wasn't set");
            return null;
        }
        else if (!(lastItems.ContainsKey(tag)))
        {
            return null;
        }
        else
        {
            return lastItems[tag];
        }
    }
    public GameObject SpawnFromPool(string tag,Vector3 position) 
    {
        GameObject objectToSpawn;
        objectToSpawn = poolsDictionary[tag].Dequeue();
        objectToSpawn.transform.position = position;
        PoolObject(tag,objectToSpawn);
        objectToSpawn.SetActive(true);
        return objectToSpawn;
    }
    #endregion
}
