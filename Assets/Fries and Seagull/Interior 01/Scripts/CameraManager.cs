using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // 单例模式，方便其他脚本直接呼叫它
    public static CameraManager Instance;

    public float moveSpeed = 2.0f; // 镜头移动速度
    public float rotateSpeed = 2.0f; // 镜头旋转速度

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // 游戏刚开始，记录摄像机当前的位置作为默认位置
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // 每一帧都让摄像机平滑地飞向目标位置 (Lerp插值)
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
    }

    // ⭐ 这个方法给 LevelManager 调用
    // 传入我们第一步设置的那些“支架”的位置信息
    public void MoveToTarget(Transform targetTransform)
    {
        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetRotation = targetTransform.rotation;
        }
    }
}