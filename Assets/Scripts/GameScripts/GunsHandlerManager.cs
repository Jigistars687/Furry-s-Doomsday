using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GunsHandlerManager : MonoBehaviour
{
    [Header("Настройка префабов для смены оружия")]
    [Tooltip("список оружий (пистолет, дробовик, пулемет, хил, т.д.)")]
    [SerializeField] private List<GameObject> weaponPrefabs = new List<GameObject>();

    [Tooltip("Пустой обьект где будет появлятся само оружие")]
    [SerializeField] private Transform weaponSpawnPoint;

    private int currentWeaponIndex = 0;
    private GameObject currentWeaponInstance;

    void Start()
    {
        if (weaponPrefabs.Count == 0 || weaponSpawnPoint == null)
        {
            Debug.LogError("ОШИБКА ПРИ СМЕНЕ ОРУЖИЯ: Список префабов или точка спавна не назначены");
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

        Debug.Log($"скролл на: {scroll}");

        // Считаем новый индекс, не меняя сразу текущий
        int newIndex = (currentWeaponIndex + (scroll > 0f ? 1 : -1) + weaponPrefabs.Count) % weaponPrefabs.Count;
        SwitchToWeapon(newIndex);
    }

    private void SwitchToWeapon(int newIndex)
    {
        // Если индекс не изменился — ничего не делаем
        if (newIndex == currentWeaponIndex)
            return;

        // Удаляем старое оружие
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