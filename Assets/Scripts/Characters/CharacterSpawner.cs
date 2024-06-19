using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    #region Event
    //public delegate void Del_SpawnCharacter(CharacterData charData);
    //public static event Del_SpawnCharacter OnSpawnAnimal;
    //public static event Del_SpawnCharacter OnSpawnSoldier_SONA;
    //public static event Del_SpawnCharacter OnSpawnSoldier_Flag;
    #endregion

    public List<CharacterData> targetPrefabs;
    List<GameObject> restCharacterList;
    List<GameObject> characterList;
    ECharacter targetType;

    void Start()
    {
        characterList = new List<GameObject>();
        // targetType 맞는 data list 불러와서 
    }

    public void SpawnTo(Vector3 position, int targetIndex=0)
    {
        //int 
        //GameObject tempCharacter = Instantiate(targetPrefabs[targetIndex])
    }
}
