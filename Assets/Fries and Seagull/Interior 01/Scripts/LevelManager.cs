using UnityEngine;
using TMPro;
using System.Collections.Generic; // 必须引用这个，才能用 List

// 定义一个枚举，列出所有的属性类型
public enum TargetStatType
{
    Hunger,     // 饱食度
    CatCoin,    // 金币
    Anxiety,    // 焦虑值
    Health      // 健康值
}

// 定义每一关的“配置单”
// [System.Serializable] 这一行非常关键，加上它才能在 Unity 面板里编辑
[System.Serializable]
public class LevelData
{
    [Header("Level Description")]
    public string description;     // 例如："任务：把猫喂到100饱食度"

    [Header("Clearance Objective")]
    public TargetStatType targetType; // 这一关看哪个属性？
    public int targetValue;           // 目标数值是多少？
    public bool isGreaterThan;       

    // ★★★ NEW: 新增这一行！ ★★★
    [Header("Camera Setting")]
    public Transform cameraPosition; // 这一关，摄像机要去哪里？
}

public class LevelManager : MonoBehaviour
{
    public PetState petState;
    public TMP_Text taskText;

    public GameObject bowlUI;
    // ★★★ 新增：拖入碗的交互脚本所在的物体 (bowl03)，我们要禁用它的点击 ★★★
    public GameObject bowlObject;
    [Header("Configure your 30 levels here")]
    public List<LevelData> allLevels; // 这就是你的关卡列表

    public int currentLevelIndex = 0; // 0 代表第一关，1 代表第二关...

    void Start()
    {
        UpdateTaskUI();
        // ★★★ NEW: 游戏开始时，先移动到第1关的视角 ★★★
        MoveCameraForCurrentLevel();
    }

    void Update()
    {
        // 如果还有关卡没打完，就继续检查
        if (currentLevelIndex < allLevels.Count)
        {
            CheckCurrentLevel();
        }
    }

    void CheckCurrentLevel()
    {
        // 1. 获取当前关卡的配置数据
        LevelData currentLevelData = allLevels[currentLevelIndex];
        bool isLevelComplete = false;

        // 2. 根据这一关设定的类型，去检查对应的数值
        switch (currentLevelData.targetType)
        {
            case TargetStatType.Hunger:
                isLevelComplete = CheckValue(petState.hunger, currentLevelData);
                break;
            case TargetStatType.CatCoin:
                isLevelComplete = CheckValue(petState.catCoin, currentLevelData);
                break;
            case TargetStatType.Anxiety:
                isLevelComplete = CheckValue(petState.anxiety, currentLevelData);
                break;
            case TargetStatType.Health:
                isLevelComplete = CheckValue(petState.health, currentLevelData);
                break;
        }

        // 3. 如果达标了，升级
        if (isLevelComplete)
        {
            LevelUp();
        }
    }

    // 一个通用的数值比较工具
    bool CheckValue(float currentValue, LevelData data)
    {
        if (data.isGreaterThan)
        {
            // 比如：饱食度 >= 100
            return currentValue >= data.targetValue;
        }
        else
        {
            // 比如：焦虑值 <= 0
            return currentValue <= data.targetValue;
        }
    }

    void LevelUp()
    {
        currentLevelIndex++; // 索引 +1，进入下一关

        // 可选：播放音效
        Debug.Log("Congratulations! Prepare to enter level " + (currentLevelIndex + 1));

        // 可选：如果你希望每一关开始时清空金币或重置饥饿，可以在这里写
        // if (currentLevelIndex < allLevels.Count) { petStats.catCoin = 0; petStats.UpdateUI(); }
        if (currentLevelIndex == 1)
        {
            if (bowlUI != null) bowlUI.SetActive(false); // 关掉界面

            // 可选：把碗的点击脚本也关掉，防止玩家再点开
            if (bowlObject != null)
            {
                var interaction = bowlObject.GetComponent<BowlInteraction>();
                if (interaction != null) interaction.enabled = false;
            }
        }
        UpdateTaskUI();

        // ★★★ NEW: 升级后，立刻指挥摄像机移动 ★★★
        MoveCameraForCurrentLevel();
    }

    // ★★★ NEW: 新增这个辅助方法 ★★★
    void MoveCameraForCurrentLevel()
    {
        // 确保索引没有超标
        if (currentLevelIndex < allLevels.Count)
        {
            // 拿到这一关的数据
            LevelData data = allLevels[currentLevelIndex];
            
            // 如果你在面板里拖拽了机位，就让 CameraManager 移动过去
            if (data.cameraPosition != null)
            {
                CameraManager.Instance.MoveToTarget(data.cameraPosition);
            }
        }
    }

    void UpdateTaskUI()
    {
        // 检查是否所有关卡都打完了
        if (currentLevelIndex >= allLevels.Count)
        {
            taskText.text = "Congratulations!\nYou have completed all " + allLevels.Count + " levels!\nThis cat is now very happy!";
            return;
        }

        // 显示当前关卡的描述
        // (currentLevelIndex + 1) 是为了让玩家看到 "第1关" 而不是 "第0关"
        LevelData data = allLevels[currentLevelIndex];
        taskText.text = $"Level {currentLevelIndex + 1}/{allLevels.Count}:\n{data.description}";
    }
    // ... 在 LeverManager 类里面添加这个方法 ...

    public int currentLevelIndexPublic { get { return currentLevelIndex; } } // 让外部能读取当前关卡数(如果变量是private的话)

    // 强制升级 (给特殊关卡用的后门)
    public void ForceLevelUp()
    {
        LevelUp();
    }
}