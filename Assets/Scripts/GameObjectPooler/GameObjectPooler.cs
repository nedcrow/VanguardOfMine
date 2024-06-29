using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPooler : MonoBehaviour
{
    public GameObject targetPrefab;
    public List<GameObject> restedGameObjects = new List<GameObject>();
    public List<GameObject> activatedGameObjects = new List<GameObject>();
    [Min(1)]
    public int maxCount = 1;
    public List<int> idxList = new List<int>();

    private void Awake()
    {
        for(int i=0; i<maxCount; i++)
        {
            idxList.Add(i);
        }
    }

    public GameObject Spawn()
    {
        GameObject gameObj;
        bool exists = restedGameObjects.Count > 0;
        if (exists)
        {
            gameObj = restedGameObjects[^1];
            restedGameObjects.RemoveAt(restedGameObjects.Count - 1);
        }
        else
        {
            if(idxList.Count <= 0)
            {
                Debug.LogWarning("OutOfRangeException: max count in pooler.");
                return null;
            }
            int idx = idxList[^1];
            if (targetPrefab == null)
            {
                Debug.LogWarning("Null Exception: target prefab object for pooling(" + name + ")");
                return null;
            }
            gameObj = Instantiate(targetPrefab);
            gameObj.transform.parent = transform;
            gameObj.AddComponent<IndexComponent>().Init(name + "_" + idx);
            idxList.RemoveAt(idxList.Count - 1);
        }
        activatedGameObjects.Add(gameObj);
        
        return gameObj;
    }

    public void RIP_All()
    {
        int idx = activatedGameObjects.Count - 1;
        for (int i = idx; i >= 0; i--)
        {
            RIP_For(activatedGameObjects[i]);
        }
    }
    
    public void RIP_For(GameObject target)
    {
        int objectIndex = activatedGameObjects.FindIndex(gameObject => string.Equals(
           target.GetComponent<CharacterCommon>().GetCharacterPosition(),
           gameObject.GetComponent<CharacterCommon>().GetCharacterPosition())
       );
        if (objectIndex >= 0)
        {
            activatedGameObjects[objectIndex].GetComponent<CharacterCommon>().OffBody();
            restedGameObjects.Add(activatedGameObjects[objectIndex]);
            activatedGameObjects.RemoveAt(objectIndex);
        }
    }
}
