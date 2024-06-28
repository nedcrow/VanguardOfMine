using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum EArmy
//{
//    Detector = 0,
//    Flag = 1,
//}

/// <summary>
/// 등록된 GameObject를 특정 위치로 생성 및 상태 관리
/// </summary>
public class ArmySpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject currentArmyPrefab;
    public List<GameObject> restedArmies = new List<GameObject>();
    public List<GameObject> activatedArmies = new List<GameObject>();


    public void Start()
    {
        GameManager.instance.mineMap.AfterSpawnMapEvent += (() => { RIP_All(); });
        CursorComponent.OnClickTile_Left += ((GameObject go) => { 
            if(!go.GetComponent<TileComponent>().opened) SpawnArmyTo(go.transform.position + (Vector3.up * 2)); 
        });
    }

    public void Init(GameObject prefab)
    {
        currentArmyPrefab = prefab;
        foreach (GameObject obj in restedArmies) Destroy(obj);
        foreach (GameObject obj in activatedArmies) Destroy(obj);

        if (restedArmies.Count > 0) { restedArmies.Clear(); }
        if (activatedArmies.Count > 0) { activatedArmies.Clear(); }
    }

    public void SpawnArmyTo(Vector3 pos)
    {
        GameObject army = SpawnArmy();
        army.transform.localPosition = pos;
        army.GetComponent<CharacterCommon>().OnBody();
    }

    public GameObject SpawnArmy()
    {
        GameObject army;
        if(restedArmies.Count > 0)
        {
            army = restedArmies[^1];
            //army.SetActive(true);
            activatedArmies.Add(army);
            restedArmies.RemoveAt(restedArmies.Count - 1);
        }
        else
        {
            army = GameObject.Instantiate(currentArmyPrefab);
            //army.SetActive(true);
            army.transform.parent = transform;
            activatedArmies.Add(army);
            //army.GetComponent<CharacterBase>().Init(); // 캐릭터 타입 지정과 함께 외형 변경 등 초기화
        }
        return army;
    }

    public void RIP_All()
    {
        int idx = activatedArmies.Count - 1;
        for (int i = idx; i>=0; i--)
        {
            RIP_For(activatedArmies[i]);
        }
    }

    public void RIP_For(GameObject target)
    {
        int objectIndex = activatedArmies.FindIndex(gameObject => string.Equals(target.name, gameObject.name));
        if (objectIndex >= 0)
        {
            activatedArmies[objectIndex].GetComponent<CharacterCommon>().OffBody();
            restedArmies.Add(activatedArmies[objectIndex]);
            activatedArmies.RemoveAt(objectIndex);
        }
    }
}