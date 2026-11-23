
using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    [Header("生命值")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("血条 UI")]
    public HealthBarUI healthBarUI;

    [Header("自动回血设置")]
    public float regenDelay = 3f;       // 受伤后等待时间
    public float regenDuration = 3f;    // 完整回血所需时间

    private Coroutine regenCoroutine;

    private void Start()
    {
        CharacterAttribute attribute =
            GetComponent<CharacterAttribute>();

        if (attribute != null && attribute.finalStats != null)
        {
            maxHealth = attribute.finalStats.health;
        }

        currentHealth = maxHealth;

        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealth(currentHealth, maxHealth);
        }
    }

    // 受到伤害
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0)
            return;

        damage = Mathf.Max(0, damage);

        if (damage <= 0)
            return;

        currentHealth = Mathf.Clamp(
            currentHealth - damage,
            0,
            maxHealth
        );

        Debug.Log(
            $"{name} 受到 {damage:F1} 点伤害，" +
            $"当前生命值：{currentHealth:F1}/{maxHealth:F1}"
        );

        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealth(currentHealth, maxHealth);
        }

        // 受到伤害后，重新计算自动回血时间
        StartRegenTimer();
    }

    // 重新启动回血计时
    private void StartRegenTimer()
    {
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
        }

        regenCoroutine = StartCoroutine(AutoRegen());
    }

    // 自动回血
    private IEnumerator AutoRegen()
    {
        // 等待一段时间没有受到伤害
        yield return new WaitForSeconds(regenDelay);

        if (currentHealth >= maxHealth)
        {
            regenCoroutine = null;
            yield break;
        }

        float startHealth = currentHealth;
        float elapsedTime = 0f;

        // 在指定时间内恢复到最大生命值
        while (elapsedTime < regenDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsedTime / regenDuration
            );

            currentHealth = Mathf.Lerp(
                startHealth,
                maxHealth,
                progress
            );

            if (healthBarUI != null)
            {
                healthBarUI.UpdateHealthDuringRegen(
                    currentHealth,
                    maxHealth
                );
            }

            yield return null;
        }

        currentHealth = maxHealth;

        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealthDuringRegen(
                currentHealth,
                maxHealth
            );
        }

        Debug.Log($"{name} 自动回血完成");

        regenCoroutine = null;
    }

    // 手动恢复生命值
    public void Heal(float amount)
    {
        if (currentHealth <= 0)
            return;

        currentHealth = Mathf.Clamp(
            currentHealth + amount,
            0,
            maxHealth
        );

        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealth(currentHealth, maxHealth);
        }
    }
}
