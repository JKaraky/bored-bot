using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLeft : MonoBehaviour
{
    [SerializeField] float speed;

    float xRange = -40.0f;
    float yRange = -15.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        // Makes gameobject move to the left

        transform.Translate(Vector3.left * speed * Time.deltaTime);

        //destroys whatever goes out of bounds on x and y axis

        if (transform.position.x < xRange || transform.position.y < yRange)
        {
            Destroy(gameObject);
        }
    }
}
