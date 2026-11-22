
using UnityEngine;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    [Header("武器配置")]
    public WeaponConfig weaponConfig;

    [Header("武器碰撞体")]
    public Collider weaponCollider;

    // 当前处于武器攻击范围内的角色
    private HashSet<CharacterCollider> targets =
        new HashSet<CharacterCollider>();

    void Awake()
    {
        if (weaponCollider == null)
        {
            weaponCollider = GetComponent<Collider>();
        }

        if (weaponCollider != null)
        {
            weaponCollider.isTrigger = true;
            weaponCollider.enabled = true;
        }
    }

    // 目标进入攻击范围
    private void OnTriggerEnter(Collider other)
    {
        CharacterCollider target =
            other.GetComponentInParent<CharacterCollider>();

        if (target != null)
        {
            targets.Add(target);
        }
    }

    // 目标离开攻击范围
    private void OnTriggerExit(Collider other)
    {
        CharacterCollider target =
            other.GetComponentInParent<CharacterCollider>();

        if (target != null)
        {
            targets.Remove(target);
        }
    }

    // Animation Event 调用：执行攻击判定
    public void AttackHit()
    {
        foreach (CharacterCollider target in targets)
        {
            if (target == null) continue;

            Debug.Log("命中目标：" + target.name);

            // 后续在这里调用伤害计算系统
        }
    }
}
