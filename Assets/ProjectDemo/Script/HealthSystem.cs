
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

    public GameObject regenEffectPrefab;
    public Transform regenEffectPoint;

    // 当前生成的特效实例
    private GameObject regenEffectInstance;

    [Header("受击动画")]
    public Animator characterAnimator;
    public Animator healthBarAnimator;

    [Header("受击特效")]
    public GameObject hitEffectPrefab;

    // 特效生成位置
    public Transform hitEffectPoint;

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
    public void TakeDamage(float damage, bool isCritical)
    {
        if (currentHealth <= 0)
            return;

        damage = Mathf.Max(0, damage);

        if (damage <= 0)
            return;

        // 扣除当前血量
        currentHealth = Mathf.Clamp(
            currentHealth - damage,
            0,
            maxHealth
        );

        // 生成受击特效
        CreateHitEffect();

        Debug.Log(
            $"{name} 受到 {damage:F1} 点伤害，" +
            $"当前生命值：{currentHealth:F1}/{maxHealth:F1}"
        );

        // 更新血条
        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealth(currentHealth, maxHealth);
        }

        // 触发角色受击动画
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("Hurt");
        }

        // 触发血条受击动画
        if (healthBarAnimator != null)
        {
            if (isCritical)
            {
                healthBarAnimator.SetTrigger("Crit");
            }
            else
            {
                healthBarAnimator.SetTrigger("NoCrit");
            }
        }

        // 重新启动自动回血计时
        StartRegenTimer();
    }

    // 重新启动回血计时
    private void StartRegenTimer()
    {
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }

        // 如果回血被打断，移除特效
        RemoveRegenEffect();

        regenCoroutine = StartCoroutine(AutoRegen());
    }

    // 自动回血
    private IEnumerator AutoRegen()
    {
        // 等待受伤后的间隔时间
        yield return new WaitForSeconds(regenDelay);

        if (currentHealth >= maxHealth)
        {
            regenCoroutine = null;
            yield break;
        }

        // 开始回血，生成特效
        CreateRegenEffect();

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

        // 确保最终生命值为最大值
        currentHealth = maxHealth;

        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealthDuringRegen(
                currentHealth,
                maxHealth
            );
        }

        // 回血完成，移除特效
        RemoveRegenEffect();

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

    // 生成回血特效
    private void CreateRegenEffect()
    {
        if (regenEffectPrefab == null)
            return;

        // 防止重复生成
        if (regenEffectInstance != null)
            return;

        Transform spawnPoint = regenEffectPoint != null
            ? regenEffectPoint
            : transform;

        regenEffectInstance = Instantiate(
            regenEffectPrefab,
            spawnPoint.position,
            spawnPoint.rotation,
            spawnPoint
        );
    }

    // 移除回血特效
    private void RemoveRegenEffect()
    {
        if (regenEffectInstance != null)
        {
            Destroy(regenEffectInstance);
            regenEffectInstance = null;
        }
    }

    // 生成受击特效
    private void CreateHitEffect()
    {
        if (hitEffectPrefab == null)
            return;

        Transform spawnPoint = hitEffectPoint != null
            ? hitEffectPoint
            : transform;

        GameObject effect = Instantiate(
            hitEffectPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        // 自动计算粒子播放结束时间
        ParticleSystem[] particleSystems =
            effect.GetComponentsInChildren<ParticleSystem>();

        float destroyTime = 0f;

        foreach (ParticleSystem ps in particleSystems)
        {
            var main = ps.main;

            float lifetime = main.startLifetime.constantMax;

            float totalTime = main.duration + lifetime;

            destroyTime = Mathf.Max(destroyTime, totalTime);
        }

        // 防止没有粒子组件时无法销毁
        if (destroyTime <= 0)
            destroyTime = 3f;

        Destroy(effect, destroyTime);
    }
}
