using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //configuration parameters
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    //laser gameObject
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileVerticalSpeed = 10f;
    [SerializeField] float projectileFiringTime = 0.1f;

    //initialize Coroutine to control it in shooting
    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    void Start()
    {
        SetUpMoveBoundaries();
        
    }

    void Update()
    {
        Move();
        Fire();
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            //Quaternion means "no rotation" - the object is perfectly aligned with the world or parent axes.
            //Instantiate(what, where, rotation)
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileVerticalSpeed);

            yield return new WaitForSeconds(projectileFiringTime);
        }
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
           firingCoroutine = StartCoroutine(FireContinuously());
        }

        //stops shooting on releasing the button
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private void Move()
    { 
        //allows for horizontal axis move: left to right
        //Time.deltaTime makes frame rate independant of pc speed
        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var newXPosition = Mathf.Clamp( transform.position.x + deltaX, xMin, xMax);

        //allows for vertical axis move: up and down
        float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newYPosition = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPosition, newYPosition); 
    }

    private void SetUpMoveBoundaries()
    {
        //assign main camera
        Camera gameCamera = Camera.main;

        //ViewportToWorldPoint sets the position of the whole game window
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

}

