using UnityEngine;

public class ClickTrigger : MonoBehaviour
{
    public LitterBoxPuzzle manager; // 引用那个总管脚本
    public int myIndex = -1;        // 我是第几个光圈？(-1代表我是猫砂盆)

    void OnMouseDown()
    {
        // 必须有 Collider 组件，鼠标点击才有效
        if (myIndex == -1)
        {
            // 我是猫砂盆
            manager.OnRealBoxClicked();
        }
        else
        {
            // 我是光圈
            manager.OnSpotClicked(myIndex);
        }
    }
}