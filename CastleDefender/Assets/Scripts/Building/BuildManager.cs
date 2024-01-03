using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [SerializeField] public GameObject[] prefabTowers; // Holder for all types of towers
    [SerializeField] public BoxCollider2D[] pathColliders; // Colliders needed for paths
    [SerializeField] public BoxCollider2D[] obstacleColliders; // Colliders needed for obstacles
    public List<BoxCollider2D> placedTowersColliders; // Collider holder for placed towers
    
    [SerializeField] private LayerMask pathMask;
    private bool isDragging;
    private GameObject currentTower;
    private Vector3 offset;
    private Vector3 screenPoint;
    [SerializeField] private SoundManager soundManager;

    public bool towerPurchased;
    public int currentSpikeCount = 0;
    public int maxSpikeCount = 9;
    public bool boughtWisp = false;


    void Update()
    {
        if (isDragging)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            currentTower.transform.position = curPosition;

            currentTower.transform.position = new Vector3(currentTower.transform.position.x, currentTower.transform.position.y, 0);
        
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                
                bool validPath = CheckValidLocation(); // Check if tower is on path or not
                bool onObstacle = CheckForObstacles(); // Check if tower is touching an obstacle
                bool onOtherTower = CheckTowerCollision(); // Check if tower is touching another tower;
                bool spikeTower = currentTower.GetComponent<TowerManager>().isSpike; // Check if tower is a spike

                if (validPath == true && spikeTower == false && onObstacle == false && onOtherTower == false) // if tower is on valid location
                {
                    isDragging = false;
                    towerPurchased = false;
                    currentTower.GetComponent<TowerManager>().towerIsPurchased = true; // Allow placed towers to shoot
                }
                if (spikeTower == true && validPath == false && onObstacle == false && onOtherTower == false) // if spike is on the path
                {
                    isDragging = false;
                    towerPurchased = false;
                    currentTower.GetComponent<TowerManager>().towerIsPurchased = true; // Allow placed spike to damage
                    currentSpikeCount += 1;
                }
            }
        }
    }


    public bool CheckValidLocation()
    {
        // Make it so buildings can not be spawned on paths
        BoxCollider2D TowerCollider = currentTower.GetComponent<TowerManager>().GetCollider();

        foreach (BoxCollider2D pathCollider in pathColliders) // Check if tower is touching any path colliders
        {
            if (TowerCollider.IsTouching(pathCollider))
            {
                return false; // Tower is not on valid location
            }
        }
        return true; // Tower is on valid location
    }

    public bool CheckForObstacles()
    {
        // Make it so buildings can not be spawned on obstacles
        BoxCollider2D TowerCollider = currentTower.GetComponent<TowerManager>().GetCollider();

        foreach (BoxCollider2D obstacleCollider in obstacleColliders) // Check if tower is touching any path colliders
        {
            if (TowerCollider.IsTouching(obstacleCollider)) 
            {
                return true; // Tower is touching a obstacle
            }
        }
        return false; // Tower is not touching an obstacle
    }

    public bool CheckTowerCollision()
    {
        // Make it so buildings can not be spawned on other towers
        BoxCollider2D TowerCollider = currentTower.GetComponent<TowerManager>().GetCollider();

        if (!placedTowersColliders.Contains(TowerCollider)) // if tower has not already been added to list
        {
            placedTowersColliders.Add(TowerCollider);
        }

        foreach (BoxCollider2D prefabColliders in placedTowersColliders)
        {
            if (TowerCollider.IsTouching(prefabColliders)) // if tower is touching any other tower
            {
                Debug.Log("Cannot place tower on other tower!");
                return true; // Tower is touching another tower
            }
        }
        return false; // Tower is not on another tower
    }

    public void SpawnBuilding(int num) // Spawn building type
    {
        if (towerPurchased == true)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentTower = Instantiate(prefabTowers[num], mousePosition, Quaternion.identity);
            offset = currentTower.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            isDragging = true;
            soundManager.MuteTowerSounds(); // Mute placed tower if games mute toggle is active
        }
    }

    public void GetTowerAudio() 
    {
        placedTowersColliders[0].GetComponent<AudioSource>();
    }

}
