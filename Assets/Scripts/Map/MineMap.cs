using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MineMap : MonoBehaviour
{
    public delegate void AfterSpawnMap_Del();
    public event AfterSpawnMap_Del AfterSpawnMapEvent;

    public GameObject tilePrefab;
    public GameObject minePrefab;
    public Vector3 tileSize = Vector3.one;

    [SerializeField]
    List<GameObject> restedTileGameObjectList = new List<GameObject>();
    [SerializeField]
    List<GameObject> activatedTileGameObjectList = new List<GameObject>();
    List<TileData> restedTileList = new List<TileData>();
    List<TileData> activatedTileList = new List<TileData>();

    [SerializeField]
    List<MineComponent> restedMineList = new List<MineComponent>();
    [SerializeField]
    List<MineComponent> activatedMineList = new List<MineComponent>();
    //List<int> mineList;
    List<MineComponent> mineList;
    Vector3 pivot = Vector3.zero;
    Vector2Int mapSize = Vector2Int.zero;

    void Start()
    {
        GameManager.instance.gameTimer.OnTimeOverEvent += GameOver;
    }


    public void GameOver(float currentTime)
    {
        Debug.Log("GameOver");
    }

    public void Spawn(Vector2Int size, int countOfMine)
    {
        mapSize = size;

        int[] aroundIdxArr = {
            -size.x-1, -size.x, -size.x +1, // 좌상단 -> 우상단
            -1, 0, 1, // 좌 -> 우
            size.x - 1, size.x, size.x +1, // 좌하단 -> 우하단
        };

        int tileCount = size.x * size.y;

        // tile 초기화
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
        else if(activatedTileGameObjectList.Count < tileCount)
        {
            // 활성 타일이 부족하면 비활성 타일에서 추가
            while(activatedTileGameObjectList.Count < tileCount && restedTileGameObjectList.Count > 0)
            {
                activatedTileList.Add(restedTileList[^1]);
                restedTileList.RemoveAt(restedTileList.Count-1);

                activatedTileGameObjectList.Add(restedTileGameObjectList[^1]);
                restedTileGameObjectList.RemoveAt(restedTileGameObjectList.Count - 1);
            }

            // 그래도 부족하면 추가 생성
            int tempDistance_ = tileCount - activatedTileGameObjectList.Count;
            int idx = activatedTileList[^1].idx;
            while (tempDistance_ > 0)
            {
                idx += 1;
                TileData tileData = new TileData(idx, false, false, 0);
                activatedTileList.Add(tileData);

                activatedTileGameObjectList.Add(Instantiate(tilePrefab));
                tempDistance_--;
            }
        }
        else if(activatedTileGameObjectList.Count > tileCount)
        {
            // 남는 타일 비활성
            while (activatedTileGameObjectList.Count > tileCount)
            {
                restedTileList.Add(activatedTileList[^1]);
                activatedTileList.RemoveAt(activatedTileList.Count - 1);

                restedTileGameObjectList.Add(activatedTileGameObjectList[^1]);
                activatedTileGameObjectList.RemoveAt(activatedTileGameObjectList.Count - 1);
            }
        }

        // mine 초기화
        if (activatedMineList.Count > 0)
        {
            while (activatedMineList.Count > 0)
            {
                restedMineList.Add(activatedMineList[^1]);
                activatedMineList.RemoveAt(activatedMineList.Count - 1);
            }
        }
        while (activatedMineList.Count < countOfMine)
        {
            int randomIndex = Random.Range(0, tileCount);
            bool hasDuplicate = restedMineList.Any(mine => mine.GetIndex() == randomIndex);
            if (!hasDuplicate)
            {
                GameObject mineGameObj;
                MineData mineData = new MineData(randomIndex, GameManager.instance.mineLevel);
                if(restedMineList.Count <= 1)
                {
                    if (minePrefab)
                    {
                        mineGameObj = Instantiate(minePrefab);
                        mineGameObj.GetComponent<MineComponent>().Init(mineData);
                        activatedMineList.Add(mineGameObj.GetComponent<MineComponent>());
                    }
                    else
                    {
                        Debug.LogWarning("Empty minePrefab");
                    }                   
                }
                else
                {
                    restedMineList[^1].Init(mineData);
                    activatedMineList.Add(restedMineList[^1]);
                    restedMineList.RemoveAt(restedMineList.Count - 1);
                }
                activatedTileList[randomIndex].bHasMine = true;
            }
        }

        // tile 설정
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

        // tile game object setting
        for (int i=0; i<activatedTileGameObjectList.Count; i++)
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
            tileComp.SetTilePosition(new Vector2Int((int)posX, (int)posZ));
        }

        foreach (var tileGameObject in restedTileGameObjectList)
        {
            TileComponent tileComp = tileGameObject.GetComponent<TileComponent>();
            if (tileComp == null) tileComp = tileGameObject.AddComponent<TileComponent>();
            tileComp.RestCube();
        }

        // mine game object setting
        activatedMineList.ForEach((MineComponent mine) => {
            Vector3 tileGameObjectPosition = activatedTileGameObjectList[mine.GetIndex()].transform.localPosition;
            mine.transform.parent = transform;
            mine.transform.localPosition = tileGameObjectPosition;
            mine.transform.localScale = tileSize * ((int)mine.GetMineType() * 2 - 1);
            mine.SetMinePosition(new Vector2Int(
                mine.GetIndex() % size.x,
                Mathf.FloorToInt(mine.GetIndex() / size.x)
                ));
        });

        // map
        float mapX = (size.x * 0.5f) - (0.5f) * tileSize.x;
        float mapY = (size.y * 0.5f) - (0.5f) * tileSize.y;
        transform.position = new Vector3(
            -mapX,
            0,
            -mapY
            );

        AfterSpawnMapEvent();
    }

    public Vector2Int GetSize()
    {
        return mapSize;
    }

    public TileComponent GetTileCompAt(int idx)
    {
        return activatedTileGameObjectList[idx].GetComponent<TileComponent>();
    }
}

   
