
using UnityEngine;

public class CharacterAttribute : MonoBehaviour
{
    [Header("角色配置")]
    public CharacterConfig characterConfig;

    [Header("当前装备武器")]
    public WeaponConfig currentWeapon;

    [Header("最终属性（运行时计算）")]
    public CharacterStats finalStats;

    void Awake()
    {
        RefreshStats();
    }

    // 刷新最终属性
    public void RefreshStats()
    {
        if (characterConfig == null)
        {
            Debug.LogWarning("没有配置角色数据！", this);
            return;
        }

        // 复制角色基础属性
        finalStats = characterConfig.stats.Clone();

        // 累加武器属性
        if (currentWeapon != null)
        {
            finalStats.Add(currentWeapon.stats);
        }
    }

    // 装备武器
    public void EquipWeapon(WeaponConfig weapon)
    {
        currentWeapon = weapon;
        RefreshStats();
    }

    // 卸下武器
    public void UnequipWeapon()
    {
        currentWeapon = null;
        RefreshStats();
    }
}
