using UnityEngine;

public class LitterBoxPuzzle : MonoBehaviour
{
    [Header("引用")]
    public LevelManager levelManager;
    public PetState petState;
    public GameObject realLitterBox;

    [Header("高亮光圈 (请确保顺序正确)")]
    // Element 0: SpotA (正确位置)
    // Element 1: SpotB
    // Element 2: SpotC
    public GameObject[] spots;

    private bool isPlaced = false;
    private int targetLevelIndex = 1; // 代表第二关

    void Update()
    {
        if (levelManager == null) return;

        // 检查是否是第二关
        if (levelManager.currentLevelIndex != targetLevelIndex)
        {
            SetPuzzleActive(false);
            return;
        }

        // 如果是第二关且未放置，显示光圈
        if (!isPlaced)
        {
            SetPuzzleActive(true);
        }
    }

    void SetPuzzleActive(bool isActive)
    {
        foreach (var spot in spots)
        {
            if (spot != null) spot.SetActive(isActive);
        }
    }

    // --- 核心点击逻辑 ---
    public void OnSpotClicked(int spotIndex)
    {
        if (isPlaced) return;

        // 1. 放置猫砂盆
        SetPuzzleActive(false);
        realLitterBox.SetActive(true);
        realLitterBox.transform.position = spots[spotIndex].transform.position;
        isPlaced = true;

        // 2. 根据位置执行不同的加减分逻辑
        if (spotIndex == 0) // Spot A (正确位置)
        {
            // 需求：Anxiety +10, Health +10, Coin +10
            petState.anxiety -= 10;
            petState.health += 10;
            petState.catCoin += 1; // 你的变量名叫 catCoin

            Debug.Log("Spot A: Perfect! All stats increased. Clear!");

            // 刷新 UI (这一步很关键，否则你看不到变化)
            petState.UpdateUI();

            // 强制通关
            levelManager.ForceLevelUp();
        }
        else if (spotIndex == 1) // Spot B (错误)
        {
            // 需求：Anxiety -10, Coin -1
            petState.anxiety += 10;
            petState.catCoin -= 1;
            petState.CheckGameStatus();

            Debug.Log("Spot B: Wrong. Anxiety decreased, money deducted.");
            petState.UpdateUI(); // 刷新 UI
        }
        else if (spotIndex == 2) // Spot C (错误)
        {
            // 需求：Health -10, Coin -1
            petState.anxiety += 10;
            petState.catCoin -= 1;
            petState.CheckGameStatus();

            Debug.Log("Spot C: Wrong. Health decreased, money deducted.");
            petState.UpdateUI(); // 刷新 UI
        }
    }

    // --- 反悔逻辑 ---
    public void OnRealBoxClicked()
    {
        // 只有在第二关，且已经放置了，才能反悔重新选
        if (levelManager.currentLevelIndex == targetLevelIndex && isPlaced)
        {
            realLitterBox.SetActive(false);
            SetPuzzleActive(true);
            isPlaced = false;
            Debug.Log("Reselect location...");
        }
    }
}