using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECharacter
{
    Light = 0,
    Common = 1,
    Heavy = 2
}

public class CharacterData
{
    public CharacterData(
        int type,
        string name
    )
    {
        this.type = type;
        this.name = name;

    }

    public int type;
    public string name;
    public string bodyMeshPath;
    public string bodyMatPath;
    public string subMeshPath;
    public string subMatPath;
}


[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Object/CharacterData")]
public class CharacterDataTable : ScriptableObject
{
    public enum ECharacter
    {
        Light = 0,
        Common = 1,
        Heavy = 2
    }

    [Header("# Main Info")]
    public int id;
    public int type;
    public string name;

    [Header("# Path")]
    public string bodyMeshPath;
    public string bodyMatPath;
    public string subMeshPath;
    public string subMatPath;
}