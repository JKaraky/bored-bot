using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed;

    GameManager gameManager;
    SpawnManager spawnManager;

    float xRange = -40.0f;
    float yRange = -15.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Makes gameobject move to the left

        transform.Translate(Vector3.left * speed * Time.deltaTime);

        //destroys whatever goes out of bounds on x and y axis

        if (transform.position.x < xRange || transform.position.y < yRange)
        {
            spawnManager.enemyPool.Release(this);
            gameManager.livesUpdate(-1);
        }
    }
    // Interactions between enemy and other objects
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Powerup"))
        {
            Destroy(collision.gameObject);
        } else if (collision.gameObject.CompareTag("Projectile"))
        {
            spawnManager.enemyPool.Release(this);

            if (!gameManager.gameOver)
            {
                gameManager.score++;
            }
        } else if (collision.gameObject.CompareTag("Player"))
        {
            spawnManager.enemyPool.Release(this);
        }
    }
}
