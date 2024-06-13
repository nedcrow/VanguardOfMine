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
    TileData tileData;
    public void SetTileData(TileData tileData) { this.tileData = tileData; }
    public TileData GetTileData() { return tileData; }

    bool wasFlaged = false;

    private void OnTriggerEnter(Collider col)
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

        Vector3 sizeOfCollider = GetComponent<BoxCollider>().size;
        GetComponent<BoxCollider>().size = new Vector3(
            sizeOfCollider.x * transform.localScale.x,
            sizeOfCollider.y * transform.localScale.y,
            sizeOfCollider.z * transform.localScale.z
            );

        wasFlaged = false;
    }

    public void Flag() {
        wasFlaged = true;
    }

    public void UnFlag()
    {
        wasFlaged = false;
        // ���� ��ȯ ���
    }

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
