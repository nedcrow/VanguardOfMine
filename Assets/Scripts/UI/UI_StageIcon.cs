using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageIcon : MonoBehaviour
{
    public Button stageButton=null;
    [Min(1)]
    public int stageLevel = 1;

    private void Start()
    {
        if (stageButton == null)
        {
            stageButton = GetComponentInChildren<Button>();
        }
        stageButton.onClick.AddListener(StartStage);
    }

    /// <summary>
    /// 레벨에 맞는 Stage Data 불러와서 맵 생성
    /// </summary>
    public void StartStage()
    {

        GameManager.instance.mineMap.DrawMap(Vector2Int.one * (10 + stageLevel), 20);
    }
}
