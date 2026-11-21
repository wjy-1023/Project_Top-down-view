using UnityEngine;
using UnityEngine.UIElements;

public class CameraControl : MonoBehaviour
{
    [Header("跟随目标")]
    public Transform target;

    [Header("摄像机偏移")]
    public Vector3 offset = new Vector3(0f, 15f, -5f);

    [Header("跟随参数")]
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // 计算目标位置
        Vector3 desiredPosition = target.position + offset;

        // 平滑跟随
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
