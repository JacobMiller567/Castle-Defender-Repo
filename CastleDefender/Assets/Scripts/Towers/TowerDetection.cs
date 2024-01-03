using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TowerDetection : MonoBehaviour
{
    [SerializeField] public float reloadSpeed; 
    [SerializeField] public float detectionRange;
    [SerializeField] public int towerLevel;
    private bool canShoot = true;
    private bool detected;
    [SerializeField] private float rotationSpeed; 
    [SerializeField] private bool useLightning; 
    [SerializeField] private bool useSplash;

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform arrowTip;
    [SerializeField] private Transform crossbow;

    [SerializeField] private TowerManager manage;
    [SerializeField] private UpgradeTower upgrade;

    //Animator anim;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private new AudioSource audio;


    public bool changeTargetType = false;
    private bool animDelay = true;
    private float radiusHolder;
    private float speedHolder;
    public bool towerBlessed = false;


    public Color color = Color.red;

    void Start()
    {
        radiusHolder = detectionRange;
        speedHolder = reloadSpeed;
        manage.UpdateTowerStats(detectionRange, reloadSpeed);
    }

    void Update()
    {
        FaceEnemy();
        if (detected == true)
        {
            ShootEnemy();
        }
    }

    public float GetTowerRadius()
    {
        return detectionRange;
    }

    private Transform GetClosestEnemy(GameObject[] enemies) { 
        Transform closest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = manage.transform.position;
        foreach (GameObject enemy in enemies) {
            float dist = Vector3.Distance(enemy.transform.position, currentPos);
            if (dist < minDist) {
                closest = enemy.transform;
                minDist = dist;
            }
        }
        return closest;
    }


    private void FaceEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0 && manage.towerIsPurchased == true)
        {
            Transform closestEnemy = GetClosestEnemy(enemies);
            Vector3 direction = closestEnemy.position - manage.transform.position; 

            if (!useLightning)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
                crossbow.rotation = Quaternion.RotateTowards(crossbow.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            if (Vector2.Distance(closestEnemy.position, manage.transform.position) <= detectionRange) 
            {
                detected = true;
            }
            else
            {
                detected = false;
            }
        }

    }

    private void ShootEnemy() 
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (canShoot && enemies.Length > 0 && manage.towerIsPurchased == true)
        {
           Transform closestEnemy = GetClosestEnemy(enemies);
            if (anim != null)
            {
                anim.SetTrigger("Shoot");
            }
            if (animDelay == false)
            {
                GameObject arrow = Instantiate(projectile, arrowTip.position, Quaternion.identity);

                if (useLightning == true)
                {
                    LightningBolt proj = arrow.GetComponent<LightningBolt>();
                    audio.Play();
                    proj.SetTarget(closestEnemy);
                    StartCoroutine(Reload());
                    animDelay = true;
                }
                
                if (useSplash == true)
                {
                    RockExplosion proj = arrow.GetComponent<RockExplosion>();
                    audio.Play();
                    proj.SetTarget(closestEnemy);
                    StartCoroutine(Reload());
                    animDelay = true;
                }          
                if (useLightning != true && useSplash != true)
                {
                    Arrow proj = arrow.GetComponent<Arrow>();
                    audio.Play();
                    proj.SetTarget(closestEnemy);
                    StartCoroutine(Reload());
                    animDelay = true;           
                }
            }
        }
    }


    public void AnimationShoot()
    {
        animDelay = false;
    }
    public void AnimationStop()
    {
        anim.ResetTrigger("Shoot");
    }


    public void ChangeTarget() 
    {
        //changeTargetType = !changeTargetType;
        //Debug.Log("Target Changed" + gameObject.name);
    }


    public void BoostTower(float boostedRange, float boostedSpeed)
    {
        detectionRange = Mathf.Round((radiusHolder * boostedRange)* 100f) / 100f;
        reloadSpeed = speedHolder - (Mathf.Round((speedHolder * boostedSpeed)* 100f) / 100f);
        manage.BlessedRangeMultiplier = boostedRange;
        manage.BlessedSpeedMultiplier = boostedSpeed; 
        manage.UpdateTowerStats(detectionRange, reloadSpeed);
    }
    public void ResetTower()
    {
        detectionRange = radiusHolder;
        reloadSpeed = speedHolder;
        manage.TowerRange = radiusHolder;
        manage.TowerSpeed = speedHolder;
    }

    



    IEnumerator Reload()
    {
        canShoot = false;
        yield return new WaitForSeconds(reloadSpeed);
        canShoot = true;

    }


}
