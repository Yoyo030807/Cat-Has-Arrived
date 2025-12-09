using UnityEngine;

public class BowlInteraction : MonoBehaviour
{
    // 在 Inspector 窗口中，把 Bowl_UI 拖到这里
    public GameObject bowlUI;

    // 当鼠标点击了这个物体 (的碰撞体) 时...
    private void OnMouseDown()
    {
        if (bowlUI != null)
        {
            // 1. 获取当前 UI 是开还是关？ (activeSelf 返回 true 或 false)
            bool isCurrentlyActive = bowlUI.activeSelf;

            // 2. 设置为相反的状态
            // ! 符号在编程里是 "非" 的意思 (即：把 true 变成 false，把 false 变成 true)
            bowlUI.SetActive(!isCurrentlyActive);

            Debug.Log("Bowl clicked. UI Active State is now: " + !isCurrentlyActive);
        }
    }
}