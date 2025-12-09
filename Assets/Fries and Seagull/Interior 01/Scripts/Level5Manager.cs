using UnityEngine;

public class Level5Manager : MonoBehaviour
{
    public LevelManager levelManager;
    public PetState petState;

    // 整个面板
    public GameObject uiPanel;

    private int targetLevelIndex = 4; // 第5关 (索引4)
    private bool hasShown = false;

    void Update()
    {
        // 如果到了第5关，且还没显示过面板，就显示出来
        if (levelManager.currentLevelIndex == targetLevelIndex)
        {
            if (!uiPanel.activeSelf)
            {
                uiPanel.SetActive(true);
            }
        }
        else
        {
            // 不是这一关，强制隐藏
            if (uiPanel.activeSelf) uiPanel.SetActive(false);
        }
    }

    // --- 按钮绑定的方法 ---

    // 选项 1: 去医院
    public void OnClick_Hospital()
    {
        petState.catCoin += 2;
        petState.health += 10;
        FinishLevel("Going to the hospital is a safe bet!");
    }

    // 选项 2: 滴在毛上 (错误)
    public void OnClick_DropOnFur()
    {
        petState.catCoin -= 1;
        petState.health -= 10;
        Debug.Log("Dropping it on the fur is useless—the medicine evaporates...");
        // 这里不调用 FinishLevel，让玩家重选
        // 或者你可以扣点属性作为惩罚
    }

    // 选项 3: 滴在皮肤上
    public void OnClick_DropOnSkin()
    {
        petState.catCoin += 2;
        petState.health += 10;
        FinishLevel("Correct operation! Saves money and works effectively!");
    }

    void FinishLevel(string msg)
    {
        Debug.Log(msg);
        petState.CheckGameStatus();
        petState.UpdateUI();

        // 关闭面板
        uiPanel.SetActive(false);

        // 强制通关
        levelManager.ForceLevelUp();
    }
}