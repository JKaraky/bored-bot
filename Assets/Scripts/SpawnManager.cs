using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    public Enemy[] enemies;
    public GameObject powerup;
    public Projectile projectile;

    public ObjectPool<Enemy> enemyPool;
    public ObjectPool<Projectile> projectilePool;

    public ParticleSystem explosionParticles;

    public AudioClip enemyDeath;

    GameManager gameManager;
    PlayerController player;

    AudioSource audioSource;

    float xSpawnPos = 40.0f;
    float zSpawnFar = 2.0f;
    float zSpawnNear = -11.0f;
    float ySpawnPos = 0f;
    [SerializeField] float repeatEnemy = 0.8f;
    float repeatPowerup = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        audioSource = GetComponent<AudioSource>();

        enemyPool = new ObjectPool<Enemy>(CreateAnEnemy, GetEnemy, RemoveEnemy, DestroyEnemy, true, 20, 30);
        projectilePool = new ObjectPool<Projectile>(CreateProjectile, GetProjectile, RemoveProjectile, DestroyProjectile, true, 20, 30);
    }

    //Functions for the enemy pool
    Enemy CreateAnEnemy()
    {
        int randomIndex = Random.Range(0, enemies.Length);
        return Instantiate(enemies[randomIndex]);
    }

    void GetEnemy(Enemy enemy)
    {
        float zSpawnPos = Random.Range(zSpawnFar, zSpawnNear);
        Vector3 spawnPosEnemy = new Vector3(xSpawnPos, ySpawnPos, zSpawnPos);
        enemy.transform.position = spawnPosEnemy;

        enemy.gameObject.SetActive(true);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        audioSource.PlayOneShot(enemyDeath, 1);

        Vector3 enemyLocation = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);
        Quaternion explosionRotation = Quaternion.Euler(0, -90, 0);

        Instantiate(explosionParticles, enemyLocation, explosionRotation);
    }

    void DestroyEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }

    //Functions for the projectile pool
    Projectile CreateProjectile()
    {
        return Instantiate(projectile);
    }

    void GetProjectile(Projectile projectile)
    {
        float offsetX = 2.0f;
        Vector3 projectileSpawnPos = new Vector3(player.transform.position.x + offsetX, player.transform.position.y, player.transform.position.z);
        projectile.transform.position = projectileSpawnPos;
        projectile.gameObject.SetActive(true);
    }

    public void RemoveProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    void DestroyProjectile(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    // For spwning powerup
    void SpawnPowerup()
    {
        int randomIndex = Random.Range(0, enemies.Length);
        float zSpawnPos = Random.Range(zSpawnFar, zSpawnNear);
        Vector3 spawnPosPowerup = new Vector3(xSpawnPos, ySpawnPos, zSpawnPos);
        Instantiate(powerup, spawnPosPowerup, powerup.transform.rotation);
    }

    // Timer for enemies to spawn
    IEnumerator EnemySpawner()
    {
        while(!gameManager.gameOver)
        {
            yield return new WaitForSeconds(repeatEnemy);
            enemyPool.Get();
        }
    }

    // Timer for powerup to spawn
    IEnumerator PowerupSpawner()
    {
        while (!gameManager.gameOver)
        {
            yield return new WaitForSeconds(repeatPowerup);
            SpawnPowerup();

        }
    }


    // What spawn powerups and enemies
    public void SpawnEverything()
    {
        if (!gameManager.gameOver)
        {
            StartCoroutine(EnemySpawner());
            StartCoroutine(PowerupSpawner());
        }
    }
}
