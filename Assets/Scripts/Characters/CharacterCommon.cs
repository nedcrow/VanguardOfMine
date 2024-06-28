using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCommon : MonoBehaviour
{
    public ECharacter type;
    public GameObject body;

    [SerializeField]
    Vector2Int characterPosition;

    void Start()
    {
        //MineData.BoomEvent += (GameObject target) => {
        //    if (target == gameObject) transform.parent.GetComponent<SoldierSpawner>().RIP_For(gameObject);
        //};
    }

    public void Init(CharacterData charData)
    {
        type = (ECharacter)charData.type;
    }

    public void OnBody()
    {
        body.SetActive(true);
    }

    public void OffBody()
    {
        body.SetActive(false);
    }

    public void SetCharacterPosition(Vector2Int pos)
    {
        characterPosition = pos;
    }

    public Vector2Int GetCharacterPosition()
    {
        return characterPosition;
    }
}
