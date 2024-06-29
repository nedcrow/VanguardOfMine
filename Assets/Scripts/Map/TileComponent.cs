using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileComponent : MonoBehaviour
{
    public GameObject tileMesh;
    public Vector3 defaultSizeOfCollider = Vector3.one;
    public TextMeshProUGUI text;
    public EMineType mineType;
    public int idx;
    public int nearbyMineCount;
    public bool hasMine = false;
    public bool opened = false;
    public bool wasFlaged = false;

    Vector2Int tilePosition;

    void OnTriggerEnter(Collider col)
    {
        if (!col.transform.parent) return;

        CharacterCommon characterComp = col.transform.parent.GetComponent<CharacterCommon>();
        if (characterComp != null)
            DetectMine((int)characterComp.type);
    }

    public void Init(TileData tileData)
    {
        transform.tag = "Tile";

        ChangeColor(Color.white);

        idx = tileData.idx;
        hasMine = tileData.bHasMine;
        opened = tileData.bActivated;
        nearbyMineCount = tileData.nearbyMineCount;
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

    public TileData GetTileData() { return new TileData(idx,hasMine, opened,nearbyMineCount); }

    public void SetTilePosition(Vector2Int pos) {
        tilePosition = pos;
    }

    public Vector2Int GetTilePosition()
    {
        return tilePosition;
    }

    public void Flag() {
        wasFlaged = true;
    }

    public void UnFlag()
    {
        wasFlaged = false;
        // º´»ç ±ÍÈ¯ ¸í·É
    }

    public void ActiveCube() { if (tileMesh != null) tileMesh.SetActive(true); }
    public void RestCube() { if (tileMesh != null) tileMesh.SetActive(false); }

    public void DetectMine(int currentWeight)
    {
        if (opened || wasFlaged) return;

        opened = true;

        if (hasMine)
        {
            //bool isBoomWeight = currentWeight >= (int) GameManager.instance.mineLevel;
            //if (isBoomWeight) {
            //}
            ChangeColor(Color.red);
        }
        else{
            text.transform.gameObject.SetActive(true);

            int sizeX = GameManager.instance.mineMap.GetSize().x;
            int sizeY = GameManager.instance.mineMap.GetSize().y;
            int[] aroundIdxArr = GetAroundIdxArr();

            ChangeColor(Color.gray);

            if (nearbyMineCount == 0)
            {
                foreach (int weight in aroundIdxArr)
                {
                    int nearbyIdx = idx + weight;
                    if (nearbyIdx > -1 && nearbyIdx < sizeX * sizeY)
                    {
                        bool isPass = IsPassDetection(nearbyIdx);
                        if (!isPass)
                        {
                            GameManager.instance.mineMap.tileSpawner.GetTileCompAt(nearbyIdx).DetectMine(currentWeight);
                        }
                    }
                }
            }
        }
    }

    public void ChangeColor(Color color)
    {
        List<Material> mats = new List<Material>();
        mats.AddRange(tileMesh.GetComponent<MeshRenderer>().materials);
        mats[0].color = color;
        tileMesh.GetComponent<MeshRenderer>().SetMaterials(mats);
    }

    public void ResetNearbyMineCount()
    {
        Vector2Int size = GameManager.instance.mineMap.GetSize();
        int[] aroundIdxArr = GetAroundIdxArr();
        nearbyMineCount = 0;
        foreach (int weight in aroundIdxArr)
        {
            int nearbyIdx = idx + weight;

            if (nearbyIdx > -1 && nearbyIdx < size.x * size.y - 1)
            {
                bool isPass = IsPassDetection(nearbyIdx);
                if (!isPass)
                {
                    if (transform.GetComponentInParent<TileSpawner>().activatedGameObjects[nearbyIdx].GetComponent<TileComponent>().hasMine)
                    {
                        nearbyMineCount++;
                    }
                }

            }
        }

        text.transform.gameObject.SetActive(true);
        if (hasMine)
        {
            text.text = "@";
            text.color = CommonStatics.mineColor[0];
        }
        else
        {
            text.text = nearbyMineCount != 0 ? nearbyMineCount.ToString() : "";
            text.color = CommonStatics.mineColor[nearbyMineCount];
        }
        text.transform.gameObject.SetActive(false);
    }

    bool IsPassDetection(int nearbyIdx)
    {
        int sizeX = GameManager.instance.mineMap.GetSize().x;
        int sizeY = GameManager.instance.mineMap.GetSize().y;

        bool isEdgeHorizonL = idx % sizeX == 0 ? true : false;
        bool isEdgeHorizonR = (idx - sizeX + 1) % sizeX == 0 ? true : false;
        bool isEdgeVerticalT = idx < sizeX ? true : false;
        bool isEdgeVerticalD = idx < sizeX * sizeY && idx > sizeX * sizeY - sizeX ? true : false;

        bool isPass = (isEdgeHorizonL && (nearbyIdx == idx - 1 || nearbyIdx == idx - sizeX - 1 || nearbyIdx == idx + sizeX - 1))
                            || (isEdgeHorizonR && (nearbyIdx == idx + 1 || nearbyIdx == idx - sizeX + 1 || nearbyIdx == idx + sizeX + 1))
                            || (isEdgeVerticalT && (nearbyIdx == idx - sizeX || nearbyIdx == idx - sizeX - 1 || nearbyIdx == idx - sizeX + 1))
                            || (isEdgeVerticalD && (nearbyIdx == idx + sizeX || nearbyIdx == idx + sizeX - 1 || nearbyIdx == idx + sizeX + 1));

        return isPass;
    }

    int[] GetAroundIdxArr()
    {
        int sizeX = GameManager.instance.mineMap.GetSize().x;
        int sizeY = GameManager.instance.mineMap.GetSize().y;
        int[] aroundIdxArr = {
                -sizeX-1, - sizeX, -sizeX+1, // ÁÂ»ó -> ¿ì»ó
                -1, 1, // ÁÂ¿ì
                sizeX-1, sizeX, sizeX+1  // ÁÂÇÏ -> ¿ìÇÏ
                };
        return aroundIdxArr;
    }
}
