using UnityEngine;
using System.Collections;

public class ShotgunController : MonoBehaviour
{
    [Header("��������� ���������")]
    [Tooltip("������ ������� �������� (����� � Rigidbody)")]
    [SerializeField] private GameObject pelletPrefab;

    [Tooltip("���������� �������� �� �������")]
    [SerializeField] private int pelletsPerShot = 99;

    [Tooltip("���� �������� � ��������")]
    [SerializeField] private float spreadAngle = 360f;

    [Tooltip("���� ������ ��������")]
    [SerializeField] private float pelletForce = 750f;

    [Header("�������� � ��")]
    [Tooltip("������������ �������� ���������")]
    [SerializeField] private int maxAmmo = 24;

    [Tooltip("������� ���������� �����������")]
    [SerializeField] private int currentAmmo;

    [Tooltip("����� ����� ���������� (���)")]
    [SerializeField] private float cooldownTime = 1f;

    private float nextFireTime = 0f;
    private Transform barrelEnd;

    void Start()
    {
        currentAmmo = maxAmmo;

        if (barrelEnd == null)
        {
            PelletSpawn spawn = FindObjectOfType<PelletSpawn>();
            if (spawn != null)
            {
                barrelEnd = spawn.transform;
            }
            else
            {
                Debug.LogError("PelletSpawn �� ������ �� �����. ���������, ��� �� ������������.");
            }
        }
    }


    void Update()
    {
        if (Input.GetKey(BindingKeysManager.Attack_Key_KEYCODE) && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                FireShotgun();
                currentAmmo--;
                Debug.Log($"�������� ��������: {currentAmmo}");
                nextFireTime = Time.time + cooldownTime;
            }
            else
            {
                Debug.Log("��� �����������! ���� ��������������.");
            }
        }
    }

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
