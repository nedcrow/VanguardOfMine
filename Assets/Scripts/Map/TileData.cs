using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public TileData(
        int idx,
        bool bHasMine,
        bool bActivated,
        int nearByMineCount
    )
    {
        this.idx = idx;
        this.bHasMine = bHasMine;
        this.bActivated = bActivated;
        this.nearbyMineCount = nearByMineCount;
    }

    public int idx;
    public bool bHasMine;
    public bool bActivated;
    public int nearbyMineCount;
}
