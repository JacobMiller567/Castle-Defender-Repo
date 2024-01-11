using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float lightningSpeed;
    [SerializeField] public int lightningDamage;
    [SerializeField] public int maxHits; // Number of enemies bolt can hit
    [SerializeField] private int rangeBetweenTargets; // How far bolts can travel between enemies

    public List<Transform> hitEnemies;
    private Transform target;
    private int numHits = 0; 

    private void Start() 
    {
        hitEnemies = new List<Transform>();
    }

    public void SetTarget(Transform _target)
    {
       target = _target;
    }

    private void FixedUpdate()
    {
        if (numHits >= maxHits) // if max number of enemies have been hit
        {
            Destroy(gameObject);
            return;
        }

        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * lightningSpeed;

        StartCoroutine(MissedShot());
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other == null || other.gameObject == null || other.transform == null || other.gameObject.transform == null) return;

        if (other.gameObject.CompareTag("Enemy") && !hitEnemies.Contains(other.transform))
        {
            if (other.gameObject.name != "7_CorruptKnight(Clone)")
            {
                other.gameObject.GetComponent<EnemyVitals>().TakeDamage(lightningDamage);
            }
            hitEnemies.Add(other.transform);
            numHits++;
            NextTarget();
        }
    }


    private void NextTarget() // Detect enemies inside of circle radius and go towards next closest enemy
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, rangeBetweenTargets, (Vector2)transform.position, 0f, enemyMask);
        float minDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Enemy") && !hitEnemies.Contains(hit.transform))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = hit.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            SetTarget(closestEnemy.transform); // Change target to nearest enemy
        }
    }
        

    IEnumerator MissedShot()
    {
        yield return new WaitForSeconds(1.1f);
        Destroy(gameObject);
    }



}
