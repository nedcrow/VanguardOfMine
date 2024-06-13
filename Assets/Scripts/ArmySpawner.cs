﻿using System.Collections;
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
    public GameObject currentArmyPrefab;
    public List<GameObject> restedArmies = new List<GameObject>();
    public List<GameObject> activatedArmies = new List<GameObject>();


    public void Start()
    {
        
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
        GameObjct army = SpawnArmy();
        army.transform.localPosition = pos;
    }

    public GameObject SpawnArmy()
    {
        GameObject army;
        if(restedArmies.Count > 0)
        {
            army = restedArmies[^1];
            army.transform.SetActive(true);
            activatedArmies.Add(army);
            restedArmies.RemoveAt(restedArmies.Count - 1);
        }
        else
        {
            army = GameObject.Instantiate(currentArmyPrefab);
            army.transform.parent = transform;
            army.transform.SetActive(true);
            //army.GetComponent<CharacterBase>().Init(); // 캐릭터 타입 지정과 함께 외형 변경 등 초기화
        }
        return army;
    }

    public void RIP_For(GameObject target)
    {
        int objectIndex = objects.FindIndex(gameObject => string.Equals(target.name, gameObject.name));
        if (objectIndex >= 0)
        {
            restedArmies.Add(activatedArmies[objectIndex]);
            activatedArmies.RemoveAt(objectIndex);
        }
    }
}