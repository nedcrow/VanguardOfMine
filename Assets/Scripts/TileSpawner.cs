using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : GameObjectPooler
{
    public void ReadyTileComponents(int count)
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
                if (!tile.GetComponent<TileComponent>()) tile.AddComponent<TileComponent>();
                tile.GetComponent<TileComponent>().Init(
                    new TileData(i + weightIndex, false, false, 0)
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

    public TileComponent GetTileCompAt(int idx)
    {
        return activatedGameObjects[idx].GetComponent<TileComponent>();
    }
}
