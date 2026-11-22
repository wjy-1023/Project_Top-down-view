
using UnityEngine;

public class CharacterAttribute : MonoBehaviour
{
    [Header("角色基础属性")]
    public CharacterStats baseStats = new CharacterStats();

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
        finalStats = baseStats.Clone();

        if (currentWeapon != null)
        {
            finalStats.Add(currentWeapon.stats);
        }
    }

    // 更换武器
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
