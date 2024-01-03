using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(EnemyVitals))]
public class EnemyVitals : MonoBehaviour
{
  public bool isBoss;
  [SerializeField] private int enemyHealth;
  [SerializeField] private int moneyDropped;
  [SerializeField] private float enemySpeed;
  [SerializeField] private int resetHealthValue;
  [SerializeField] private int resetMoneyValue;

  public bool isDead = false;
  private int maxHealth;
  public PlayerStats money;
  public bool isBurned = false;
  public bool canTakeSpikeDamage = true;

  [SerializeField] private int bossSpawnAmount; // Amount of minions boss can spawn

  [SerializeField] private GameObject damagePoints;

  BossHealthbar healthbar; // Used only by king boss

  public bool showEnemyHealth;
  [SerializeField] private GameObject healthbarHolder;
  EnemyHealthbar enemyHealthbar;

  [SerializeField] private Animator anim; // Only used by Ogre Boss
  public bool isOgreBoss;
  private bool usedSpecialMove = false;


    private void Start()
    {
      gameObject.tag = "Enemy";
      maxHealth = enemyHealth;
      money = FindObjectOfType<PlayerStats>();

      if (money.HardMode == true)
      {
        bossSpawnAmount = 12;
      }

      if (isBoss) 
      {
        if (money.HardMode == true)
        {
          enemyHealth = Mathf.RoundToInt(enemyHealth * 1.25f); // 25% increased health
        }
        healthbar = FindObjectOfType<BossHealthbar>();
        healthbar.DisplayMaxHealth(enemyHealth); 
        healthbar.DisplayHealth(enemyHealth);

      }

      if (showEnemyHealth == true)
      {
        enemyHealthbar = gameObject.GetComponentInChildren<EnemyHealthbar>(); 
        enemyHealthbar.DisplayMaxHealth(maxHealth); 
        enemyHealthbar.DisplayHealth(enemyHealth); 
      }
    }

    public void TakeDamage(int dmg)
    {
      enemyHealth -= dmg;
      GameObject floatingDamage = Instantiate(damagePoints, transform.position, Quaternion.identity) as GameObject;
      floatingDamage.transform.GetChild(0).GetComponent<TMP_Text>().text = dmg.ToString();


      if (isBoss) 
      {
        healthbar.DisplayHealth(enemyHealth); 
      }

      if (showEnemyHealth == true) 
      {
        enemyHealthbar.DisplayHealth(enemyHealth);
      }



      if (isBoss && enemyHealth <= 350 && usedSpecialMove == false) 
      {
        if (isOgreBoss == true)
        {
          anim.SetBool("enraged", true);
          usedSpecialMove = true;
          enemyHealth += Mathf.RoundToInt(maxHealth * 0.25f); // Boost ogres health
          return;
        }

        EnemySpawner spawn = FindObjectOfType<EnemySpawner>();
        spawn.SpawnAbility(transform, gameObject.GetComponent<EnemyMovement>().pathIndex, gameObject.GetComponent<EnemyMovement>().LevelTwo);
        bossSpawnAmount --;

        if (bossSpawnAmount <= 0)
        {
          usedSpecialMove = true;
        }
      }
      if (enemyHealth <= 0 && !isDead)
      {
        isDead = true;
        money.PlayerMoney += moneyDropped;
        EnemySpawner.onEnemyDestroy.Invoke();  // Call listener in Spawner
        Destroy(gameObject); 
      }
    }

    public int GetEnemyHealth()
    {
      return resetHealthValue;
    }

    public int GetCurrentHealth()
    {
      return enemyHealth;
    }

    public void IncreaseHealth(int health)
    {
      maxHealth = health;
      enemyHealth = health;
    }
    public void IncreaseMoney(int enemyMoney)
    {
      moneyDropped += enemyMoney;
    }
    public void DecreaseMoney(int enemyMoney)
    {
      moneyDropped = enemyMoney;
    }

    public void EnemyIsBurned() 
    {
      if(gameObject.name == "5_CorruptKnight(Clone)") 
      {
        isBurned = false;
      }
      else
      {
        isBurned = true;
      }
    }

    public void EnemySpiked()
    {
      canTakeSpikeDamage = false;
      StartCoroutine(ResetSpike());
    }

    IEnumerator ResetSpike()
    {
      yield return new WaitForSeconds(1.5f);
      canTakeSpikeDamage = true;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
      if (other.gameObject.tag == "Castle")
      {
        other.gameObject.GetComponent<CastleHealth>().CastleBreach(enemyHealth);
        EnemySpawner.onEnemyDestroy.Invoke();
        Destroy(gameObject); 
      }

    }
    public void ResetEnemyVitals() // Reset enemy values at start of each game
    {
      enemyHealth = resetHealthValue;
      moneyDropped = resetMoneyValue;
    }





    public void EnableHealthbar(bool active)
    {
      healthbarHolder.SetActive(active);
      if (active == true)
      {
        enemyHealthbar = gameObject.GetComponentInChildren<EnemyHealthbar>();
        enemyHealthbar.DisplayMaxHealth(maxHealth);
        enemyHealthbar.DisplayHealth(enemyHealth); 
      }
    }

    public int DisplayEnemyHealth()
    {
      return enemyHealth;
    }
    public float DisplayEnemySpeed()
    {
      return enemySpeed;
    }
    public int DisplayEnemyMoney()
    {
      return moneyDropped;
    }


}
