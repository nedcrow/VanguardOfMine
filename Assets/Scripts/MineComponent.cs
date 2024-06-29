using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineComponent : MonoBehaviour
{
    [SerializeField]
    MineData mineData;
    [SerializeField]
    Vector2Int minePosition;

    List<GameObject> soldierListInRange = new List<GameObject>();

    public void Init(MineData mineData)
    {
        this.mineData = mineData;
        name = "mine_" + mineData.idx;
        soldierListInRange = new List<GameObject>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (!col.transform.parent) return;
        CharacterCommon characterComp = col.transform.parent.GetComponent<CharacterCommon>();
        if (characterComp != null && !soldierListInRange.Contains(characterComp.gameObject))
        {
            soldierListInRange.Add(characterComp.gameObject);
            if(characterComp.GetCharacterPosition() == minePosition && (int) characterComp.type >= (int)mineData.mineType)
            {
                StartCoroutine(Boom());
            }
            //Debug.LogWarning("Add : " + characterComp.name);
        }
    }

    public int GetIndex() {
        return mineData != null ? mineData.idx : -1;
    }

    public EMineType GetMineType()
    {
        return mineData != null ? mineData.mineType : EMineType.normal;
    }
    public void SetMinePosition(Vector2Int pos)
    {
        minePosition = pos;
    }

    public Vector2Int GetMinePosition()
    {
        return minePosition;
    }

    public IEnumerator Boom()
    {
        yield return new WaitForSeconds(0.6f);

        if(soldierListInRange.Count > 0)
        {
            soldierListInRange.ForEach((GameObject soldier) => {
                soldier.GetComponentInParent<SoldierSpawner>().RIP_For(soldier);
            });
        }
    }
}
