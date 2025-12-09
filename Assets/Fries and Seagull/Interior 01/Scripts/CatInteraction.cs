using UnityEngine;

public class CatInteraction : MonoBehaviour
{
    public PetState petState; // 拖入 PetState

    // 定义一个选项：这是好事(玩耍)还是坏事(抱抱)？
    public enum InteractionType { Play, Hug }
    public InteractionType type;

    void OnMouseDown()
    {
        // 1. 安全检查
        if (petState == null) return;

        // 2. 根据类型调用不同的功能
        if (type == InteractionType.Play)
        {
            petState.OnPlayWithTeaser();
            // 可选：播放个音效或动画
        }
        else if (type == InteractionType.Hug)
        {
            petState.OnForceHug();
        }
    }
}