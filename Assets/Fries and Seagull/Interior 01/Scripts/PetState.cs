using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PetState : MonoBehaviour
{
    // 1. 定义四个核心数值
    public int health = 100;
    public int hunger = 50;
    public int anxiety = 0;
    public int catCoin = 0;

    // 2. 定义 UI 文本引用
    [Header("请把 UI Text 拖到这里")]
    public TMP_Text healthText;
    public TMP_Text hungerText;
    public TMP_Text anxietyText;
    public TMP_Text coinText;

    // ★★★ 新增：游戏结束标记 ★★★
    // 如果变成 true，玩家就不能再点击按钮了
    private bool isGameOver = false;

    // 3. 初始化
    void Start()
    {
        // 重置所有状态
        health = 100;
        hunger = 50;
        anxiety = 0;
        catCoin = 0;
        isGameOver = false; // 确保游戏开始时是活着的

        UpdateUI();
    }

    // 4. 刷新界面
    public void UpdateUI()
    {
        // 如果游戏已经结束了，显示特殊文字，然后不再刷新数值
        if (isGameOver)
        {
            // 这里可以写一些逻辑，比如把文字变红，或者显示 Game Over
            // healthText.text = "GAME OVER"; 
            return;
        }

        healthText.text = "Health: " + health;
        hungerText.text = "Satiety meter: " + hunger;
        anxietyText.text = "Anxiety: " + anxiety;
        coinText.text = "Coin: " + catCoin;
    }

    // ★★★ 新增：检查数值边界和游戏结束条件 ★★★
    // 每次数值变化后，都必须调用这个方法！
    public void CheckGameStatus()
    {
        // 1. 数值限制 (Clamp)
        // 焦虑值不能小于 0
        if (anxiety < 0) anxiety = 0;

        // (可选) 饱食度和健康值也不能无限大吧？比如最大100？
        if (hunger > 100) hunger = 100;
        if (health > 100) health = 100;

        // 2. 游戏结束判定 (Game Over)
        // 逻辑：健康<0  或者  饱食度<0  或者  焦虑>100
        if (health < 0 || hunger < 0 || anxiety > 100)
        {
            GameOver();
        }
    }

    // ★★★ 新增：执行游戏结束 ★★★
    void GameOver()
    {
        isGameOver = true;
        Debug.LogError("Game over! Your cat has died or run away.");

        // 修改 UI 提示玩家
        // 你可以找一个 Text 显示 "Game Over"，这里暂时借用 Coin 的位置显示
        coinText.text = "Your Cat Dead";
        coinText.color = Color.red; // 把字变红

        // (可选) 冻结时间，让游戏暂停
        // Time.timeScale = 0; 
    }

    // ---------------------------------------------------------
    // 按钮功能函数
    // ---------------------------------------------------------

    public void OnEatCostMoney()
    {
        // ★★★ 如果游戏结束，禁止操作 ★★★
        if (isGameOver) return;

        hunger += 10;
        catCoin -= 1;

        if (catCoin < 0) catCoin = 0;

        // ★★★ 关键：每次改完数值，都要检查一下状态 ★★★
        CheckGameStatus();

        UpdateUI();
        Debug.Log("How can you charge me for feeding you?");
    }

    public void OnEatEarnMoney()
    {
        // ★★★ 如果游戏结束，禁止操作 ★★★
        if (isGameOver) return;

        hunger += 10;
        catCoin += 1;

        // ★★★ 关键：每次改完数值，都要检查一下状态 ★★★
        CheckGameStatus();

        UpdateUI();
        Debug.Log("Wow, feeding kittens will earn new coins!");
    }

    // 在 PetState 类里面添加这两个方法

    // 玩法 A：逗猫棒 (第3关用)
    public void OnPlayWithTeaser()
    {
        // 1. 改数值
        anxiety -= 20; // 假设一次减20，效果明显点
        catCoin += 1;

        // 2. 检查状态 (防负数 + 判死活)
        CheckGameStatus();

        // 3. 刷新 UI
        UpdateUI();

        Debug.Log("Happy! Playing with the cat teaser stick reduces anxiety.");

        // ★★★ 注意：这里不需要调用 CheckLevelTarget() ★★★
        // LevelManager 会自己在每一帧检查 anxiety 是不是小于 50 了
    }

    // 玩法 B：强行抱抱 (第3关用)
    public void OnForceHug()
    {
        // 1. 改数值
        anxiety += 20; // 吓到了
        catCoin -= 1;  // 假设扣钱

        // 2. 检查状态
        CheckGameStatus();

        // 3. 刷新 UI
        UpdateUI();

        Debug.Log("This love is too heavy! The cat ran away, and anxiety increased.");
    }
}