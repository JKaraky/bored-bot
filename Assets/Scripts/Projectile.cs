using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 20.0f;
    float xRange = 40.0f;

    GameManager gameManager;
    SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Moves projectile

        transform.Translate(Vector3.right * speed * Time.deltaTime);

        //Destroys projectile when out of bounds

        if (transform.position.x > xRange)
        {
            spawnManager.projectilePool.Release(this);
        }
    }

    // Destroy projectile when it hits powerup

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PowerUp>() != null)
        {
            spawnManager.projectilePool.Release(this);
        }
    }

    // Destroy projectile when it hits enemy

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            spawnManager.projectilePool.Release(this);
        }
    }
}
