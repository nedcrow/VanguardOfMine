using UnityEngine;
using System.Linq;
using System.Collections.Generic;



[ExecuteInEditMode]
public class CursorComponent : MonoBehaviour
{
    #region Event
    public delegate void Del_SelectdTile(GameObject tileObj);
    public static event Del_SelectdTile OnClickTile_Left;
    public static event Del_SelectdTile OnClickTile_Right;
    public static event Del_SelectdTile OnClickTile_Mid;
    #endregion

    public Vector3 DefaultPosition = Vector3.zero;
    public bool canSelect = true;

    [SerializeField]
    bool released_update = false;
    [SerializeField]
    bool released_lateUpdate = false;

    List<RaycastHit> hitList;
    RaycastHit hit;
    Ray ray;

    void Awake()
    {
        if (!IsDuplicated()) return;

        Init();
    }

    void Update()
    {        
        bool successedTileCasting = false;
        CastTile(ref successedTileCasting);
        if (successedTileCasting)
        {
            OnClickTile();
        };
    }

    public void HideCursor()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
    }
    public void ActiveCursor()
    {
        GetComponentInChildren<MeshRenderer>().enabled = true;
    }

    void Init()
    {
        gameObject.tag = "Cursor";

        OnClickTile_Left += ((GameObject go) => Debug.Log("click_left: " + go.name));
        OnClickTile_Right += ((GameObject go) => Debug.Log("click_right: " + go.name));
        OnClickTile_Mid += ((GameObject go) => Debug.Log("click_mid: " + go.name));

        HideCursor();
    }

    void CastTile(ref bool success)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hitList = new List<RaycastHit>();
        hitList.AddRange(Physics.RaycastAll(ray, 999, 3));

        int idx = -1;
        for(int i=0; i<hitList.Count; i++)
        {
            if (hitList[i].collider.gameObject.CompareTag("Tile"))
            {
                idx = i;
                hit = hitList[idx];
                break;
            }
        }

        if (idx < 0)
        {
            HideCursor();
            success = false;
        }
        else
        {
            ActiveCursor();
            transform.position = hitList[idx].collider.gameObject.transform.position;
            success = true;
        }
    }

    void OnClickTile()
    {
        bool releasedLeft = Input.GetMouseButtonUp(1) && Input.GetMouseButton(0);
        bool releasedRight = Input.GetMouseButtonUp(0) && Input.GetMouseButton(1);
        bool releasedMid = Input.GetMouseButton(0) && Input.GetMouseButton(1);

        if (releasedLeft || releasedRight || releasedMid)
        {
            released_update = true;
        }

        released_lateUpdate = Input.GetMouseButton(0) || Input.GetMouseButton(1);
        if (!released_lateUpdate)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (released_update)
                {
                    OnClickTile_Mid(hit.collider.gameObject);
                    released_update = false;
                }
                else
                {
                    OnClickTile_Left(hit.collider.gameObject);
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                if (released_update)
                {
                    OnClickTile_Mid(hit.collider.gameObject);
                    released_update = false;
                }
                else
                {
                    OnClickTile_Right(hit.collider.gameObject);
                }
            }
            else if (Input.GetMouseButtonUp(2))
            {
                OnClickTile_Mid(hit.collider.gameObject);
            }
        }
    }

    bool IsDuplicated()
    {
        int countOfCursor = GameObject.FindGameObjectsWithTag("Cursor").Length;
        if (countOfCursor > 0) return true;
        Debug.Log("To much cursor this count is " + countOfCursor);
        return false;
    }

    
}
