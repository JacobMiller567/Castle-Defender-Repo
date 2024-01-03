using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balls : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    private Transform target;

    public void Damage(float damage){}

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
        StartCoroutine(MissedShot());
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Damageable damagable = other.gameObject.GetComponent<Damageable>();
        damagable?.Damage(damage);
        Destroy(gameObject); 
    }

    IEnumerator MissedShot()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

