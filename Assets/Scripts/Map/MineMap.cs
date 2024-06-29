using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MineMap : MonoBehaviour
{
    public delegate void AfterSpawnMap_Del();
    public event AfterSpawnMap_Del AfterSpawnMapEvent;

    public TileSpawner tileSpawner;
    public MineSpawner mineSpawner;
    public SoldierSpawner soldierSpawner;
    public Vector3 tileSize = Vector3.one;

    Vector2Int size = Vector2Int.zero;

    void Start()
    {
        GameManager.instance.gameTimer.OnTimeOverEvent += GameOver;
    }


    public void GameOver(float currentTime)
    {
        Debug.Log("GameOver");
    }

    public void DrawMap(Vector2Int size, int countOfMine)
    {
        this.size = size;
        int tileCount = size.x * size.y;
        int[] aroundIdxArr = {
            -size.x-1, -size.x, -size.x +1, // 좌상단 -> 우상단
            -1, 0, 1, // 좌 -> 우
            size.x - 1, size.x, size.x +1, // 좌하단 -> 우하단
        };

        #region TileMap
        tileSpawner.ReadyTileComponents(tileCount);
        List<GameObject> activatedTiles = tileSpawner.activatedGameObjects;
        for (int i = 0; i < activatedTiles.Count; i++)
        {
            TileData tileData = activatedTiles[i].GetComponent<TileComponent>().GetTileData();
            GameObject tileGameObject = activatedTiles[i];

            float posX = (tileData.idx % size.x);
            float posY = 0;
            float posZ = Mathf.Floor(tileData.idx / size.x);

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
            tileComp.SetTilePosition(new Vector2Int((int)posX, (int)posZ));
        }

        foreach (var tileGameObject in tileSpawner.restedGameObjects)
        {
            TileComponent tileComp = tileGameObject.GetComponent<TileComponent>();
            if (tileComp == null) tileComp = tileGameObject.AddComponent<TileComponent>();
            tileComp.RestCube();
        }
        #endregion

        #region LocateMine
        List<int> mineIndexList = new List<int>();
        mineSpawner.ReadyMineComponents(countOfMine);
        mineSpawner.activatedGameObjects.ForEach((GameObject mine) => {
            bool hasDuplicate = true;
            int randomIndex = Random.Range(0, tileCount);
            while (hasDuplicate)
            {
                randomIndex = Random.Range(0, tileCount);
                hasDuplicate = mineIndexList.Contains(randomIndex);
            }
            mineIndexList.Add(randomIndex);
        });

        for(int i=0; i<mineIndexList.Count; i++)
        {
            mineSpawner.activatedGameObjects[i].GetComponent<MineComponent>().Init(
                new MineData(mineIndexList[i], GameManager.instance.mineLevel)
                );
        }

        mineSpawner.activatedGameObjects.ForEach((GameObject mine) =>
        {
            MineComponent mineComp = mine.GetComponent<MineComponent>();
            TileComponent tileComp = tileSpawner.activatedGameObjects[mineComp.GetIndex()].GetComponent<TileComponent>();
            Vector3 tileGameObjectPosition = tileComp.transform.localPosition;
            tileComp.hasMine = true;
            mine.transform.localPosition = tileGameObjectPosition;
            mine.transform.localScale = tileSize * ((int)mineComp.GetMineType() * 2 - 1);
            mineComp.SetMinePosition(new Vector2Int(
                mineComp.GetIndex() % size.x,
                Mathf.FloorToInt(mineComp.GetIndex() / size.x)
                ));
        });
        #endregion

        #region DetectNearbyMineCount
        tileSpawner.activatedGameObjects.ForEach(
            (GameObject value) => value.GetComponent<TileComponent>().ResetNearbyMineCount()
            );
        #endregion

        // obstacle

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
        return size;
    }
}

   
