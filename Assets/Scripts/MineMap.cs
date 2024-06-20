using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MineMap : MonoBehaviour
{
    public GameObject tilePrefab;
    public Vector3 tileSize = Vector3.one;

    [SerializeField]
    List<GameObject> restedTileGameObjectList = new List<GameObject>();
    [SerializeField]
    List<GameObject> activatedTileGameObjectList = new List<GameObject>();
    List<TileData> restedTileList = new List<TileData>();
    List<TileData> activatedTileList = new List<TileData>();
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
            -size.x-1, -size.x, -size.x +1, // 좌상단 -> 우상단
            -1, 0, 1, // 좌 -> 우
            size.x - 1, size.x, size.x +1, // 좌하단 -> 우하단
        };
        int tileCount = size.x * size.y;

        // tile 및 mesh 초기화
        int currentCount = restedTileGameObjectList.Count + activatedTileGameObjectList.Count;
        if(currentCount == 0 && activatedTileGameObjectList.Count != tileCount)
        {
            for (int i = 0; i < tileCount; i++)
            {
                TileData tileData = new TileData(i, false, false, 0);
                activatedTileList.Add(tileData);

                activatedTileGameObjectList.Add(Instantiate(tilePrefab));
            }
        }
        else if(activatedTileGameObjectList.Count != tileCount)
        {
            // 활성 타일이 부족하면 비활성 타일에서 추가
            while(activatedTileGameObjectList.Count < tileCount && restedTileGameObjectList.Count > 0)
            {
                activatedTileList.Add(restedTileList[^1]);
                restedTileList.RemoveAt(restedTileList.Count-1);

                activatedTileGameObjectList.Add(restedTileGameObjectList[^1]);
                restedTileGameObjectList.RemoveAt(restedTileGameObjectList.Count - 1);
            }


            //// 활성중 타일이 있으면 모두 역순으로 비활성화 
            //if(activatedTileGameObjectList.Count > 0)
            //{
            //    int i = activatedTileList.Count;
            //    while (i > 0)
            //    {
            //        restedTileList.Add(activatedTileList[^1]);
            //        activatedTileList.RemoveAt(activatedTileList.Count - 1);

            //        restedTileGameObjectList.Add(activatedTileGameObjectList[^1]);
            //        activatedTileGameObjectList.RemoveAt(activatedTileGameObjectList.Count - 1);
            //        i--;
            //    }
            //}


            //// restedTile 수량으로 커버되면 새로 안 만듦
            //if(restedTileList.Count >= tileCount)
            //{
            //    for(int i=0; i<tileCount; i++)
            //    {
            //        activatedTileList.Add(restedTileList[^1]);
            //        restedTileList.RemoveAt(restedTileList.Count - 1);

            //        activatedTileGameObjectList.Add(restedTileGameObjectList[^1]);
            //        restedTileGameObjectList.RemoveAt(restedTileGameObjectList.Count - 1);
            //    }
            //}
            //else
            //{
            //    int tempDistance_ = tileCount - restedTileList.Count;
            //    int restedTileCount = restedTileList.Count;
            //    for (int i = 0; i < restedTileList.Count; i++)
            //    {
            //        activatedTileList.Add(restedTileList[^1]);
            //        restedTileList.RemoveAt(restedTileList.Count - 1);

            //        activatedTileGameObjectList.Add(restedTileGameObjectList[^1]);
            //        restedTileGameObjectList.RemoveAt(restedTileGameObjectList.Count - 1);
            //    }
            //    for (int i = restedTileCount; i < tileCount; i++)
            //    {
            //        TileData tileData = new TileData(i, false, false, 0);
            //        activatedTileList.Add(tileData);

            //        activatedTileGameObjectList.Add(Instantiate(tilePrefab));
            //    }
            //}

            int tempDistance_ = tileCount - activatedTileGameObjectList.Count;
            while(tempDistance_ > 0)
            {
                TileData tileData = new TileData(i, false, false, 0);
                activatedTileList.Add(tileData);

                activatedTileGameObjectList.Add(Instantiate(tilePrefab));
                tempDistance_--;
            }
        }


        // mine
        mineList = new List<int>();
        while (mineList.Count < countOfMine)
        {
            int randomIndex = Random.Range(0, tileCount);
            if (!mineList.Contains(randomIndex))
            {
                mineList.Add(randomIndex);
                activatedTileList[randomIndex].bHasMine = true;
            }
        }

        activatedTileList.ForEach((TileData tile) => {
            if (!tile.bHasMine) {
                int nearbyMineCount = 0;
                foreach (int weight in aroundIdxArr)
                {
                    int nearbyIdx = tile.idx + weight;
                    if (nearbyIdx > -1 && nearbyIdx < activatedTileList.Count)
                    {
                        if (activatedTileList[nearbyIdx].bHasMine) nearbyMineCount++;
                    }
                }
                tile.nearbyMineCount = nearbyMineCount;
            };
        });

        // obstacle
        // attach for mesh

        // tile mesh setting
        for(int i=0; i<activatedTileGameObjectList.Count; i++)
        {
            TileData tile = activatedTileList[i];
            GameObject tileGameObject = activatedTileGameObjectList[i];

            float posX = pivot.x + (tile.idx % size.x);
            float posY = pivot.y;
            float posZ = pivot.z + Mathf.Floor(tile.idx / size.x);

            tileGameObject.transform.parent = transform;
            tileGameObject.name = posX.ToString() + " x " + posZ.ToString();
            tileGameObject.transform.localPosition = new Vector3(
                posX * tileSize.x,
                posY * tileSize.y,
                posZ * tileSize.z
                );
            tileGameObject.transform.localScale = tileSize;

            TileComponent tileComp = tileGameObject.GetComponent<TileComponent>();
            if (tileComp == null) tileComp = tileGameObject.AddComponent<TileComponent>();
            tileComp.ActiveCube();
            tileComp.Init(tile);
        }

        foreach (var tileGameObject in restedTileGameObjectList)
        {
            TileComponent tileComp = tileGameObject.GetComponent<TileComponent>();
            if (tileComp == null) tileComp = tileGameObject.AddComponent<TileComponent>();
            tileComp.RestCube();
        }

        // map
        float mapX = (size.x * 0.5f) - (0.5f) * tileSize.x;
        float mapY = (size.y * 0.5f) - (0.5f) * tileSize.y;
        transform.position = new Vector3(
            -mapX,
            0,
            -mapY
            );
    }
    // size * y + x
}

   
