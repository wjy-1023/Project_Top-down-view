
using UnityEngine;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    [Header("武器配置")]
    public WeaponConfig weaponConfig;

    [Header("武器碰撞体")]
    public Collider weaponCollider;

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
        else
        {
            Debug.LogError("武器没有找到 Collider！");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("武器检测到碰撞体：" + other.name);

        CharacterCollider target =
            other.GetComponentInParent<CharacterCollider>();

        if (target != null)
        {
            targets.Add(target);
            Debug.Log("检测到敌人：" + target.name);
        }
        else
        {
            Debug.LogWarning(
                other.name + " 没有找到 CharacterCombat！"
            );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterCollider target =
            other.GetComponentInParent<CharacterCollider>();

        if (target != null)
        {
            targets.Remove(target);
            Debug.Log("敌人离开武器范围：" + target.name);
        }
    }

    public void AttackHit()
    {
        Debug.Log("攻击事件触发，当前检测敌人数量：" + targets.Count);

        if (weaponConfig == null)
        {
            Debug.LogWarning("没有配置武器！");
            return;
        }

        CharacterAttribute attackerAttribute =
            GetComponentInParent<CharacterAttribute>();

        if (attackerAttribute == null)
        {
            Debug.LogWarning("没有找到攻击者属性！");
            return;
        }

        foreach (CharacterCollider target in targets)
        {
            if (target == null || target.characterAttribute == null)
                continue;

            bool isCritical;

            float damage = DamageCalculator.CalculateDamage(
                attackerAttribute.finalStats,
                target.characterAttribute.finalStats,
                weaponConfig.damageType,
                out isCritical
            );

            HealthSystem targetHealth =
            target.GetComponent<HealthSystem>();

            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage, isCritical);
            }

            Debug.Log(
                $"命中：{target.name}，伤害：{damage:F1}，暴击：{isCritical}"
            );
        }
    }
}
