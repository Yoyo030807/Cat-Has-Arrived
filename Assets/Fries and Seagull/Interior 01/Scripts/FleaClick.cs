using UnityEngine;

public class FleaClick : MonoBehaviour
{
    public Level4Manager manager;

    void OnMouseDown()
    {
        // 只有在当前关卡是第4关时才能点
        // 为了简单，直接通知经理，经理会判断
        if (manager != null)
        {
            manager.OnFleaClicked(this.gameObject);
        }
    }
}