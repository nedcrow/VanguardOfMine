using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineManager : MonoBehaviour
{
    #region event
    public delegate void Boom_Del(GameObject target);
    public static Boom_Del BoomEvent;
    #endregion

    public List<GameObject> soldiersInRange = new List<GameObject>();

    public void BoonRainge()
    {
        soldiersInRange.ForEach((GameObject soldier) => { BoomAt(soldier); });
    }

    public void BoomAt(GameObject target)
    {
        BoomEvent(target);
    }

    public void SpawnMine()
    {

    }
}
