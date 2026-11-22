
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    [Header("基础属性")]
    public float health = 100f;
    public float attack = 10f;
    public float magicPower = 0f;
    public float armor = 0f;
    public float magicResistance = 0f;

    [Header("固定穿透")]
    public float armorPenetration = 0f;
    public float magicPenetration = 0f;

    [Header("万分比属性")]
    public float armorPenetrationRate = 0f;
    public float magicPenetrationRate = 0f;
    public float critRate = 0f;
    public float critDamage = 0f;

    // 创建属性副本
    public CharacterStats Clone()
    {
        return new CharacterStats
        {
            health = health,
            attack = attack,
            magicPower = magicPower,
            armor = armor,
            magicResistance = magicResistance,
            armorPenetration = armorPenetration,
            magicPenetration = magicPenetration,
            armorPenetrationRate = armorPenetrationRate,
            magicPenetrationRate = magicPenetrationRate,
            critRate = critRate,
            critDamage = critDamage
        };
    }

    // 属性累加
    public void Add(CharacterStats other)
    {
        health += other.health;
        attack += other.attack;
        magicPower += other.magicPower;
        armor += other.armor;
        magicResistance += other.magicResistance;
        armorPenetration += other.armorPenetration;
        magicPenetration += other.magicPenetration;
        armorPenetrationRate += other.armorPenetrationRate;
        magicPenetrationRate += other.magicPenetrationRate;
        critRate += other.critRate;
        critDamage += other.critDamage;
    }
}
