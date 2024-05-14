using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineMap : MonoBehaviour
{
    public GameObject tilePrefab;
    public Vector3 tileSize = Vector3.one;

    List<TileData> tileList;
    List<int> mineList;
    Vector3 pivot = Vector3.zero;

    void Awake()
    {
        GameTimer.OnTimeOverEvent += GameOver;
    }


    public void GameOver(float currentTime)
    {
        Debug.Log("GameOver");
    }

    public void Spawn(Vector2Int size, int countOfMine)
    {
        int[] aroundIdxArr = {
            -size.x-1, -size.x, -size.x +1, // ÁÂ»ó´Ü -> ¿ì»ó´Ü
            -1, 0, 1, // ÁÂ -> ¿ì
            size.x - 1, size.x, size.x +1, // ÁÂÇÏ´Ü -> ¿ìÇÏ´Ü
        };
        int tileCount = size.x * size.y;

        // tile
        tileList = new List<TileData>();
        for (int i = 0; i < tileCount; i++)
        {
            TileData tileData = new TileData(i, false, false, 0);
            tileList.Add(tileData);
        }

        // mine
        mineList = new List<int>();
        while (mineList.Count < countOfMine)
        {
            int randomIndex = Random.Range(0, tileCount);
            if (!mineList.Contains(randomIndex))
            {
                mineList.Add(randomIndex);
                tileList[randomIndex].bHasMine = true;
            }
        }

        tileList.ForEach((TileData tile) => {
            if (!tile.bHasMine) {
                int nearbyMineCount = 0;
                foreach (int weight in aroundIdxArr)
                {
                    int nearbyIdx = tile.idx + weight;
                    if (nearbyIdx > -1 && nearbyIdx < tileList.Count)
                    {
                        if (tileList[nearbyIdx].bHasMine) nearbyMineCount++;
                    }
                }
                tile.nearbyMineCount = nearbyMineCount;
            };
        });

        // obstacle
        // attach for mesh

        // mesh
        tileList.ForEach((TileData tile) =>
        {
            float posX = pivot.x + (tile.idx % size.x);
            float posY = pivot.y;
            float posZ = pivot.z + Mathf.Floor(tile.idx / size.x);

            GameObject tileMesh = Instantiate(tilePrefab);
            tileMesh.transform.parent = transform;
            tileMesh.name = posX.ToString() + " x " + posZ.ToString();
            tileMesh.transform.localPosition = new Vector3(
                posX * tileSize.x,
                posY * tileSize.y,
                posZ * tileSize.z
                );
            tileMesh.transform.localScale = tileSize;            
            TileComponent tileComp = tileMesh.GetComponent<TileComponent>();
            if (tileComp == null) tileComp = tileMesh.AddComponent<TileComponent>();
            tileComp.Init(tile);
        });

        // map
        float mapX = (size.x * 0.5f) - (0.5f) * tileSize.x;
        float mapY = (size.y * 0.5f) - (0.5f) * tileSize.y;
        transform.position = new Vector3(
            transform.position.x - mapX,
            0,
            transform.position.x - mapY
            );
    }
    // size * y + x
}

   
