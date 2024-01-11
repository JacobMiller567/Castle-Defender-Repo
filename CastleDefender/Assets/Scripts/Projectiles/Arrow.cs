using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float arrowSpeed;
    [SerializeField] public int damage;
    [SerializeField] private bool allowRotation;

    private Transform target;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target) return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * arrowSpeed;

        if (allowRotation == true)
        {
            transform.up = direction; 
        }
        StartCoroutine(MissedShot());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other == null || other.gameObject == null) return;
        if (other.gameObject.CompareTag("Path") || other.gameObject.CompareTag("Obstacles")) return;

        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyVitals e = other.gameObject.GetComponent<EnemyVitals>();
            if (e.isDead == false)
            {
                e.TakeDamage(damage);
                Destroy(gameObject); 
            }
        }
        Destroy(gameObject);
    }

    IEnumerator MissedShot()
    {
        yield return new WaitForSeconds(1f); 
        Destroy(gameObject);
    }
}
