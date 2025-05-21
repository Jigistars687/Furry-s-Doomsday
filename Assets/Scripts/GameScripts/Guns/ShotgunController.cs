using UnityEngine;
using System.Collections;

/// <summary>
/// Скрипт реализация стрельбы из дробовика: вылет 6 дробинок (сфер) с разбросом, учёт боеприпасов и перезарядка (КД).
/// Примените к объекту "Shotgun" с Transform ствола и префабом дробинки.
/// </summary>
public class ShotgunController : MonoBehaviour
{
    [Header("Настройки дробовика")]
    [Tooltip("Префаб снаряда дробинки (сфера с Rigidbody)")]
    [SerializeField] private GameObject pelletPrefab;

    [Tooltip("Точка появления дробинок (обычно Transform ствола)")]
    [SerializeField] private Transform barrelEnd;

    [Tooltip("Количество дробинок за выстрел")]
    [SerializeField] private int pelletsPerShot = 99;

    [Tooltip("Угол разброса в градусах")]
    [SerializeField] private float spreadAngle = 360f;

    [Tooltip("Сила вылета дробинки")]
    [SerializeField] private float pelletForce = 750f;

    [Header("Боезапас и КД")]
    [Tooltip("Максимальный боезапас дробовика")]
    [SerializeField] private int maxAmmo = 24;

    [Tooltip("Текущее количество боеприпасов")]
    [SerializeField] private int currentAmmo;

    [Tooltip("Время между выстрелами (сек)")]
    [SerializeField] private float cooldownTime = 1f;

    private float nextFireTime = 0f;

    void Start()
    {
        // Инициализация текущего боезапаса
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        // Проверка нажатия кнопки "Fire1" (обычно левая кнопка мыши или Ctrl)
        if (Input.GetKey(BindingKeysManager.Attack_Key_KEYCODE) && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                FireShotgun();
                currentAmmo--; // уменьшить боезапас
                Debug.Log($"Осталось патронов: {currentAmmo}");
                nextFireTime = Time.time + cooldownTime;
            }
            else
            {
                Debug.Log("Нет боеприпасов! Пора перезарядиться.");
            }
        }
    }

    /// <summary>
    /// Выполняет выстрел дробовика: создаёт дробинки с рандомным разбросом и придаёт им импульс.
    /// </summary>
    private void FireShotgun()
    {
        for (int i = 0; i < pelletsPerShot; i++)
        {
            float yaw = Random.Range(-spreadAngle, spreadAngle);
            float pitch = Random.Range(-spreadAngle, spreadAngle);

            Quaternion randomRotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 shootDirection = randomRotation * barrelEnd.forward;

            GameObject pellet = Instantiate(pelletPrefab, barrelEnd.position, Quaternion.LookRotation(shootDirection));

            Rigidbody rb = pellet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(shootDirection * pelletForce, ForceMode.Impulse);
            }
        }
    }
}
