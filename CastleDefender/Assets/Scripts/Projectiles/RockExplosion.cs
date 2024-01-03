using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RockExplosion : MonoBehaviour
{
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float rockSpeed;
    [SerializeField] public int damage;
    [SerializeField] public int splashDamage;
    [SerializeField] private bool allowRotation;
    [SerializeField] public float explosionRange;

    private Transform target;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * rockSpeed;

        if (allowRotation == true)
        {
            transform.up = direction; 
        }
        StartCoroutine(MissedShot());
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other == null || other.gameObject == null) return;
        if (other.gameObject.CompareTag("Enemy"))
        { 
            other.gameObject.GetComponent<EnemyVitals>().TakeDamage(damage);
            SplashArea();
        }
    }

    private void SplashArea()
    {
        // Create splash damage area where rock collides with enemy
       RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                if (hit.transform == null) return;

                EnemyVitals enemy = hit.transform.GetComponent<EnemyVitals>();
                if (enemy == null) return;

                enemy.TakeDamage(splashDamage); 
                Destroy(gameObject);
            }
        }       
    }

    IEnumerator MissedShot()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

