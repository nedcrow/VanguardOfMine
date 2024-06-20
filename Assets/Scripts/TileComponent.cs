using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    public TileData(
        int idx,
        bool bGotMine,
        bool bActivated,
        int nearByMineCount
    )
    {
        this.idx = idx;
        this.bHasMine = bGotMine;
        this.bActivated = bActivated;
        this.nearbyMineCount = nearByMineCount;
    }

    public int idx;
    public bool bHasMine;
    public bool bActivated;
    public int nearbyMineCount;
}

public class TileComponent : MonoBehaviour
{
    public GameObject tileMesh;
    public Vector3 defaultSizeOfCollider = Vector3.one;

    TileData tileData;

    bool wasFlaged = false;

    void OnTriggerEnter(Collider col)
    {
        // 지뢰 폭발
        CharacterCommon characterComp = col.GetComponent<CharacterCommon>();
        if(characterComp != null)
        {
            bool isBoomWeight = (int)characterComp.type >= GameManager.instance.mineLevel;
            if (tileData.bHasMine && isBoomWeight)
            {
                // 범위 폭발 이벤트
            }
        }
    }

    public void Init(TileData tileData)
    {
        SetTileData(tileData);
        transform.tag = "Tile";

        CursorComponent.OnClickTile_Left += OnClickTile_Left;
        CursorComponent.OnClickTile_Right += OnClickTile_Right;
        CursorComponent.OnClickTile_Mid += OnClickTile_Mid;

        wasFlaged = false;
    }

    public void SetTileData(TileData tileData) { this.tileData = tileData; }
    public TileData GetTileData() { return tileData; }

    public void Flag() {
        wasFlaged = true;
    }

    public void UnFlag()
    {
        wasFlaged = false;
        // 병사 귀환 명령
    }

    public void ActiveCube() { if (tileMesh != null) tileMesh.SetActive(true); }
    public void RestCube() { if (tileMesh != null) tileMesh.SetActive(false); }

    void OnClickTile_Left(GameObject tileObj)
    {
        if(tileObj.name == gameObject.name) {
            Debug.Log("left clicked " + gameObject.name);
            // 소나 병사 호출
        }
    }

    void OnClickTile_Right(GameObject tileObj)
    {
        if (tileObj.name == gameObject.name)
        {
            Debug.Log("right clicked " + gameObject.name);
            // 내 위치에 깃발 병 호출
        }
    }

    void OnClickTile_Mid(GameObject tileObj)
    {
        if(tileObj.name == gameObject.name)
        {
            Debug.Log("mid clicked " + gameObject.name);
            // 열린 칸이면 주변 탐색(주변 8칸 중 깃발이 없는 미공개 칸을 모두 열어라)
            // 닫힌 칸이면 이벤트 없음
        }
    }

    // 병사 체커
}
