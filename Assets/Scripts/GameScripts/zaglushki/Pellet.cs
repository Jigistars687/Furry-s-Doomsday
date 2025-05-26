using UnityEngine;

public class Pellet : MonoBehaviour
{
    private Enemy_Stats _here_stats;
    private Shotgun_stats Shotgun_damage;
    private void Start()
    {
        Destroy(gameObject, 10f);
        _here_stats = new Enemy_Stats();
        Shotgun_damage = new Shotgun_stats();
    }

    private void OnCollisionEnter(Collision collision)
    {
 //       if (!collision.gameObject.TryGetComponent<Pellet>(out var _))
 //       {
//            Destroy(gameObject, 0.3f);
//        }
        if (collision.gameObject.TryGetComponent<EnemyAI>(out var enemyAI))
        {
            Debug.Log($"Enemy Hit by a pellet!\n{_here_stats.Health}");
            _here_stats.TakeDamage(Shotgun_damage.DamagePerPellet);
            if (_here_stats.Health <= 0)
            {
                enemyAI.gameObject.SetActive(false);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
            
    }
}
