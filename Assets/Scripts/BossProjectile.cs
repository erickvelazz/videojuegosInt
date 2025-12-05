using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public int damage = 1;
    public float lifeTime = 3.0f; // Tiempo de vida en segundos

    void Start()
    {
        // Esto asegura que la bola se destruya sola a los 3 segundos
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Si choca con el Jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject); // Se destruye al pegar
        }
        // Si choca con el suelo o paredes (Default), no hace nada especial, solo rebota
    }
}