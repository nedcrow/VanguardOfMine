using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCommon : MonoBehaviour
{
    public ECharacter type;
    public GameObject body;

    void Start()
    {
        
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
}
