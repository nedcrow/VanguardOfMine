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
public class SoldierSpawner : GameObjectPooler
{
    public void Start()
    {
        GameManager.instance.mineMap.AfterSpawnMapEvent += (() => { RIP_All(); });
        CursorComponent.OnClickTile_Left += ((GameObject go) => {
            Vector2Int tilePos = go.GetComponent<TileComponent>().GetTilePosition();
            if (!go.GetComponent<TileComponent>().opened) SpawnSoldierTo(go.transform.localPosition + (Vector3.up * 2), tilePos); 
        });
    }

    public void Init(GameObject prefab)
    {
        targetPrefab = prefab;
        if (activatedGameObjects.Count > 0) RIP_All();
    }

    public void SpawnSoldierTo(Vector3 realPos, Vector2Int intPos)
    {
        GameObject soldier = Spawn();
        soldier.transform.localPosition = realPos;
        soldier.GetComponent<CharacterCommon>().OnBody();
        soldier.GetComponent<CharacterCommon>().SetCharacterPosition(intPos);
    }
}