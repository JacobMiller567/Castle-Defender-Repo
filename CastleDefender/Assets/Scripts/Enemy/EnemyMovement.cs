using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [Header("Attributes")]
    [SerializeField] public float speed;
    public bool LevelTwo;

    private Transform target;
    public int pathIndex = 0; 
    private float speedHolder;
    private bool isSlow;
    private Transform[] newPathTransform;
    private bool topPath;
    private bool middlePath;
    private bool bottomPath;

    private void Start()
    {
        speedHolder = speed;
        
        if (LevelTwo == false)
        {
            target = LevelManager.main.Waypoints[pathIndex];
        }
        if (LevelTwo == true)
        {
            ChooseNewPath();
        }
    }

    private void Update()
    {

        if (LevelTwo == false)
        {
            if (Vector2.Distance(target.position, transform.position) <= 0.1f)
            {
                pathIndex++;
                
                if (pathIndex == LevelManager.main.Waypoints.Length)
                {
                    return;
                }
                else
                {
                    target = LevelManager.main.Waypoints[pathIndex];
                }
            }
        }

        if (LevelTwo == true)
        {
            if (Vector2.Distance(target.position, transform.position) <= 0.1f)
            {
                pathIndex++;
                
                if (pathIndex == newPathTransform.Length)
                {
                    return;
                }
                else
                {
                    target = newPathTransform[pathIndex]; 
                }
            }
        }   
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position);
        float distance = direction.magnitude;
        Vector2 velocity = direction * (speed / distance);
        rb.velocity = velocity;

        if (LevelTwo == false)
        {
            if (pathIndex == 8) 
            {
                transform.rotation = Quaternion.Euler(0, 180, 0); // Rotates enemy to the left
            }
            if (pathIndex == 17) 
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Rotates enemy to the right
            } 
        }
        if (LevelTwo == true)
        {
            if (topPath == true) 
            {
                if (pathIndex == 6) 
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0); // Rotates enemy to the left
                } 
            }
            if (middlePath == true) 
            {
                if (pathIndex == 3) 
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0); // Rotates enemy to the left
                }               
            }
            if (bottomPath == true) 
            {
                if (pathIndex == 7) 
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0); // Rotates enemy to the left
                }    
            }
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 0); 
    }


    public void ChooseNewPath() // Change destination to new path
    {
        pathIndex = 0; 
        float newPath = Random.Range(0, 5); 
        if (newPath == 0) // 20%
        {
            // Up is new path
            target = LevelManager.main.TopPath[pathIndex];
            newPathTransform = LevelManager.main.TopPath;
            topPath = true;
        }
        if (newPath == 1 || newPath == 2) // 40%
        {
            // Middle is new path
            target = LevelManager.main.MiddlePath[pathIndex];
            newPathTransform = LevelManager.main.MiddlePath;
            middlePath = true;
        }
        if (newPath == 3 || newPath == 4) // 40%
        {
            // Down is new path
            target = LevelManager.main.BottomPath[pathIndex];
            newPathTransform = LevelManager.main.BottomPath;
            bottomPath = true;
        }
    }


    public void UpdateSpeed(float newSpeed)
    {
        if (isSlow == false)
        {
            isSlow = true;
            speed = newSpeed * speed;
        }
    }

    public void ResetSpeed()
    {
        speed = speedHolder;
        isSlow = false;
    }

}
