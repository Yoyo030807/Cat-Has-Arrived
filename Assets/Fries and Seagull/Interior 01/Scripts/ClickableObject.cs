using UnityEngine;
using UnityEngine.Events; // 引入事件系统

[System.Diagnostics.DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class ClickableObject : MonoBehaviour
{
    // 这是一个可以在 Inspector 里配置的“事件槽”
    // 你可以把任何函数拖进来：比如“打开面板”、“播放声音”、“加金币”
    [Header("Click Settings")]
    public UnityEvent onClickAction;

    // 鼠标按下时触发
    void OnMouseDown()
    {
        // 只有当鼠标点到这个带有 Collider 的物体时，Unity 会自动调用这个方法
        Debug.Log("Clicked on object: " + gameObject.name);

        // 执行你拖进来的所有操作
        if (onClickAction == null)
        {
            return;
        }
        onClickAction.Invoke();
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}