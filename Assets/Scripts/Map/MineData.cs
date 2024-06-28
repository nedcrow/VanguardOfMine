using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMineType
{
    normal = 1,
    huge = 2,
}

public class MineData
{   
    
    public MineData( int idx, EMineType type)
    {
        Init(idx, type);
    }

    public int idx;
    public EMineType mineType;

    public void Init(int idx, EMineType type)
    {
        this.idx = idx;
        this.mineType = type;
    }
}
