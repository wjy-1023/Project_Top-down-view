
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarUI : MonoBehaviour
{
    [Header("血条 Image")]
    public Image currentHealthImage;
    public Image bufferHealthImage;

    [Header("血量文字")]
    public Text currentHealthText;
    public Text maxHealthText;

    [Header("缓冲设置")]
    public float bufferDuration = 1f;

    private Coroutine bufferCoroutine;

    // 更新血量
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (maxHealth <= 0)
            return;

        float targetFill = Mathf.Clamp01(
            currentHealth / maxHealth
        );

        // 当前血条立即变化
        currentHealthImage.fillAmount = targetFill;

        // 更新文字
        UpdateHealthText(currentHealth, maxHealth);

        // 停止之前的缓冲动画
        if (bufferCoroutine != null)
        {
            StopCoroutine(bufferCoroutine);
        }

        // 启动缓冲动画
        bufferCoroutine = StartCoroutine(
            BufferHealthAnimation(targetFill)
        );
    }

    // 更新血量文字
    private void UpdateHealthText(float currentHealth, float maxHealth)
    {
        if (currentHealthText != null)
        {
            currentHealthText.text =
                Mathf.CeilToInt(currentHealth).ToString();
        }

        if (maxHealthText != null)
        {
            maxHealthText.text =
                Mathf.CeilToInt(maxHealth).ToString();
        }
    }

    // 缓冲血条动画
    private IEnumerator BufferHealthAnimation(float targetFill)
    {
        float startFill = bufferHealthImage.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < bufferDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsedTime / bufferDuration
            );

            bufferHealthImage.fillAmount = Mathf.Lerp(
                startFill,
                targetFill,
                progress
            );

            yield return null;
        }

        bufferHealthImage.fillAmount = targetFill;
        bufferCoroutine = null;
    }

    // 自动回血时同步更新
    public void UpdateHealthDuringRegen(
        float currentHealth,
        float maxHealth)
    {
        if (maxHealth <= 0)
            return;

        float targetFill = Mathf.Clamp01(
            currentHealth / maxHealth
        );

        currentHealthImage.fillAmount = targetFill;
        bufferHealthImage.fillAmount = targetFill;

        UpdateHealthText(currentHealth, maxHealth);
    }
}
