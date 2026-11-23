using UnityEngine;

public class CharacterCollider : MonoBehaviour
{
    [Header("角色碰撞体")]
    public Collider[] characterColliders;

    [Header("角色属性")]
    public CharacterAttribute characterAttribute;

    void Awake()
    {
        if (characterColliders == null || characterColliders.Length == 0)
        {
            characterColliders = GetComponentsInChildren<Collider>();
        }

        if (characterAttribute == null)
        {
            characterAttribute = GetComponent<CharacterAttribute>();
        }
    }
}
