using UnityEngine;
using UnityEngine.UIElements;

public class EnemyControl : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject HPbar;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;

        // Canvas 与摄像机保持相同旋转
        Vector3 direction = mainCamera.transform.position - HPbar.transform.position;

        // 让血条正面朝向摄像机
        HPbar.transform.rotation = Quaternion.LookRotation(direction);
    }
}
