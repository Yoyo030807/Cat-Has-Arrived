using UnityEngine;

public class ClickToPlay : MonoBehaviour
{
    // 注意：这里引用的是新的 Level3Manager
    public Level3Manager manager;

    public enum TargetType { Cat, Teaser }
    public TargetType myType;

    void OnMouseDown()
    {
        if (manager == null) return;

        if (myType == TargetType.Cat)
        {
            manager.ShowHugOption(); // 点猫 -> 显示抱抱按钮
        }
        else
        {
            manager.ShowPlayOption(); // 点逗猫棒 -> 显示逗猫按钮
        }
    }
}