using System;
using UnityEngine;

public class PistolBullet : MonoBehaviour
{
    [Header("Настройки пули")]
    [SerializeField] private float damage = 25f;       // Урон, можно настроить в инспекторе
    [SerializeField] private float maxDistance = 100f; // Дальность луча
    [SerializeField] private float lifeTime = 10f;     // Время до автоудаления

    private bool hasHit = false; // флаг, чтобы не обрабатывать попадание несколько раз

    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (hasHit) return;

        float radius = 0.10f; // радиус проверки (настроить в инспекторе)
        float distance = maxDistance * Time.deltaTime; // шаг проверки за кадр

        // Сферический луч
        if (Physics.SphereCast(transform.position, radius, transform.forward, out var hit, distance))
        {
            HandleHit(hit.collider);
            return;
        }

        // На всякий случай проверим вокруг пули в текущем месте
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var col in colliders)
        {
            if (col != null && col.gameObject != gameObject)
            {
                HandleHit(col);
                break;
            }
        }
    }


    private void HandleHit(Collider collider)
    {
        if (hasHit) return;
        hasHit = true;

        GameObject hitObj = collider.gameObject;

        // 1) Попытка получить строго типизированный компонент EnemyStats (тот, что ты добавил в шаблон)
        if (hitObj.TryGetComponent<Enemy_Stats>(out var enemyStats))
        {
            enemyStats.TakeDamage(damage);
            Debug.Log($"Enemy hit! HP: {enemyStats.Health}");

            if (enemyStats.Health <= 0f)
            {
                enemyStats.gameObject.SetActive(false);
            }

            Destroy(gameObject, 2f);
            return;
        }

        //// 2) Если у тебя в проекте другой класс (например Enemy_Stats), попробуем найти его через GetComponent(string)
        ////    и вызвать метод TakeDamage через reflection (это безопасно: если метода нет — просто пропустим).
        //Component other = hitObj.GetComponent("Enemy_Stats"); // вернёт null если такого типа нет
        //if (other != null)
        //{
        //    try
        //    {
        //        var t = other.GetType();

        //        // Вызвать TakeDamage(float)
        //        var takeDamageMethod = t.GetMethod("TakeDamage", new Type[] { typeof(float) });
        //        if (takeDamageMethod != null)
        //        {
        //            takeDamageMethod.Invoke(other, new object[] { damage });
        //        }

        //        // Попробовать получить Health через свойство или поле (для логов и проверки мёртв ли)
        //        float currentHealth = float.NaN;
        //        var healthProp = t.GetProperty("Health");
        //        if (healthProp != null)
        //        {
        //            var val = healthProp.GetValue(other);
        //            if (val is float f) currentHealth = f;
        //            else if (val is double d) currentHealth = (float)d;
        //            else if (val is int i) currentHealth = i;
        //        }
        //        else
        //        {
        //            var healthField = t.GetField("Health");
        //            if (healthField != null)
        //            {
        //                var val = healthField.GetValue(other);
        //                if (val is float f2) currentHealth = f2;
        //                else if (val is double d2) currentHealth = (float)d2;
        //                else if (val is int i2) currentHealth = i2;
        //            }
        //        }

        //        if (!float.IsNaN(currentHealth))
        //            Debug.Log($"Enemy (Enemy_Stats) hit! HP: {currentHealth}");

        //        // Если здоровье доступно и <= 0 — деактивируем объект
        //        if (!float.IsNaN(currentHealth) && currentHealth <= 0f)
        //        {
        //            other.gameObject.SetActive(false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.LogWarning($"Reflection hit handling failed: {ex.Message}");
        //    }

        //    Destroy(gameObject, 2f);
        //    return;
        //}

        // 3) Если это не враг — просто уничтожаем пулю (например, попали в стену)
        Destroy(gameObject, 2f);
    }

    // Дополнительный обработчик если пуля настроена как Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        // Если попали в объект — обработаем так же как Raycast
        HandleHit(other);
    }
}
