using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatWall : MonoBehaviour
{
    GameManager gameManager;

    float speed = 2.0f;
    float width;
    float scale;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        // We get starting position

        startPos = transform.position;

        // We get the midpoint and multiply it with scale of object to get accurate position of midpoint

        scale = transform.localScale.x;
        width = (GetComponent<BoxCollider>().size.x / 2) * scale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameManager.gameOver)
        {
            // Move object to the left

            transform.Translate(Vector3.left * speed * Time.deltaTime);

            // Reset position when object's midpoint reaches starting point

            if (transform.position.x < startPos.x - width)
            {
                transform.position = startPos;
            }

        }
    }
}
