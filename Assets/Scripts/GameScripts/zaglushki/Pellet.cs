using UnityEngine;

public class Pellet : MonoBehaviour
{
    [Tooltip("Префаб следа от дробинки")]
    [SerializeField] private GameObject decalPrefab;

    [Tooltip("Масштаб decal (следа)")]
    [SerializeField] private Vector3 decalScale = new Vector3(0.5f, 0.5f, 0.5f);

    private void Start()
    {
        //Destroy(gameObject, 2.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (decalPrefab != null && collision.contacts.Length > 0 & !collision.gameObject.TryGetComponent<Pellet>(out var _))
        {
            ContactPoint contact = collision.contacts[0];
            GameObject decal = Instantiate(
                decalPrefab,
                contact.point + contact.normal * 0.01f,
                Quaternion.LookRotation(contact.normal) * Quaternion.Euler(90, 0, 0)
            );
            decal.transform.localScale = decalScale; // увеличиваем размер decal
            Destroy(decal, 10f);
        }
        if (!collision.gameObject.TryGetComponent<Pellet>(out var _))
        {
            Destroy(gameObject, 0.02f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<EnemyAI>(out var _))
        {
            //Debug.Log("Hit enemy");
            Destroy(gameObject, 0.01f);
        }
    }
}

