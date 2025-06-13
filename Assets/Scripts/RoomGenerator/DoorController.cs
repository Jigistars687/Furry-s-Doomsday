using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("��������� �������� �����")]
    private float openHeight = 8f;         // ��������� ��������� �����
    private float moveSpeed = 3f;          // �������� �������� �����
    private bool isOpen = false;           // ��������� �����

    private Vector3 closedPosition;
    private Vector3 openedPosition;
    private Coroutine moveCoroutine;
    private Coroutine currentRoutine;

    public void OpenDoor()
    {
        if (isOpen) return;

        // ���������� ������� ������� ��� ����� ������
        Vector3 from = transform.position;
        // ���� � ��������� �� openHeight �����
        Vector3 to = from + Vector3.up * openHeight;

        // ��������� ���������� ��������, ���� ����
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(MoveDoor(from, to, 2f));
    }

    public void CloseDoor()
    {
        if (!isOpen) return;

        // ���������� ������� ������� ��� ����� ������
        Vector3 from = transform.position;
        // ���� � ���������� �� 2 * openHeight ���� (��� � ����� ���������)
        Vector3 to = from - new Vector3(0, openHeight, 0);

        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(MoveDoor(from, to, 0.35f));
    }

    private IEnumerator MoveDoor(Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = to;
        isOpen = (to == from + Vector3.up * openHeight);
        currentRoutine = null;
    }


    private System.Collections.IEnumerator MoveDoor(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<playercontrollerBETA>(out var player))
        {
            // ���� ����� ������ � �������, ��������� �����
            if (!isOpen)
            {
                OpenDoor();
            }
        }
    }
}
