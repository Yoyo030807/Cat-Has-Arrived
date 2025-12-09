using UnityEngine;
using TMPro; // 确保导入 TextMeshPro

public class StatDisplay : MonoBehaviour
{
    // 在 Inspector 中，选择这个文本框要显示哪个数值
    public enum StatType { Health, Hunger, Anxiety, CatCoin }
    public StatType statToDisplay;

    // 拖拽：把您的“猫”物体 (PetStats脚本所在物体) 拖到这里
    public PetState targetPet;

    private TextMeshProUGUI textComponent;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        if (targetPet == null || textComponent == null)
        {
            Debug.LogError("StatDisplay 脚本缺少 PetStats 目标或 Text 组件!");
            enabled = false;
        }
    }

    void Update()
    {
        if (targetPet == null) return;

        float value = 0f;
        string label = "";

        // 根据选择的类型，读取对应的数值
        switch (statToDisplay)
        {
            case StatType.Health:
                value = targetPet.health;
                label = "健康值";
                break;
            case StatType.Hunger:
                value = targetPet.hunger;
                label = "饱食度"; // 换成“饱食度”可能比“饥饿值”更符合 0-100 的数值逻辑
                break;
            case StatType.Anxiety:
                value = targetPet.anxiety;
                label = "焦虑值";
                break;
            case StatType.CatCoin:
                // 猫猫币是整数，单独处理
                textComponent.text = $"猫猫币: {targetPet.catCoin}";
                return;
        }

        // 通用格式化并显示文本 (显示整数)
        textComponent.text = $"{label}: {value:F0}%";
    }
}