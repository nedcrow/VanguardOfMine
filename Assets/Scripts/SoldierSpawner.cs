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
public class SoldierSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject currentSoldierPrefab;
    public List<GameObject> restedSoldiers = new List<GameObject>();
    public List<GameObject> activatedSoldiers = new List<GameObject>();


    public void Start()
    {
        GameManager.instance.mineMap.AfterSpawnMapEvent += (() => { RIP_All(); });
        CursorComponent.OnClickTile_Left += ((GameObject go) => {
            Vector2Int tilePos = go.GetComponent<TileComponent>().GetTilePosition();
            if (!go.GetComponent<TileComponent>().opened) SpawnSoldierTo(go.transform.position + (Vector3.up * 2), tilePos); 
        });
    }

    public void Init(GameObject prefab)
    {
        currentSoldierPrefab = prefab;
        foreach (GameObject obj in restedSoldiers) Destroy(obj);
        foreach (GameObject obj in activatedSoldiers) Destroy(obj);

        if (restedSoldiers.Count > 0) { restedSoldiers.Clear(); }
        if (activatedSoldiers.Count > 0) { activatedSoldiers.Clear(); }
    }

    public void SpawnSoldierTo(Vector3 realPos, Vector2Int intPos)
    {
        GameObject soldier = SpawnSoldier();
        soldier.transform.localPosition = realPos;
        soldier.GetComponent<CharacterCommon>().OnBody();
        soldier.GetComponent<CharacterCommon>().SetCharacterPosition(intPos);
    }

    public GameObject SpawnSoldier()
    {
        GameObject soldier;
        if(restedSoldiers.Count > 0)
        {
            soldier = restedSoldiers[^1];
            activatedSoldiers.Add(soldier);
            restedSoldiers.RemoveAt(restedSoldiers.Count - 1);
        }
        else
        {
            soldier = GameObject.Instantiate(currentSoldierPrefab);
            soldier.transform.parent = transform;
            activatedSoldiers.Add(soldier);
        }
        return soldier;
    }

    public void RIP_All()
    {
        int idx = activatedSoldiers.Count - 1;
        for (int i = idx; i>=0; i--)
        {
            RIP_For(activatedSoldiers[i]);
        }
    }

    public void RIP_For(GameObject target)
    {
        int objectIndex = activatedSoldiers.FindIndex(gameObject => string.Equals(
            target.GetComponent<CharacterCommon>().GetCharacterPosition(), 
            gameObject.GetComponent<CharacterCommon>().GetCharacterPosition())
        );
        if (objectIndex >= 0)
        {
            activatedSoldiers[objectIndex].GetComponent<CharacterCommon>().OffBody();
            restedSoldiers.Add(activatedSoldiers[objectIndex]);
            activatedSoldiers.RemoveAt(objectIndex);
        }
    }
}