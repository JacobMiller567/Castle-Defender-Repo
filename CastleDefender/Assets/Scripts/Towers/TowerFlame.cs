using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TowerFlame : MonoBehaviour
{
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] public float reloadSpeed;
    [SerializeField] public float flameRadius; 
    [SerializeField] public int towerLevel;
    [SerializeField] public float burnTime; 
    [SerializeField] public float burnSpeed; 
    [SerializeField] public int burnDamage; 
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private new AudioSource audio;

    private float timeUntilFire;
    private float radiusHolder;
    private float speedHolder;

    [SerializeField] private TowerManager manage;


    void Start()
    {
        radiusHolder = flameRadius;
        speedHolder = reloadSpeed;
        manage.UpdateTowerStats(flameRadius, reloadSpeed);
    }

    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / reloadSpeed)
        {
            BurnEnemies();
            timeUntilFire = 0f;
        }
    }

    public float GetFlameRadius()
    {
        return flameRadius;
    }

    private void BurnEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, flameRadius, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0 && manage.towerIsPurchased == true)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                if (hit.transform == null) return;
                
                if(hit.transform.name == "7_CorruptKnight(Clone)") return; // Prevent Corrupt Knight from taking burn damage!
                EnemyVitals enemyHealth = hit.transform.GetComponent<EnemyVitals>();
                if (enemyHealth == null || enemyHealth.isDead == true) return;
                particle.Play();
                audio.Play();
                enemyHealth.TakeDamage(burnDamage);
                if (enemyHealth.isBurned == true) return;
                enemyHealth.EnemyIsBurned(); 

                StartCoroutine(ApplyBurn(enemyHealth));
                StartCoroutine(StopEnemyBurn(enemyHealth));
            }
        }
    }

    private IEnumerator ApplyBurn(EnemyVitals enemy)
    {
        while (enemy.isBurned && enemy.isDead == false)
        {
            yield return new WaitForSeconds(burnSpeed);
            if (enemy == null) break;
            enemy.TakeDamage(burnDamage);
        }
    }

    private IEnumerator StopEnemyBurn(EnemyVitals enemy)
    {
        yield return new WaitForSeconds(burnTime);
        enemy.isBurned = false;
    }

    public void BoostTower(float boostedRange, float boostedSpeed)
    {
        flameRadius = radiusHolder * boostedRange;
        reloadSpeed = (1 + boostedSpeed) * speedHolder;
        manage.UpdateTowerStats(flameRadius, reloadSpeed);
    }
    public void ResetTower()
    {
        flameRadius = radiusHolder;
        reloadSpeed = speedHolder;
        manage.TowerRange = radiusHolder;
        manage.TowerSpeed = speedHolder;
    }

}
