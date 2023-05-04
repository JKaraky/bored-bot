using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager spawnManagerInstance;

    [SerializeField]
    [Header ("Enemies")]
    public Enemy[] enemies;
    public ObjectPool<Enemy> enemyPool;
    public float repeatEnemy = 0.8f;

    [SerializeField]
    [Header("Power ups")]
    public PowerUp[] powerups;
    public GameObject powerup;
    public ObjectPool<PowerUp> powerupPool;
    public float repeatPowerup = 5.0f;

    [SerializeField]
    [Header("Projectiles")]
    public Projectile projectile;
    public ObjectPool<Projectile> projectilePool;

    public ParticleSystem explosionParticles;

    public AudioClip enemyDeath;

    GameManager gameManager;
    public GameObject player;

    AudioSource audioSource;

    float xSpawnPos = 40.0f;
    float zSpawnFar = 2.0f;
    float zSpawnNear = -11.0f;
    float ySpawnPos = 0f;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (spawnManagerInstance != null && spawnManagerInstance != this)
        {
            Destroy(this);
        }
        else
        {
            spawnManagerInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.gameManagerInstance;

        audioSource = GetComponent<AudioSource>();

        enemyPool = new ObjectPool<Enemy>(CreateAnEnemy, GetEnemy, RemoveEnemy, DestroyEnemy, true, 20, 30);
        projectilePool = new ObjectPool<Projectile>(CreateProjectile, GetProjectile, RemoveProjectile, DestroyProjectile, true, 20, 30);
        powerupPool = new ObjectPool<PowerUp>(CreatePowerup, GetPowerup, RemovePowerup, DestroyPowerup, true, 10, 20);
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

    // Functions for powerup pool

    PowerUp CreatePowerup()
    {
        int randomIndex = Random.Range(0, powerups.Length);
        return Instantiate(powerups[randomIndex]);
    }

    void GetPowerup(PowerUp powerup)
    {
        float zSpawnPos = Random.Range(zSpawnFar, zSpawnNear);
        Vector3 spawnPosPowerup = new Vector3(xSpawnPos, ySpawnPos, zSpawnPos);
        powerup.transform.position = spawnPosPowerup;

        powerup.gameObject.SetActive(true);
    }

    public void RemovePowerup(PowerUp powerup)
    {
        powerup.gameObject.SetActive(false);
    }

    void DestroyPowerup(PowerUp powerup)
    {
        Destroy(powerup.gameObject);
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
            powerupPool.Get();

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
