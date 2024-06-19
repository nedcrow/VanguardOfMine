using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCommon : MonoBehaviour
{
    public ECharacter type;

    void Start()
    {
        
    }

    public void Init(CharacterData charData)
    {
        type = (ECharacter)charData.type;
    }
}
