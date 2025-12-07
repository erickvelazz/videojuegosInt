using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 1;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        BossFinalController boss = other.GetComponent<BossFinalController>();
        
        if (boss != null)
        {
            boss.TakeDamage(); 
            Destroy(gameObject); 
        }
        else if (!other.CompareTag("Player") && !other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}