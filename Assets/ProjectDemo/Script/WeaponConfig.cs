
using UnityEngine;

public enum DamageType
{
    Physical,
    Magical
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game/Weapon Config")]
public class WeaponConfig : ScriptableObject
{
    [Header("武器基础信息")]
    public string weaponName;
    public Sprite weaponIcon;
    public GameObject weaponPrefab;

    [Header("伤害类型")]
    public DamageType damageType = DamageType.Physical;

    [Header("武器属性")]
    public CharacterStats stats = new CharacterStats();
}
