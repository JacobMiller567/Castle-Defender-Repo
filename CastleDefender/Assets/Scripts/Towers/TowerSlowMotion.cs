using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TowerSlowMotion : MonoBehaviour
{
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] public float reloadSpeed;
    [SerializeField] public float freezeRadius;
    [SerializeField] public int towerLevel;
    [SerializeField] public float freezeTime;
    [SerializeField] public float slowAmount;
    [SerializeField] private new AudioSource audio;

    private float timeUntilFire;
    private float radiusHolder;
    private float speedHolder;

    [SerializeField] private TowerManager manage;

    void Start()
    {
        radiusHolder = freezeRadius;
        speedHolder = reloadSpeed;
        manage.UpdateTowerStats(freezeRadius, reloadSpeed);
    }

    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / reloadSpeed)
        {
            FreezeEnemies();
            timeUntilFire = 0f;
        }
    }

    public float GetFreezeRadius()
    {
        return freezeRadius;
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, freezeRadius, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0 && manage.towerIsPurchased == true)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if (hit.transform == null) return;
                EnemyMovement enemyMove = hit.transform.GetComponent<EnemyMovement>();
                audio.Play();
                enemyMove.UpdateSpeed(slowAmount);

                StartCoroutine(ResetEnemySpeed(enemyMove)); 
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement enemy)
    {
        yield return new WaitForSeconds(freezeTime);
        enemy.ResetSpeed();
    }

    public void BoostTower(float boostedRange, float boostedSpeed)
    {
        freezeRadius = radiusHolder * boostedRange;
        reloadSpeed = (1 + boostedSpeed) * speedHolder;
        manage.UpdateTowerStats(freezeRadius, reloadSpeed);
    }
    public void ResetTower()
    {
        freezeRadius = radiusHolder;
        reloadSpeed = speedHolder;
        manage.TowerRange = radiusHolder;
        manage.TowerSpeed = speedHolder;
    }
}
