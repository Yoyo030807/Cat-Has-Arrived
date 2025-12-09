using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    [Header("引用")]
    public PetState petState;
    public LevelManager levelManager; // 确保这里是 LevelManager (带L)

    [Header("UI 按钮 (World Space)")]
    public GameObject buttonForceHug;   // 悬浮在猫头顶的按钮
    public GameObject buttonPlayTeaser; // 悬浮在逗猫棒旁的按钮

    private int targetLevelIndex = 2; // 第三关 (索引2)

    void Start()
    {
        HideAllButtons();
    }

    // --- 第一阶段：接收点击信号 ---

    // ★★★ 修正点：名字改回 ShowHugOption 以匹配你的调用脚本 ★★★
    public void ShowHugOption()
    {
        // 检查是不是第3关
        if (levelManager.currentLevelIndex != targetLevelIndex) return;

        HideAllButtons();
        if (buttonForceHug != null) buttonForceHug.SetActive(true);
    }

    // ★★★ 修正点：名字改回 ShowPlayOption ★★★
    public void ShowPlayOption()
    {
        if (levelManager.currentLevelIndex != targetLevelIndex) return;

        HideAllButtons();
        if (buttonPlayTeaser != null) buttonPlayTeaser.SetActive(true);
    }

    // --- 第二阶段：点击 UI 按钮执行逻辑 ---

    public void OnClick_ForceHug()
    {
        petState.anxiety += 20;
        petState.catCoin -= 1;
        FinishAction("You picked up the cat against its will, and it's furious!");
    }

    public void OnClick_UseTeaser()
    {
        petState.anxiety -= 20;
        petState.catCoin += 1;
        FinishAction("You played with the cat for a while, and it was very happy!");
    }

    void FinishAction(string msg)
    {
        Debug.Log(msg);
        petState.CheckGameStatus(); // 检查状态
        petState.UpdateUI();        // 刷新界面
        HideAllButtons();           // 隐藏按钮

        // 👇 加上这一行！告诉总导演进入下一关
        levelManager.ForceLevelUp(); 
    }

    public void HideAllButtons()
    {
        if (buttonForceHug != null) buttonForceHug.SetActive(false);
        if (buttonPlayTeaser != null) buttonPlayTeaser.SetActive(false);
    }

    void Update()
    {
        // 右键取消显示
        if (Input.GetMouseButtonDown(1)) HideAllButtons();
    }
}