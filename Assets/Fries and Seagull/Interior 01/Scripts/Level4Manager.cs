using UnityEngine;

public class Level4Manager : MonoBehaviour
{
    public LevelManager levelManager;

    // 只要是第4关(索引4)，就显示跳蚤；否则隐藏
    private int targetLevelIndex = 4;

    // 这里存那 3 个跳蚤物体
    public GameObject[] fleas;

    // 计数器：找到了几个？
    private int foundCount = 0;

    void Update()
    {
        if (levelManager.currentLevelIndex == targetLevelIndex)
        {
            // 如果是这一关，确保没被找到的跳蚤是显示的
            // (这里不需要一直SetActive，为了省性能，只在进入关卡时初始化一次最好，但放在Update最简单防Bug)
        }
        else
        {
            // 不是这一关，统统隐藏
            foreach (var flea in fleas)
            {
                if (flea != null) flea.SetActive(false);
            }
        }
    }

    // 当跳蚤被点击时，由跳蚤自己调用这个方法
    public void OnFleaClicked(GameObject fleaObj)
    {
        // 1. 让跳蚤消失
        fleaObj.SetActive(false);

        // 2. 计数 +1
        foundCount++;
        Debug.Log("Found " + foundCount + " / " + fleas.Length + " fleas");

        // 3. 检查是否找齐了
        if (foundCount >= fleas.Length)
        {
            Debug.Log("All fleas found! Proceed to next stage!");
            levelManager.ForceLevelUp();
        }
    }
}