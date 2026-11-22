using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    [Header("移动参数")]
    public float moveSpeed = 5f;

    [Header("转向参数")]
    public float rotationSpeed = 15f;

    [Header("摄像机")]
    public Camera mainCamera;

    [Header("攻击参数")]
    public Animator animator;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        Move();
        RotateToMouse();
        Attack();
    }

    // WASD 移动
    void Move()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) input.y += 1;
            if (Keyboard.current.sKey.isPressed) input.y -= 1;
            if (Keyboard.current.dKey.isPressed) input.x += 1;
            if (Keyboard.current.aKey.isPressed) input.x -= 1;
        }

        Vector3 moveDirection = new Vector3(
            input.x, 0f, input.y
        ).normalized;

        controller.Move(
            moveDirection * moveSpeed * Time.deltaTime
        );
    }

    // 朝向鼠标
    void RotateToMouse()
    {
        if (mainCamera == null || Mouse.current == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        Plane groundPlane = new Plane(
            Vector3.up,
            transform.position
        );

        float distance;

        if (groundPlane.Raycast(ray, out distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);

            Vector3 direction = targetPosition - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation =
                    Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }

    // 鼠标左键攻击
    void Attack()
    {
        if (Mouse.current == null || animator == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            animator.SetTrigger("Attack");
        }
    }

    public void OpenComboWindow()
    {
        animator.SetBool("Combo", true);
    }

    public void OnAttackEvent()
    {
        StartCoroutine(DelayedFunction());
    }

    IEnumerator DelayedFunction()
    {
        yield return new WaitForSeconds(0.5f);

        // 0.5秒后执行
        CloseComboWindow();
    }

    // 关闭连击窗口
    public void CloseComboWindow()
    {
        animator.SetBool("Combo", false);
    }
}