using UnityEngine;

public class Level6Manager : MonoBehaviour
{
    public LevelManager levelManager;
    public PetState petState;

    // 整个面板 (拖拽 Level6_Panel)
    public GameObject uiPanel;

    // ★★★ NEW: 拖入香水瓶本身 (用来控制它的触摸感应)
    public Collider perfumeCollider;

    // ⚠️ 修改点1: 第6关的索引应该是 5 (因为从0开始数: 0,1,2,3,4,5)
    private int targetLevelIndex = 5; 
    private bool hasShown = false;

    void Update()
    {
        // 实时检查：是不是第6关？
        bool isCurrentLevel = (levelManager.currentLevelIndex == targetLevelIndex);

        // ★★★ 核心修复逻辑 ★★★
        // 如果是第6关 -> 开启碰撞体 (允许点击)
        // 如果不是 -> 关闭碰撞体 (点击会穿透过去，没反应)
        if (perfumeCollider != null)
        {
            perfumeCollider.enabled = isCurrentLevel;
        }

        // 面板的显示逻辑不需要在这里写了，交给香水瓶的点击事件去控制
        // 我们只需要确保如果玩家切走了关卡，面板能自动关掉
        if (!isCurrentLevel && uiPanel.activeSelf)
        {
            uiPanel.SetActive(false);
        }
    }

    // --- 🔘 按钮绑定的方法 (根据新文档修改) ---

    // 选项 1: 喷两下 (文档: 焦虑值+10)
    public void OnClick_SprayPerfume()
    {
        // 文档: 基本上，猫不喜欢强烈的香味... (焦虑值+10)
        // 注意：请确保 PetState 脚本里有 anxiety 这个变量，且大小写一致
        petState.anxiety += 10; 
        
        FinishLevel("Cats dislike strong scents; they find them a bit unsettling...");
    }

    // 选项 2: 不喷了 (文档: 猫猫币+1)
    public void OnClick_NoPerfume()
    {
        // 文档: 你真的很敏锐！... (猫猫币+1)
        petState.catCoin += 1;
        
        // 这是一个好选择，也可以顺便减点焦虑（可选）
        // petState.anxiety -= 5; 

        FinishLevel("Wise choice! Cats prefer natural scents.");
    }

    // (移除了第3个选项，因为文档里这一关只有两个选择)

    // --- 通用结算逻辑 ---
    void FinishLevel(string msg)
    {
        Debug.Log(msg); // 在控制台打印结果
        
        // 刷新数值显示
        petState.CheckGameStatus();
        petState.UpdateUI();

        // 关闭面板
        uiPanel.SetActive(false);

        // 🎉 强制升级到下一关
        levelManager.ForceLevelUp();
    }
}