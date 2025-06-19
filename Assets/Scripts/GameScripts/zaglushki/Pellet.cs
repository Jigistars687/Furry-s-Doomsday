using UnityEngine;
using static UnityEngine.UI.Image;

// Ensure the EnemyStats class is defined and inherits from MonoBehaviour  
public class EnemyStats : MonoBehaviour
{
    public float Health { get; set; }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }
}

public class Pellet : MonoBehaviour
{
    private Enemy_Stats _here_stats;
    private Shotgun_stats Shotgun_damage;

    private Vector3 origin;
    private Vector3 direction;
    private float maxDistance = 100f;

    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }
    private void Start()
    {
        Destroy(gameObject, 10f);
        _here_stats = new Enemy_Stats();
        Shotgun_damage = new Shotgun_stats();
        origin = transform.position; // Начальная позиция пули  
        direction = transform.forward; // Направление пули  
    }

    private void Update()
    {
        // Информация о попадании  
        RaycastHit hit;
        // Запуск Raycast  
        Debug.DrawRay(transform.position, transform.forward, Color.red, 0f);
        // Проверка попадания в объект
        if (Physics.Raycast(origin, direction, out hit, maxDistance))
        {
            var hittenGameObject = hit.collider.name;
            if (hittenGameObject == "Enemy 1")
            {
                if (hit.collider.gameObject.TryGetComponent<EnemyAI>(out var enemyAI))
                {
                    if (hit.collider.gameObject.TryGetComponent<Enemy_Stats>(out var enemyStats))
                    {
                        enemyStats.TakeDamage(Shotgun_damage.DamagePerPellet);
                        Debug.Log($"Enemy Hit by a pellet!\n{_here_stats.Health}");
                        if (enemyStats.Health <= 0)
                        {
                            enemyAI.gameObject.SetActive(false);
                            Destroy(gameObject);
                        }
                        Destroy(gameObject, 0.3f);
                    }
                }
            }
            // Можно проверить тег, компонент и т.д.  
            Destroy(gameObject, 0.3f);
        }
        
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (!collision.gameObject.TryGetComponent<Pellet>(out var _))
    //    {
    //        if (collision.gameObject.TryGetComponent<EnemyAI>(out var enemyAI))
    //        {
    //            Debug.Log($"Enemy Hit by a pellet!\n{_here_stats.Health}");
    //            _here_stats.TakeDamage(Shotgun_damage.DamagePerPellet);
    //            if (_here_stats.Health <= 0)
    //            {
    //                enemyAI.gameObject.SetActive(false);
    //            }
    //            Destroy(gameObject);
    //        }
    //        Destroy(gameObject, 0.3f);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {

    }
}
