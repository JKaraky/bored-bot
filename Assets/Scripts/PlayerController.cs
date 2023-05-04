using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectile;

    public AudioClip shootingSound;
    public AudioClip powerupPickup;
    public AudioClip enemyHit;

    AudioSource playerAudio;

    Animator playerAnim;

    GameManager gameManager;
    SpawnManager spawnManager;

    float speed = 5.0f;
    float zRangeFar = 4.5f;
    float zRangeNear = -12.0f;
    float powerupTime = 2.0f;
    float invincibleTime = 5.0f;

    bool firstShot = false;
    bool canShoot = true;
    bool poweredUp = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.gameManagerInstance;
        spawnManager = SpawnManager.spawnManagerInstance;
        playerAudio = GetComponent<AudioSource>();
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        ContraintPlayerMovement();

        Shoot();
    }

    // To move the player
    void MovePlayer()
    {
        // Move player on depth axis

        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.left * speed * verticalInput * Time.deltaTime);
    }

    // To put boundaries for player movement
    void ContraintPlayerMovement()
    {
        // Limit movement on the depth axis

        if (transform.position.z < zRangeNear)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRangeNear);
        }
        else if (transform.position.z > zRangeFar)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRangeFar);
        }
    }

    // to shoot

    void Shoot()
    {
        // Shooting with no powerup

        if (Input.GetKeyDown(KeyCode.Space) && !poweredUp)
        {
            // First shot
            if(!firstShot && canShoot)
            {
                spawnManager.projectilePool.Get();

                firstShot = true;

                playerAudio.PlayOneShot(shootingSound, 1);
            }
            
            // Second shot

            else if (firstShot && canShoot)
            {
                spawnManager.projectilePool.Get();

                canShoot = false;

                playerAudio.PlayOneShot(shootingSound, 1);

                Invoke("ResetCooldown", 1.0f);
            }
        } 

        // Shooting with powerup
        else if(Input.GetKeyDown(KeyCode.Space) && poweredUp)
        {
            spawnManager.projectilePool.Get();
            playerAudio.PlayOneShot(shootingSound, 1);
        }
    }

    // Effects of enemies colliding with player

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameManager.livesUpdate(-1);
            playerAudio.PlayOneShot(enemyHit, 1);
        }
    }

    //Powerup effects when picked up

    void OnTriggerEnter(Collider other)
    {
        // Ammo powerup
        if (other.gameObject.CompareTag("AmmoPowerup"))
        {
            StartCoroutine(PowerupTime());
            playerAudio.PlayOneShot(powerupPickup, 1);
        }
        // Health powerup
        else if(other.gameObject.CompareTag("HealthPowerup"))
        {
            gameManager.AddLife();
            playerAudio.PlayOneShot(powerupPickup, 1);
        }
        // Time powerup
        else if (other.gameObject.CompareTag("InvinciblePowerup"))
        {
            //StartCoroutine(InvincibleTime());
            StartCoroutine(gameManager.InvisibilityOnOff(invincibleTime));
            playerAudio.PlayOneShot(powerupPickup, 1);
        }
    }

    // cooldown for shooting

    void ResetCooldown()
    {
        firstShot = false;
        canShoot = true;
    }

    // Timer for powerup

    IEnumerator PowerupTime()
    {
        poweredUp = true;
        yield return new WaitForSeconds(powerupTime);
        poweredUp = false;
    }

    /*IEnumerator InvincibleTime()
    {
        gameManager.isInvincible = true;
        gameManager.InvisibilityOnOff();
        yield return new WaitForSeconds(invincibleTime);
        gameManager.isInvincible = false;
        gameManager.InvisibilityOnOff();
    }*/
}
