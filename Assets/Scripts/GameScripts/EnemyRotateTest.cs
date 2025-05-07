using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotateTest : MonoBehaviour
{
    [SerializeField] private Transform _Player;
    [SerializeField] private float _MaxDistance = 100f; // Максимальная дистанция, на которой объект будет следовать за игроком
    [SerializeField] private float _ObstacleAvoidanceRadius = 1.5f; // Радиус обхода препятствий
    [SerializeField] private float _Speed = 5f; // Скорость движения
    [SerializeField] private float _RotationSpeed = 5f; // Скорость поворота

    private float _InitialY; // Начальная позиция по оси Y

    void Start()
    {
        // Сохраняем начальную позицию по оси Y
        _InitialY = transform.position.y;
    }

    void Update()
    {
        if (_Player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _Player.position);

            // Проверяем, находится ли игрок в пределах _MaxDistance  
            if (distanceToPlayer <= _MaxDistance)
            {
                Vector3 directionToPlayer = (_Player.position - transform.position).normalized;

                // Проверяем наличие препятствий на пути  
                RaycastHit hit;
                if (Physics.SphereCast(transform.position, _ObstacleAvoidanceRadius, directionToPlayer, out hit, _ObstacleAvoidanceRadius * 2f))
                {
                    // Выводим отладочную информацию
                    Debug.Log($"Обнаружен объект: {hit.collider.name}, IsTrigger: {hit.collider.isTrigger}");

                    // Игнорируем коллайдеры, помеченные как триггеры
                    if (!hit.collider.isTrigger)
                    {
                        // Если есть препятствие, изменяем направление движения  
                        Vector3 hitNormal = hit.normal; // Нормаль поверхности препятствия
                        Vector3 avoidanceDirection = Vector3.Reflect(directionToPlayer, hitNormal).normalized; // Отражаем направление
                        directionToPlayer = Vector3.Lerp(directionToPlayer, avoidanceDirection, 0.5f).normalized; // Плавно меняем направление
                    }
                }

                // Двигаемся в направлении игрока, фиксируя позицию по оси Y
                Vector3 newPosition = transform.position + directionToPlayer * _Speed * Time.deltaTime;
                newPosition.y = _InitialY; // Фиксируем Y
                transform.position = newPosition;

                // Поворачиваемся в сторону игрока, фиксируя ось Y
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                Vector3 fixedRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _RotationSpeed).eulerAngles;
                fixedRotation.z = 0; // Фиксируем Y
                fixedRotation.x = 0; // Фиксируем Y
                transform.rotation = Quaternion.Euler(fixedRotation);
            }
        }
    }
}
