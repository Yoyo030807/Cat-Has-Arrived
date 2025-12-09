using UnityEngine;
using UnityEngine.AI;
public class Level9Manager : MonoBehaviour
{
    public LevelManager levelManager;
    public PetState petState;
    public GameObject uiPanel;

    [Header("Stage Settings")]
    public GameObject catObject;       // 你的猫咪物体
    public Transform catEatPosition;   // 刚才创建的 CatPos_Eating 空物体
    public Collider catCollider;       // 猫身上的 Box Collider (用来控制能不能点)

    [Header("Animation (Optional)")]
    public Animator catAnimator;       // 猫身上的动画控制器 (如果没有先空着)

    // 第9关索引 = 8
    private int targetLevelIndex = 8;
    private bool hasSetupScene = false; // 用来保证只瞬移一次

    void Update()
    {
        bool isCurrentLevel = (levelManager.currentLevelIndex == targetLevelIndex);

        if (isCurrentLevel)
        {
            // 1. 进关卡的第一帧：瞬移猫咪 + 播放动作
            if (!hasSetupScene)
            {
                SetupEatingScene();
                hasSetupScene = true;
            }

            // 2. 开启猫咪的点击功能
            if (catCollider != null) catCollider.enabled = true;
        }
        else
        {
            // 不在这一关时，重置状态，关掉猫咪点击
            hasSetupScene = false;
            if (catCollider != null) catCollider.enabled = false;
            if (uiPanel.activeSelf) uiPanel.SetActive(false);
        }
    }

    void SetupEatingScene()
    {
        if (catObject != null && catEatPosition != null)
        {
            Debug.Log("正在尝试移动猫咪..."); // 调试信息

            // 1. 检查有没有导航组件
            UnityEngine.AI.NavMeshAgent agent = catObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

            // 2. 如果有导航，必须先关掉它！(否则它会把猫拉回原地)
            if (agent != null) 
            {
                agent.enabled = false; 
            }

            // 3. 执行瞬移
            catObject.transform.position = catEatPosition.position;
            catObject.transform.rotation = catEatPosition.rotation;

            Debug.Log("猫咪应该已经到了碗边：" + catEatPosition.position);

            // 4. (可选) 如果你希望它之后还能走动，可以在这里重新打开 agent.enabled = true;
            // 但因为这是吃饭动画，建议就先关着，防止它乱跑
        }
        else
        {
            Debug.LogError("导演！你忘了在 Inspector 里把猫或位置拖进脚本了！");
        }

        // 播放动画
        if (catAnimator != null)
        {
            catAnimator.SetBool("isEating", true); 
        }
    }

    // --- 按钮选项逻辑 ---

    // 选项1: 摸摸它 (Anxiety +10)
    public void OnClick_TouchCat()
    {
        petState.anxiety += 10;
        
        // 可以在这里加猫咪哈气的动画 triggers
        // if(catAnimator) catAnimator.SetTrigger("Hiss");

        FinishLevel("Don't disturb! The cat is angry because you touched it while eating.");
    }

    // 选项2: 等它吃完 (Coin +1, Anxiety -10)
    public void OnClick_Wait()
    {
        petState.catCoin += 1;
        petState.anxiety -= 10;
        FinishLevel("Good owner! The cat finished eating and cuddled with you.");
    }

    void FinishLevel(string msg)
    {
        Debug.Log(msg);
        petState.CheckGameStatus();
        petState.UpdateUI();
        uiPanel.SetActive(false);
        
        // 停止吃饭动画
        if (catAnimator != null) catAnimator.SetBool("isEating", false);

        levelManager.ForceLevelUp();
    }
}