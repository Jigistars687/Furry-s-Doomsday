using UnityEngine;

public class PistolController : MonoBehaviour
{
    [Header("Настройки пистолета")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletsPerShot = 1;
    [SerializeField] private float bulletForce = 750f;

    [Header("Боезапас и КД")]
    [SerializeField] private int maxAmmo = 17;
    [SerializeField] private int currentAmmo;
    [SerializeField] private float cooldownTime = 0.25f;

    [Header("Разброс")]
    [SerializeField] private float aimSpread = 0.16f;
    [SerializeField] private float idleSpread = 0.5f;
    [SerializeField] private float moveSpread = 1.5f;
    [SerializeField] private float airSpread = 3f;

    [Header("Отдача")]
    [Tooltip("Сила подброса ствола (в градусах)")]
    [SerializeField] private float recoilAmount = 25f;
    [Tooltip("Скорость возврата камеры")]
    [SerializeField] private float recoilRecoverySpeed = 10f;

    private float nextFireTime = 0f;
    private Transform barrelEnd;
    private Vector3 currentRecoilRotation;
    private Vector3 targetRecoilRotation;

    // Состояния игрока (привяжи к своему контроллеру)
    public bool isAiming;
    public bool isMoving;
    public bool isInAir;

    void Start()
    {
        currentAmmo = maxAmmo;
        PelletSpawn spawn = FindObjectOfType<PelletSpawn>();
        if (spawn != null) barrelEnd = spawn.transform;
    }

    void Update()
    {
        if (Input.GetKey(BindingKeysManager.Attack_Key_KEYCODE) && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                FirePistol();
                ApplyRecoil();
                currentAmmo--;
                nextFireTime = Time.time + cooldownTime;
            }
        }

        // Плавное возвращение прицела после отдачи
        targetRecoilRotation = Vector3.Lerp(targetRecoilRotation, Vector3.zero, recoilRecoverySpeed * Time.deltaTime);
        currentRecoilRotation = Vector3.Lerp(currentRecoilRotation, targetRecoilRotation, recoilRecoverySpeed * Time.deltaTime);
        Camera.main.transform.localEulerAngles = currentRecoilRotation;
    }

    private void FirePistol()
    {
        float spreadAngle = GetCurrentSpread();

        for (int i = 0; i < bulletsPerShot; i++)
        {
            Vector2 spread = Random.insideUnitCircle * (spreadAngle / 2f);
            Quaternion rotation = Quaternion.Euler(spread.y, spread.x, 0);
            Vector3 shootDirection = rotation * barrelEnd.forward;

            GameObject bullet = Instantiate(bulletPrefab, barrelEnd.position, Quaternion.LookRotation(new Vector3(0,-90,90)));

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(shootDirection * bulletForce, ForceMode.Impulse);
            }
        }
    }

    private float GetCurrentSpread()
    {
        if (isInAir) return airSpread;
        if (isMoving) return moveSpread;
        if (isAiming) return aimSpread;
        return idleSpread;
    }

    private void ApplyRecoil()
    {
        targetRecoilRotation += new Vector3(-recoilAmount, Random.Range(-recoilAmount * 0.5f, recoilAmount * 0.5f), 0);
    }
}
