
using UnityEngine;

public static class DamageCalculator
{
    /// <summary>
    /// 通用伤害计算
    /// </summary>
    public static float CalculateDamage(
        CharacterStats attacker,
        CharacterStats target,
        DamageType damageType,
        out bool isCritical)
    {
        isCritical = false;

        if (attacker == null || target == null)
            return 0f;

        float baseDamage;
        float targetDefense;
        float penetrationValue;
        float penetrationRate;

        // 根据伤害类型选择对应属性
        switch (damageType)
        {
            case DamageType.Physical:
                baseDamage = attacker.attack;
                targetDefense = target.armor;
                penetrationValue = attacker.armorPenetration;
                penetrationRate = attacker.armorPenetrationRate;
                break;

            case DamageType.Magical:
                baseDamage = attacker.magicPower;
                targetDefense = target.magicResistance;
                penetrationValue = attacker.magicPenetration;
                penetrationRate = attacker.magicPenetrationRate;
                break;

            default:
                return 0f;
        }

        // 1. 计算有效防御
        float rate = Mathf.Clamp01(
            penetrationRate / 10000f
        );

        float effectiveDefense =
            targetDefense * (1f - rate)
            - penetrationValue;

        effectiveDefense = Mathf.Max(0f, effectiveDefense);

        // 2. 计算防御减伤
        float damageReduction =
            effectiveDefense / (effectiveDefense + 100f);

        // 3. 计算最终基础伤害
        float damage =
            baseDamage * (1f - damageReduction);

        // 4. 暴击判定
        float critRate = Mathf.Clamp01(
            attacker.critRate / 10000f
        );

        if (Random.value < critRate)
        {
            isCritical = true;

            float critMultiplier =
                attacker.critDamage / 10000f;

            damage *= (1f+critMultiplier);
        }

        return damage;
    }
}
