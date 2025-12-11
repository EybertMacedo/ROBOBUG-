using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("El Prefab del proyectil/enemigo con EnemyMovement2.cs.")]
    public GameObject projectilePrefab;
    
    [Tooltip("Intervalo entre la creación de proyectiles.")]
    public float spawnInterval = 1.0f;

    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            nextSpawnTime = Time.time + spawnInterval;
        }
    }
}