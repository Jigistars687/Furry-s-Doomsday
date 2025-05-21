using UnityEngine;

public class Pellet : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 10f); 
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Destroy(gameObject); 
    //}
}
