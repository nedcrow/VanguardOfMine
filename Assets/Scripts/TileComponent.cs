using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public TMPro.TextMeshProUGUI text;
    public bool opened = false;
    public bool wasFlaged = false;

    TileData tileData;

    void OnTriggerEnter(Collider col)
    {        
        CharacterCommon characterComp = col.transform.parent.GetComponent<CharacterCommon>();
        if (characterComp != null)
            DetectMine((int)characterComp.type);
    }

    public void Init(TileData tileData)
    {
        SetTileData(tileData);
        transform.tag = "Tile";

        List<Material> mats = new List<Material>();
        mats.AddRange(tileMesh.GetComponent<MeshRenderer>().materials);
        mats[0].color = Color.white;
        tileMesh.GetComponent<MeshRenderer>().SetMaterials(mats);

        opened = false;
        wasFlaged = false;
        if (tileData.bHasMine)
        {
            text.text = "@";
            text.color = CommonStatics.mineColor[0];
        }
        else
        {
            int mineNum = tileData.nearbyMineCount;
            text.text = mineNum != 0 ? mineNum.ToString() : "";
            text.color = CommonStatics.mineColor[mineNum];
        }
        text.transform.gameObject.SetActive(false);
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

    //void OnClickTile_Left(GameObject tileObj)
    //{
    //    if(tileObj.name == gameObject.name) {

    //        // �ҳ� ���� ȣ��            
    //    }
    //}

    //void OnClickTile_Right(GameObject tileObj)
    //{
    //    if (tileObj.name == gameObject.name)
    //    {
    //        Debug.Log("right clicked " + gameObject.name);
    //        // �� ��ġ�� ��� �� ȣ��
    //    }
    //}

    //void OnClickTile_Mid(GameObject tileObj)
    //{
    //    if(tileObj.name == gameObject.name)
    //    {
    //        Debug.Log("mid clicked " + gameObject.name);
    //        // ���� ĭ�̸� �ֺ� Ž��(�ֺ� 8ĭ �� ����� ���� �̰��� ĭ�� ��� �����)
    //        // ���� ĭ�̸� �̺�Ʈ ����
    //    }
    //}

    public void DetectMine(int currentWeight)
    {
        if (opened || wasFlaged) return;

        opened = true;

        if (tileData.bHasMine)
        {
            bool isBoomWeight = currentWeight >= GameManager.instance.mineLevel;
            if (isBoomWeight) { }
            // ���� ���� �̺�Ʈ
        }
        else{
            //Debug.Log(tileData.nearbyMineCount);
            text.transform.gameObject.SetActive(true);

            int sizeX = GameManager.instance.mineMap.GetSize().x;
            int sizeY = GameManager.instance.mineMap.GetSize().y;
            int[] aroundIdxArr = {-sizeX, sizeX, -1, 1}; // �����¿�

            bool isEdgeHorizon = tileData.idx % sizeX == 0 || (tileData.idx + 1) % sizeX == 0 ? true : false;
            bool isEdgeVertical = tileData.idx % sizeY == 0 || (tileData.idx + 1) % sizeY == 0 ? true : false;
            bool isEdge = isEdgeHorizon || isEdgeVertical || tileData.idx == 0 || tileData.idx == sizeX * sizeY - 1;

            List<Material> mats = new List<Material>();
            mats.AddRange(tileMesh.GetComponent<MeshRenderer>().materials);
            mats[0].color = Color.gray;
            tileMesh.GetComponent<MeshRenderer>().SetMaterials(mats);

            if (tileData.nearbyMineCount == 0)
            {
                if (!isEdge)
                {
                    

                    foreach (int weight in aroundIdxArr)
                    {
                        int nearbyIdx = tileData.idx + weight;
                        if (nearbyIdx > -1 && nearbyIdx < sizeX * sizeY)
                        {
                            GameManager.instance.mineMap.GetTileCompAt(nearbyIdx).DetectMine(currentWeight);
                        }
                    }
                }                
            }
        }
    }
}
