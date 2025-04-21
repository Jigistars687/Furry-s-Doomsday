using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GunsHandlerManager : MonoBehaviour
{
    [Header("��������� �������� ��� ����� ������")]
    [Tooltip("������ ������ (��������, ��������, �������, ���, �.�.)")]
    [SerializeField] private List<GameObject> weaponPrefabs = new List<GameObject>();

    [Tooltip("������ ������ ��� ����� ��������� ���� ������")]
    [SerializeField] private Transform weaponSpawnPoint;

    private int currentWeaponIndex = 0;
    private GameObject currentWeaponInstance;

    void Start()
    {
        if (weaponPrefabs.Count == 0 || weaponSpawnPoint == null)
        {
            Debug.LogError("������ ��� ����� ������: ������ �������� ��� ����� ������ �� ���������");
            enabled = false;
            return;
        }
        SpawnWeaponAt(currentWeaponIndex);
    }

    void Update()
    {
        HandleNumberKeyInput();
        HandleMouseScrollInput();
    }

    private void HandleNumberKeyInput()
    {
        for (int i = 0; i < weaponPrefabs.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchToWeapon(i);
            }
        }
    }

    private void HandleMouseScrollInput()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Approximately(scroll, 0f))
            return;

        Debug.Log($"������ ��: {scroll}");

        // ������� ����� ������, �� ����� ����� �������
        int newIndex = (currentWeaponIndex + (scroll > 0f ? 1 : -1) + weaponPrefabs.Count) % weaponPrefabs.Count;
        SwitchToWeapon(newIndex);
    }

    private void SwitchToWeapon(int newIndex)
    {
        // ���� ������ �� ��������� � ������ �� ������
        if (newIndex == currentWeaponIndex)
            return;

        // ������� ������ ������
        if (currentWeaponInstance != null)
            Destroy(currentWeaponInstance);

        currentWeaponIndex = newIndex;
        SpawnWeaponAt(currentWeaponIndex);
    }

    private void SpawnWeaponAt(int index)
    {
        GameObject prefab = weaponPrefabs[index];
        currentWeaponInstance = Instantiate(prefab, weaponSpawnPoint.position, weaponSpawnPoint.rotation, weaponSpawnPoint);
    }
}