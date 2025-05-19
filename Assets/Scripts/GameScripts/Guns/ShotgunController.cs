using UnityEngine;
using System.Collections;

/// <summary>
/// ������ ���������� �������� �� ���������: ����� 6 �������� (����) � ���������, ���� ����������� � ����������� (��).
/// ��������� � ������� "Shotgun" � Transform ������ � �������� ��������.
/// </summary>
public class ShotgunController : MonoBehaviour
{
    [Header("��������� ���������")]
    [Tooltip("������ ������� �������� (����� � Rigidbody)")]
    public GameObject pelletPrefab;

    [Tooltip("����� ��������� �������� (������ Transform ������)")]
    public Transform barrelEnd;

    [Tooltip("���������� �������� �� �������")]
    public int pelletsPerShot = 6;

    [Tooltip("���� �������� � ��������")]
    public float spreadAngle = 10f;

    [Tooltip("���� ������ ��������")]
    public float pelletForce = 500f;

    [Header("�������� � ��")]
    [Tooltip("������������ �������� ���������")]
    public int maxAmmo = 24;

    [Tooltip("������� ���������� �����������")]
    public int currentAmmo;

    [Tooltip("����� ����� ���������� (���)")]
    public float cooldownTime = 1f;

    private float nextFireTime = 0f;

    void Start()
    {
        // ������������� �������� ���������
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        // �������� ������� ������ "Fire1" (������ ����� ������ ���� ��� Ctrl)
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                FireShotgun();
                currentAmmo--; // ��������� ��������
                Debug.Log($"�������� ��������: {currentAmmo}");
                nextFireTime = Time.time + cooldownTime;
            }
            else
            {
                Debug.Log("��� �����������! ���� ��������������.");
            }
        }
    }

    /// <summary>
    /// ��������� ������� ���������: ������ �������� � ��������� ��������� � ������ �� �������.
    /// </summary>
    private void FireShotgun()
    {
        for (int i = 0; i < pelletsPerShot; i++)
        {
            // ��������� ���� �������� �� ����������� � ���������
            float yaw = Random.Range(-spreadAngle, spreadAngle);
            float pitch = Random.Range(-spreadAngle, spreadAngle);

            // �������� ����������� � ������ ��������
            Quaternion randomRotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 shootDirection = randomRotation * barrelEnd.forward;

            // ������ ��������
            GameObject pellet = Instantiate(pelletPrefab, barrelEnd.position, Quaternion.LookRotation(shootDirection));

            // ������ �������� ����� Rigidbody
            Rigidbody rb = pellet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(shootDirection * pelletForce);
            }
        }
    }
}
