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
        // ���� ����
        CharacterCommon characterComp = col.GetComponent<CharacterCommon>();
        if(characterComp != null)
        {
            bool isBoomWeight = (int)characterComp.type >= GameManager.instance.mineLevel;
            if (tileData.bHasMine && isBoomWeight)
            {
                // ���� ���� �̺�Ʈ
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
        // ���� ��ȯ ���
    }

    public void ActiveCube() { if (tileMesh != null) tileMesh.SetActive(true); }
    public void RestCube() { if (tileMesh != null) tileMesh.SetActive(false); }

    void OnClickTile_Left(GameObject tileObj)
    {
        if(tileObj.name == gameObject.name) {
            Debug.Log("left clicked " + gameObject.name);
            // �ҳ� ���� ȣ��
        }
    }

    void OnClickTile_Right(GameObject tileObj)
    {
        if (tileObj.name == gameObject.name)
        {
            Debug.Log("right clicked " + gameObject.name);
            // �� ��ġ�� ��� �� ȣ��
        }
    }

    void OnClickTile_Mid(GameObject tileObj)
    {
        if(tileObj.name == gameObject.name)
        {
            Debug.Log("mid clicked " + gameObject.name);
            // ���� ĭ�̸� �ֺ� Ž��(�ֺ� 8ĭ �� ����� ���� �̰��� ĭ�� ��� �����)
            // ���� ĭ�̸� �̺�Ʈ ����
        }
    }

    // ���� üĿ
}
