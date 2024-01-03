using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int spikeDamage;
    [SerializeField] private int spikeHealth;
    private BuildManager build;

    private void Start()
    {
        build = FindObjectOfType<BuildManager>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyVitals e = other.gameObject.GetComponent<EnemyVitals>();
        if (e != null && e.isDead == false && e.canTakeSpikeDamage == true) 
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                e.TakeDamage(spikeDamage);
                e.EnemySpiked();
                spikeHealth -= 1;

                if (spikeHealth <= 0)
                {
                    build.placedTowersColliders.Remove(gameObject.GetComponent<TowerManager>().GetCollider()); // Remove spike from collider list
                    build.currentSpikeCount -= 1;
                    Destroy(gameObject); 

                }
            }
        }
    }
}
