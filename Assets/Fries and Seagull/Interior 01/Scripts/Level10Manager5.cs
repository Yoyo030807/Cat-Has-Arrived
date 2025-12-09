using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.AI; // 引入导航命名空间

public class Level10Manager : MonoBehaviour
{
    public LevelManager levelManager;
    public PetState petState;
    public GameObject uiPanel;
    public TMP_Text questionText; // 用来显示结果文字

    [Header("Actors")]
    public GameObject catObject;       // 猫咪本体
    public GameObject mouseInMouth;    // 嘴里的老鼠
    public GameObject mouseOnFloor;    // 地上的老鼠
    
    [Header("Stage")]
    public Transform startPos;         // 猫出发的地方 (比如门口)
    public Transform endPos;           // 猫停下的地方 (镜头前)
    public Animator catAnimator;       // 猫的动画

    // 第10关索引 = 9
    private int targetLevelIndex = 9;
    private bool hasStartedScene = false;

    void Update()
    {
        if (levelManager.currentLevelIndex == targetLevelIndex)
        {
            // 刚进关卡那一瞬间，开始表演
            if (!hasStartedScene)
            {
                StartCoroutine(PlaySequence());
                hasStartedScene = true;
            }
        }
        else
        {
            // 不在这一关时重置
            hasStartedScene = false;
            if (uiPanel.activeSelf) uiPanel.SetActive(false);
            if (mouseInMouth != null) mouseInMouth.SetActive(false); // 平时嘴里没老鼠
            if (mouseOnFloor != null) mouseOnFloor.SetActive(false); // 平时地上没老鼠
        }
    }

    // 🎬 核心表演协程：像拍电影一样按时间顺序执行
    IEnumerator PlaySequence()
    {
        // 1. 准备阶段
        mouseInMouth.SetActive(true);  // 嘴里出现老鼠
        mouseOnFloor.SetActive(false); // 地上清空
        
        // 瞬移猫到出发点
        NavMeshAgent agent = catObject.GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false; // 关导航
        catObject.transform.position = startPos.position;
        catObject.transform.LookAt(endPos); // 面朝目标
        
        // 播放走路动画
        if (catAnimator != null) catAnimator.SetBool("isWalking", true);

        // 2. 移动阶段 (简单的插值移动)
        float duration = 3.0f; // 走3秒钟
        float timer = 0;
        
        while (timer < duration)
        {
            timer += Time.deltaTime;
            // 平滑移动猫咪
            catObject.transform.position = Vector3.Lerp(startPos.position, endPos.position, timer / duration);
            yield return null; // 等待下一帧
        }

        // 3. 到达阶段
        if (catAnimator != null) catAnimator.SetBool("isWalking", false);
        
        // ★ 魔术时间：放下老鼠
        mouseInMouth.SetActive(false); // 嘴里的消失
        mouseOnFloor.SetActive(true);  // 地上的出现

        // 4. 吓人阶段 (弹出面板)
        yield return new WaitForSeconds(0.5f); // 停顿一下
        uiPanel.SetActive(true); // 弹出选项：“啊！老鼠！”
    }

    // --- 选项逻辑 ---

    public void OnClick_Scold()
    {
        petState.catCoin -= 1;
        petState.anxiety += 10;
        ShowResult("The cat looks sad. It just wanted to share its 'prey' with you...");
        // 这里可以加一个猫咪耳朵耷拉下来的动画
    }

    public void OnClick_Praise()
    {
        petState.catCoin += 1;
        petState.anxiety -= 10;
        ShowResult("You understood! It's a gift of love.");
        // 这里可以加一个猫咪蹭人的动画
    }

    void ShowResult(string msg)
    {
        if (questionText != null) questionText.text = msg;
        StartCoroutine(WaitAndFinish());
    }

    IEnumerator WaitAndFinish()
    {
        yield return new WaitForSeconds(3.0f);
        uiPanel.SetActive(false);
        // 这里可以跳转到结局画面，或者显示“通关”
        levelManager.ForceLevelUp(); 
    }
}