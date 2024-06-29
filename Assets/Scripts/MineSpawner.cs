using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : GameObjectPooler
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

    public void ReadyMineComponents(int count)
    {
        int currentCount = restedGameObjects.Count + activatedGameObjects.Count;

        if (count == activatedGameObjects.Count) return;

        if (count > activatedGameObjects.Count)
        {
            int insufficientCount = count - activatedGameObjects.Count;
            int weightIndex = activatedGameObjects.Count;
            for (int i = 0; i < insufficientCount; i++)
            {
                GameObject tile = Spawn();
                if (!tile.GetComponent<MineComponent>()) tile.AddComponent<MineComponent>();
                tile.GetComponent<MineComponent>().Init(
                    new MineData(i + weightIndex, GameManager.instance.mineLevel)
                    );
            }
        }
        else if (count < activatedGameObjects.Count)
        {
            int leftOverCount = activatedGameObjects.Count - count;
            for (int i = 0; i < leftOverCount; i++)
            {
                RIP_For(activatedGameObjects[^1]);
            }
        }
    }
}
