using UnityEngine;

public class CoinCollectible : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Moneda recolectada!");

            if(UIManager.Instance != null) 
            {
                UIManager.Instance.AddCoin();
            }

            Destroy(gameObject);
        }
    }
}