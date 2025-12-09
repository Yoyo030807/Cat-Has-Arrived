using UnityEngine;

public class Level7Manager : MonoBehaviour
{
    public LevelManager levelManager;
    public PetState petState;
    public GameObject uiPanel;

    // ★★★ NEW: 拖入场景里的 Backpack 物体，控制它的触摸开关
    public Collider bagCollider;

    // 第7关索引 = 6
    private int targetLevelIndex = 6; 

    void Update()
    {
        // 1. 检查是不是第7关
        bool isCurrentLevel = (levelManager.currentLevelIndex == targetLevelIndex);

        // 2. 控制猫包能不能被点击
        if (bagCollider != null)
        {
            bagCollider.enabled = isCurrentLevel;
        }

        // 3. 如果切关了，自动把面板关掉
        if (!isCurrentLevel && uiPanel.activeSelf)
        {
            uiPanel.SetActive(false);
        }
    }

    // --- 选项逻辑 (根据你的脚本) ---

    // 选项1: 透明包 (Anxiety +10)
    public void OnClick_TransparentBag()
    {
        petState.anxiety += 10;
        // 脚本来源: [cite: 10]
        FinishLevel("Don't buy! Transparent bags make cats feel insecure and hot.");
    }

    // 选项2: 布艺包 (Anxiety -10)
    public void OnClick_ClothBag()
    {
        petState.anxiety -= 10;
        // 脚本来源: [cite: 12]
        FinishLevel("Great choice! It's spacious, breathable, and safe for short trips.");
    }

    void FinishLevel(string msg)
    {
        Debug.Log(msg);
        petState.CheckGameStatus();
        petState.UpdateUI();
        uiPanel.SetActive(false);
        levelManager.ForceLevelUp();
    }
}