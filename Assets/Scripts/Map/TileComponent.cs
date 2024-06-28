using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileComponent : MonoBehaviour
{
    public GameObject tileMesh;
    public Vector3 defaultSizeOfCollider = Vector3.one;
    public TextMeshProUGUI text;
    public EMineType mineType;
    public bool opened = false;
    public bool wasFlaged = false;

    TileData tileData;
    Vector2Int tilePosition;

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

        ChangeColor(Color.white);

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

        if (tileData.bHasMine)
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
            int[] aroundIdxArr = {
                -sizeX-1, - sizeX, -sizeX+1, // ÁÂ»ó -> ¿ì»ó
                -1, 1, // ÁÂ¿ì
                sizeX-1, sizeX, sizeX+1  // ÁÂÇÏ -> ¿ìÇÏ
                };

            bool isEdgeHorizonL = tileData.idx % sizeX == 0 ? true : false;
            bool isEdgeHorizonR = (tileData.idx - sizeX + 1) % sizeX == 0 ? true : false;
            bool isEdgeVerticalT = tileData.idx < sizeX ? true : false;
            bool isEdgeVerticalD = tileData.idx < sizeX * sizeY && tileData.idx > sizeX * sizeY - sizeX ? true : false;

            ChangeColor(Color.gray);

            if (tileData.nearbyMineCount == 0)
            {
                foreach (int weight in aroundIdxArr)
                {
                    int nearbyIdx = tileData.idx + weight;
                    if (nearbyIdx > -1 && nearbyIdx < sizeX * sizeY)
                    {
                        bool isPass = (isEdgeHorizonL && (nearbyIdx == tileData.idx - 1 || nearbyIdx == tileData.idx - sizeX - 1 || nearbyIdx == tileData.idx + sizeX - 1))
                            || (isEdgeHorizonR && (nearbyIdx == tileData.idx + 1 || nearbyIdx == tileData.idx - sizeX + 1 || nearbyIdx == tileData.idx + sizeX + 1))
                            || (isEdgeVerticalT && (nearbyIdx == tileData.idx - sizeX || nearbyIdx == tileData.idx - sizeX - 1 || nearbyIdx == tileData.idx - sizeX + 1))
                            || (isEdgeVerticalD && (nearbyIdx == tileData.idx + sizeX || nearbyIdx == tileData.idx + sizeX - 1 || nearbyIdx == tileData.idx + sizeX + 1));
                        if (!isPass)
                        {
                            GameManager.instance.mineMap.GetTileCompAt(nearbyIdx).DetectMine(currentWeight);
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
}
