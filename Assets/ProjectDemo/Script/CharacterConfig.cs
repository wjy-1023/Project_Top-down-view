
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Game/Character Config")]
public class CharacterConfig : ScriptableObject
{
    [Header("角色基础信息")]
    public string characterName;
    public Sprite characterIcon;

    [Header("角色基础属性")]
    public CharacterStats stats = new CharacterStats();
}
