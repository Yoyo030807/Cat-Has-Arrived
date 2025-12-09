using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // 在 Inspector 窗口中，把您的主摄像机拖到这里
    public Camera mainCamera;

    void LateUpdate()
    {
        // 检查摄像机是否存在
        if (mainCamera == null)
        {
            // 如果没在 Inspector 中设置，就自动查找 "MainCamera" 标签
            mainCamera = Camera.main;

            // 如果还是找不到，就停止执行
            if (mainCamera == null)
            {
                Debug.LogWarning("LookAtCamera 脚本: 找不到主摄像机！");
                return;
            }
        }

        // 核心代码：
        // 让这个物体(画布)的“正面”(transform.forward)
        // 始终指向摄像机的位置
        transform.LookAt(transform.position + mainCamera.transform.forward);
    }
}