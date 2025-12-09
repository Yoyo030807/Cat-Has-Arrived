using UnityEngine;

public class Level8Manager : MonoBehaviour
{
    public LevelManager levelManager;
    public PetState petState;
    public GameObject uiPanel;

    // ★★★ NEW 1: 窗户的点击锁 (像之前一样)
    public Collider windowCollider; 

    // ★★★ NEW 2: 那个做好的纱窗物体
    public GameObject safetyMesh; 

    // 第8关索引 = 7
    private int targetLevelIndex = 7; 

    void Update()
    {
        // 1. 检查是不是第8关
        bool isCurrentLevel = (levelManager.currentLevelIndex == targetLevelIndex);

        // 2. 只有这一关，窗户才能点
        if (windowCollider != null)
        {
            windowCollider.enabled = isCurrentLevel;
        }

        // 3. 切关自动关面板
        if (!isCurrentLevel && uiPanel.activeSelf)
        {
            uiPanel.SetActive(false);
        }
    }

    // --- 选项逻辑 ---

    // 选项1: 立马封窗 (正确)
    public void OnClick_InstallMesh()
    {
        // 文档: 做得好！养猫一定要封窗... (Health +10) [cite: 17]
        petState.health += 10;

        // ★★★ 核心特效：把纱窗显示出来！ ★★★
        if (safetyMesh != null)
        {
            safetyMesh.SetActive(true);
        }

        FinishLevel("Safety First! Window mesh installed successfully.");
    }

    // 选项2: 侥幸心理 (错误/BE)
    public void OnClick_Ignore()
    {
        // 文档: 达成BE... (Health -100) [cite: 19]
        petState.health -= 100; 

        // 这一步之后 PetState 会处理 Game Over，所以我们不用在这里写太多
        Debug.Log("Bad End: The cat fell...");
        
        // 关闭面板
        uiPanel.SetActive(false);
    }

    void FinishLevel(string msg)
    {
        Debug.Log(msg);
        petState.CheckGameStatus();
        petState.UpdateUI();
        uiPanel.SetActive(false);
        
        // 只有猫活着才升级
        if (petState.health > 0)
        {
            levelManager.ForceLevelUp();
        }
    }
}