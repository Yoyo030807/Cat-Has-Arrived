using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // 1. 必须引用

public class PlayerMove : MonoBehaviour
{
    private NavMeshAgent catAgent;
    private Animator catAnimator;

    // 【新】我们用一个布尔值(bool)来跟踪猫的“意图”
    private bool isSitting = false;

    void Start()
    {
        catAgent = GetComponent<NavMeshAgent>();
        catAnimator = GetComponent<Animator>();

        // 确保 Unity 和脚本的“站/坐”状态在游戏开始时是一致的
        catAnimator.SetBool("IsSitting", isSitting);
    }

    void Update()
    {
        // --- 1. “坐下/站起”逻辑 (S 键) ---
        // 当按下 S 键...
        if (Input.GetKeyDown(KeyCode.S))
        {
            // 我们“切换”坐下的意图
            isSitting = !isSitting;

            // --- 如果我们刚决定要“坐下” ---
            if (isSitting == true)
            {
                // ...并且猫“没有在走” (速度很慢)
                if (catAgent.velocity.magnitude < 0.1f)
                {
                    // 1. 立刻停止 Agent (AI大脑)，防止它滑动
                    catAgent.isStopped = true;
                    catAgent.ResetPath(); // 清除它可能正在走的路径

                    // 2. 告诉 Animator 播放“坐下”
                    catAnimator.SetBool("IsSitting", true);
                }
                else
                {
                    // 如果猫正在走路时，我们按了S键，先不让它坐下
                    // (防止它在走路时突然卡住)
                    isSitting = false; // 取消本次“坐下”的意图
                }
            }
            // --- 如果我们刚决定要“站起来” ---
            else
            {
                // 1. 告诉 Animator 播放“站立” (回到 idle)
                catAnimator.SetBool("IsSitting", false);

                // 2. 允许 Agent 再次移动
                catAgent.isStopped = false;
            }
        }

        // --- 2. “点击移动”逻辑 (鼠标左键) ---
        if (Input.GetMouseButtonDown(0))
        {
            // 【重要】如果用户点击移动，这“自动”意味着猫必须“站起来”
            if (isSitting == true)
            {
                isSitting = false; // 取消坐下的意图
                catAnimator.SetBool("IsSitting", false); // 告诉 Animator 站起来
                catAgent.isStopped = false; // 允许 Agent 移动
            }

            // --- (您的射线检测代码，现在是安全的) ---
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 我们只在“地面” (Ground) 图层上检测
            if (Physics.Raycast(ray, out hit))
            {
                catAgent.SetDestination(hit.point);
            }
        }

        // --- 3. “动画”逻辑 (每帧更新) ---
        // 只有当猫“没有”被命令坐下时，才更新行走动画
        if (!isSitting)
        {
            UpdateAnimation();
        }
    }

    void UpdateAnimation()
    {
        float speed = catAgent.velocity.magnitude;
        catAnimator.SetFloat("Speed", speed);
    }
}