using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    [Tooltip("RectTransform, отвечающий за изменение размера health bar-а")]
    [SerializeField] private RectTransform barRect;
    private Image barImage;

    [Header("Health Settings")]
    [Tooltip("ћаксимальное здоровье")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth = 100f;

    private float initialBarScaleX;
    private Coroutine blinkCoroutine;
    [Tooltip("—корость плавного перехода при мигании")]
    [SerializeField] private float blinkSpeed = 5f;

    private void Awake()
    {
        barImage = GetComponent<Image>();
        if (barRect == null)
        {
            barRect = GetComponent<RectTransform>();
        }
        initialBarScaleX = barRect.localScale.x;
    }

    public void SetHealth(float newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, 0f, maxHealth);
        RefreshHealthBar();
    }

    private void RefreshHealthBar()
    {
        float healthRatio = currentHealth / maxHealth;
        barRect.localScale = new Vector3(initialBarScaleX * healthRatio, barRect.localScale.y, 1);

        if (healthRatio > 0.25f)
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }

            if (healthRatio > 0.75f)
            {
                barImage.color = Color.Lerp(Color.yellow, Color.green, (healthRatio - 0.75f) / 0.25f);
            }
            else if (healthRatio > 0.5f)
            {
                Color darkYellow = new Color(1.0f, 0.6f, 0.0f);
                barImage.color = Color.Lerp(darkYellow, Color.yellow, (healthRatio - 0.5f) / 0.25f);
            }
            else
            {
                Color darkYellow = new Color(1.0f, 0.6f, 0.0f);
                barImage.color = Color.Lerp(Color.red, darkYellow, (healthRatio - 0.25f) / 0.25f);
            }
        }
        else
        {
            if (blinkCoroutine == null)
            {
                blinkCoroutine = StartCoroutine(SmoothBlinkRedWhite());
            }
        }
    }

    private IEnumerator SmoothBlinkRedWhite()
    {
        float t = 0f;
        while (true)
        {
            while (t < 1f)
            {
                barImage.color = Color.Lerp(Color.red, Color.white, t);
                t += Time.deltaTime * blinkSpeed;
                yield return null;
            }
            while (t > 0f)
            {
                barImage.color = Color.Lerp(Color.red, Color.white, t);
                t -= Time.deltaTime * blinkSpeed;
                yield return null;
            }
        }
    }
}