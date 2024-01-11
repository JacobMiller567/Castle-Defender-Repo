using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public int CurrentMap = 1;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] public bool [] CheckIfNew;
    [SerializeField] public List<string> CheckNewEnemies; 
    [SerializeField] private Transform startPoint;

    [SerializeField] private int enemyAmount = 8;
    [SerializeField] private float spawnSpeed;
    [SerializeField] private float spawnCoolDown;
    [SerializeField] private float difficultyScaler;
    [SerializeField] private int enemyType = 0;
    [SerializeField] private int currentWave = 1;
    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] private GameObject bossText;
    [SerializeField] private GameObject winText;

    public static UnityEvent onEnemyDestroy = new UnityEvent();
    [SerializeField] private PlayerStats menu;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] Toggle healthbarToggle;
    [SerializeField] private SaveData data;
    [SerializeField] private GameObject PopupCorruptKnight;


    public bool infiniteMode;
    private bool infiniteMapTwo;
    private bool infiniteMapThree;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool canSpawn;
    private bool waveActive = false;
    private bool active = false;



    private void Awake()
    {
        onEnemyDestroy.AddListener(OnDestroy); // add listener to call for enemy deaths
        
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyVitals>().ResetEnemyVitals(); // Reset enemy health and money for new game
        }
    }

    void Start()
    {
        Time.timeScale = 1;
        data = FindObjectOfType<SaveData>();
        healthbarToggle.isOn = data.GetComponent<SaveData>().healthBarShown;
        ShowHealthbar();

        if (CurrentMap == 2 || CurrentMap == 3)
        {
            bool difficultyNormal = data.GetComponent<SaveData>().isDifficultyNormal;
            bool storyMode = data.GetComponent<SaveData>().isStoryMode;

            if (storyMode == true)
            {
                if (difficultyNormal == true)
                {
                    PlayStory();
                    NormalMode();
                }
                else
                {
                    PlayStory();
                    HardMode();
                }
            }
            if (storyMode == false)
            {
                if (difficultyNormal == true)
                {
                    PlayInfinite();
                    NormalMode();
                }
                else
                {
                    PlayInfinite();
                    HardMode();
                }
            }
        }
    }

    public void NormalMode() 
    {
        enemyAmount = 8;
        menu.NormalMode = true;
        data.isDifficultyNormal = true;
        if (infiniteMapTwo == true)
        {
            mainMenu.LoadLevelTwo();
        }
        if (infiniteMapThree == true) // TEST
        {
            mainMenu.LoadLevelThree();
        }
        StartCoroutine(StartGame());
    }
    public void HardMode() 
    {
        enemyAmount = 10;
        menu.HardMode = true;
        data.isDifficultyNormal = false;
        if (infiniteMapTwo == true)
        {
            mainMenu.LoadLevelTwo();
        }
        if (infiniteMapThree == true)
        {
            mainMenu.LoadLevelThree();
        }
        StartCoroutine(StartGame());
    }

    public void PlayStory()
    {
        waveText.text = "Wave: " + currentWave.ToString();
        data.isStoryMode = true;
    }

    public void PlayInfinite()
    {
        waveText.text = "Wave: " + currentWave.ToString();
        infiniteMode = true;
        data.isStoryMode = false;
    }

    public void SecondMapInfinite()
    {
        infiniteMapTwo = true;
    }
    public void ThirdMapInfinite()
    {
        infiniteMapThree = true;
    }

    private void Update()
    {
        if (!waveActive) return;
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= spawnSpeed && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }
        

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void OnDestroy()
    {
        enemiesAlive--;
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(spawnCoolDown);
        StartCoroutine(StartWave());
    }
    
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(spawnCoolDown);

        
        if (currentWave >= 10 && currentWave <= 20) // If current wave is 10 - 20
        {
            spawnSpeed = 2; // Decrease time between spawns
        }
        if (currentWave > 20 ) // If current wave greater than 20
        {
            spawnSpeed = 1; // Decrease time between spawns even more
        }

        waveActive = true;
        waveText.text = "Wave: " + currentWave.ToString();
        enemiesLeftToSpawn = IncreaseEnemies();
    }


    private void EndWave()
    {
        waveActive = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }

    private int IncreaseEnemies()
    {
        UpgradeEnemyHealth();
        return Mathf.RoundToInt(enemyAmount * Mathf.Pow(currentWave, difficultyScaler));
    }

    private void UpgradeEnemyHealth()
    {
        int checkCount = CheckNewEnemies.Count;

        for (int i = 0; i < checkCount; i++)
        {
            enemyType = i;
            
            if (CurrentMap == 1) // MAP 1
            {
                GameObject enemyPrefab = enemies[enemyType];

            /// NORMAL MODE ///
                if (enemyType == 0 && infiniteMode == false) 
                {
                    int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();

                    if (menu.NormalMode == true)
                    {
                        enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 1) * 1.9f)));
                        Debug.Log("Increased Scorpion Health");
                    }

                    if (menu.HardMode == true)
                    {
                        enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 1) * 2.75f))); 
                    }
                }

                if (enemyType == 1 && infiniteMode == false) 
                {

                    int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth(); // Default health + given value
                    if (currentWave >= 6)
                    {
                        if (menu.NormalMode == true) 
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 3) * 1.75f))); 
                        }

                        if (menu.HardMode == true)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 3) * 2.45f))); 
                            enemyPrefab.GetComponent<EnemyVitals>().DecreaseMoney(2); // Decrease enemy money from 3 to 2
                        }
                    }
                }

                if (enemyType == 2 && infiniteMode == false) 
                { 

                    if (currentWave >= 11 && currentWave < 21)
                    {
                        if (menu.NormalMode == true)
                        {
                            int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();           
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 8) * 1.9f)));                           
                        }
                        if (menu.HardMode == true)
                        {
                            int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 8) * 2.9f)));
                            enemyPrefab.GetComponent<EnemyVitals>().DecreaseMoney(3); 
                        }
                    }
                    if (currentWave >= 21) 
                    {
                        if (menu.NormalMode == true)
                        {
                            int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();                       
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 13) * 3f)));
                        }
                        if (menu.HardMode == true)
                        {
                            int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 13) * 4.25f)));
                            enemyPrefab.GetComponent<EnemyVitals>().DecreaseMoney(3); 
                        }
                    }
                }

                if (enemyType == 3 && infiniteMode == false) 
                {

                    if (currentWave >= 20)
                    {
                        if (menu.NormalMode == true)
                        {
                            int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();               
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 17) * 1.8f)));                        
                        }
                        if (menu.HardMode == true)
                        {
                            int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();                        
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 17) * 2.5f)));
                            enemyPrefab.GetComponent<EnemyVitals>().DecreaseMoney(4);

                        }
                    }
                }
            /// END OF NORMAL ///


            /// INFINITE MODE ///
                if (enemyType == 0 && infiniteMode == true && currentWave > 1) 
                {
                    int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();
                    if (menu.NormalMode == true)
                    {
                        if (currentWave < 15)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 3f) ));
                        }
                        if (currentWave >= 15 && currentWave < 30)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 4f) )); 
                        }
                        if (currentWave >= 30)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 4.5f) )); 
                        }
                    }
                    if (menu.HardMode == true)
                    {
                        if (currentWave < 15)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 4f) )); 
                        }
                        if (currentWave >= 15 && currentWave < 30)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 5.25f) )); 
                        }
                        if (currentWave >= 30)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 5.85f) )); 
                        }
                    }
                    if (currentWave == 5 || currentWave == 15 || currentWave == 25 || currentWave == 35) 
                    {
                        // Enemies start at $2 and can max out at $6 per kill
                        enemyPrefab.GetComponent<EnemyVitals>().IncreaseMoney(1); 
                    }
                }
            /// END OF INFINITE ///       
            }

            if (CurrentMap == 2) // MAP 2
            {
                GameObject enemyPrefab = enemies[enemyType];

            /// NORMAL MODE ///
                if (enemyType == 0 && infiniteMode == false) 
                {
                    int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();

                    if (menu.NormalMode == true) 
                    {
                        enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 1) * 1.7f)));                      
                    }

                    if (menu.HardMode == true)
                    {
                        enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 1) * 2.5f))); 
                    }
                }

                if (enemyType == 1 && infiniteMode == false) 
                {

                    int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth(); 
                    if (currentWave >= 6)
                    {
                        if (menu.NormalMode == true) 
                        {                          
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 4) * 1.65f))); 
                        }

                        if (menu.HardMode == true) 
                        {                           
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 4) * 2.4f)));  
                        }
                    }
                }

                if (enemyType == 2 && infiniteMode == false) 
                { 
                    if (currentWave >= 12 && currentWave < 21)
                    {
                        if (menu.NormalMode == true)
                        {
                            int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();                     
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 8) * 1.85f)));
                        }
                        if (menu.HardMode == true)
                        {
                            int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 8) * 2.8f)));
                        }
                    }
                    if (currentWave >= 21) 
                    {
                        if (menu.NormalMode == true)
                        {
                            int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 12) * 2.5f)));
                        }
                        if (menu.HardMode == true)
                        {
                            int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 12) * 3.6f)));
                        }
                    }
                }

                if (enemyType == 3 && infiniteMode == false) 
                {
                    if (currentWave >= 16)
                    {
                        if (menu.NormalMode == true)
                        {
                            int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();                       
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 15) * 2.5f)));
                        }
                        if (menu.HardMode == true)
                        {
                            int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 15) * 3.5f)));
                        }
                    }
                }

                if (enemyType == 4 && infiniteMode == false) 
                {
                    if (currentWave > 25)
                    {
                        if (menu.NormalMode == true)
                        {
                            int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 21) * 3.75f))); 
                        }
                        if (menu.HardMode == true)
                        {
                            int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt((currentWave - 21) * 4.8f))); 
                            enemyPrefab.GetComponent<EnemyVitals>().DecreaseMoney(5);
                        }
                    }
                }
            /// END OF NORMAL ///


            /// INFINITE MODE ///
                if (enemyType == 0 && infiniteMode == true && currentWave > 1) 
                {
                    int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();
                    if (menu.NormalMode == true)
                    {
                        if (currentWave < 15)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 3f) )); 
                        }
                        if (currentWave >= 15 && currentWave < 30)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 4f) )); 
                        }
                        if (currentWave >= 30)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 4.5f) )); 
                        }
                    }
                    if (menu.HardMode == true)
                    {
                        if (currentWave < 15)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 4f) )); 
                        }
                        if (currentWave >= 15 && currentWave < 30)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 5f) )); 
                        }
                        if (currentWave >= 30)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 6f) )); 
                        }
                    }
            
                    if (currentWave == 5 || currentWave == 15 || currentWave == 25 || currentWave == 35)
                    {
                        // Enemies start at $2 and can max out at $6 per kill
                        enemyPrefab.GetComponent<EnemyVitals>().IncreaseMoney(1); // increase enemy money dropped
                    }
                }
            /// END OF INFINITE ///
            }

            if (CurrentMap == 3) // TEST: Currently only used for Infinite Mode
            {
                GameObject enemyPrefab = enemies[enemyType];

                /// INFINITE MODE ///
                if (enemyType == 0 && infiniteMode == true && currentWave > 1) 
                {
                    int getHealth = enemyPrefab.GetComponent<EnemyVitals>().GetEnemyHealth();
                    if (menu.NormalMode == true)
                    {
                        if (currentWave < 15)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 3f) )); 
                        }
                        if (currentWave >= 15 && currentWave < 30)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 4f) )); 
                        }
                        if (currentWave >= 30)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 4.5f) )); 
                        }
                    }
                    if (menu.HardMode == true)
                    {
                        if (currentWave < 15)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 4f) )); 
                        }
                        if (currentWave >= 15 && currentWave < 30)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 5f) )); 
                        }
                        if (currentWave >= 30)
                        {
                            enemyPrefab.GetComponent<EnemyVitals>().IncreaseHealth(getHealth + (Mathf.RoundToInt(currentWave * 6f) )); 
                        }
                    }
            
                    if (currentWave == 5 || currentWave == 15 || currentWave == 25 || currentWave == 35)
                    {
                        // Enemies start at $2 and can max out at $6 per kill
                        enemyPrefab.GetComponent<EnemyVitals>().IncreaseMoney(1); // increase enemy money dropped
                    }
                }
            /// END OF INFINITE ///
            }
        }
    }


    private void SpawnEnemy()
    {
        NewEnemyType();
        GameObject enemyPrefab = enemies[enemyType];
        Instantiate(enemyPrefab, startPoint.position, Quaternion.identity); 

        if (active == true) 
        {
            ShowHealthbar();
        }
    }

    private void NewEnemyType()
    {
        if (CurrentMap == 1)
        {
            LevelOneEnemies();
        }
        if (CurrentMap == 2)
        {
            LevelTwoEnemies();
        }
        if (CurrentMap == 3) 
        {
            LevelThreeEnemies();
        }
    }

    private void LevelOneEnemies()
    {
        if (infiniteMode == false)
        {
            if (currentWave < 5) 
            {
                enemyType = 0;
                if (CheckIfNew[0] == false)
                {
                    CheckIfNew[0] = true; // Add enemy to registry
                    CheckNewEnemies.Add("Scorpion");
                }
            }
            if (currentWave > 4 && currentWave < 10) 
            {
                float chooseEnemy = Random.Range(0, 2); 
                if (chooseEnemy == 1)
                {
                    enemyType = 1;
                    if (CheckIfNew[1] == false)
                    {
                        CheckIfNew[1] = true; 
                        CheckNewEnemies.Add("Goblin");
                    }
                }
                else
                {
                    enemyType = 0;
                }
            }
            if (currentWave > 9 && currentWave < 16) 
            {
                float chooseEnemy = Random.Range(0, 3);
                if (chooseEnemy == 1)
                {
                    enemyType = 1;
                }
                else if (chooseEnemy == 2)
                {
                    enemyType = 2;
                    if (CheckIfNew[2] == false)
                    {
                        CheckIfNew[2] = true; 
                        CheckNewEnemies.Add("Wizard");
                    }
                }
                else
                {
                    enemyType = 0;
                }
            }
            if (currentWave > 15 && currentWave < 21) 
            {
                float chooseEnemy = Random.Range(0, 3);
                if (chooseEnemy == 1) 
                {
                    enemyType = 3;
                    if (CheckIfNew[3] == false)
                    {
                        CheckIfNew[3] = true; 
                        CheckNewEnemies.Add("Warrior Goblin");
                    }
                }
                else 
                {
                    enemyType = 2;
                }
            }
            if (currentWave > 20 && currentWave < 25) 
            {
                float chooseEnemy = Random.Range(0, 2);
                if (chooseEnemy == 1)
                {
                    enemyType = 3;
                    if (CheckIfNew[3] == false)
                    {
                        CheckIfNew[3] = true; 
                        CheckNewEnemies.Add("Warrior Goblin");
                    }
                }
                else
                {
                    enemyType = 2;
                }
            }

            if (currentWave == 25) 
            {
                enemyAmount = 1;
                enemiesLeftToSpawn = 1;
                enemyType = 4; 
                bossText.SetActive(true);
                StartCoroutine(HideBossText());
            }

            if (currentWave == 26)
            {
                data.GameLevel = 2;
                mainMenu.LoadLevelTwo();
            }
        }
        
        if (infiniteMode == true)
        {
            enemyType = 0;
            if (CheckIfNew[0] == false)
            {
                CheckIfNew[0] = true; 
                CheckNewEnemies.Add("Scorpion");
            }
        }
    }

    private void LevelTwoEnemies()
    {
        if (infiniteMode == false)
        {
            if (currentWave < 5) 
            {
                enemyType = 0;
                if (CheckIfNew[0] == false)
                {
                    CheckIfNew[0] = true; 
                    CheckNewEnemies.Add("Scorpion");
                }
            }
            if (currentWave > 4 && currentWave < 10) 
            {
                float chooseEnemy = Random.Range(0, 2); 
                if (chooseEnemy == 1)
                {
                    enemyType = 1;
                    if (CheckIfNew[1] == false)
                    {
                        CheckIfNew[1] = true; 
                        CheckNewEnemies.Add("Spear Goblin");
                    }
                }
                else
                {
                    enemyType = 0;
                }
            }
            if (currentWave > 9 && currentWave < 16) 
            {
                float chooseEnemy = Random.Range(0, 3);
                if (chooseEnemy == 1)
                {
                    enemyType = 1;
                }
                else if (chooseEnemy == 2)
                {
                    enemyType = 2;
                    if (CheckIfNew[2] == false)
                    {
                        CheckIfNew[2] = true; 
                        CheckNewEnemies.Add("Wizard");
                    }
                }
                else
                {
                    enemyType = 0;
                }
            }
            if (currentWave > 15 && currentWave < 21)
            {
                float chooseEnemy = Random.Range(0, 3);
                if (chooseEnemy == 1)
                {
                    enemyType = 3;
                    if (CheckIfNew[3] == false)
                    {
                        CheckIfNew[3] = true;
                        CheckNewEnemies.Add("Armored Troll");
                    }
                }
                else
                {
                    enemyType = 2;
                }
            }
            if (currentWave > 20 && currentWave < 25)
            {
                float chooseEnemy = Random.Range(0, 2);
                if (chooseEnemy == 1)
                {
                    enemyType = 3;
                    if (CheckIfNew[3] == false)
                    {
                        CheckIfNew[3] = true;
                        CheckNewEnemies.Add("Armored Troll");
                    }
                }
                else
                {
                    enemyType = 2;
                }
            }
            if (currentWave > 24 && currentWave < 30)
            {
                float chooseEnemy = Random.Range(0, 3);
                if (chooseEnemy == 1) 
                {
                    enemyType = 4;
                    if (CheckIfNew[4] == false)
                    {
                        CheckIfNew[4] = true; 
                        CheckNewEnemies.Add("Corrupt Knight");
                        PopupCorruptKnight.SetActive(true);
                        Destroy(PopupCorruptKnight, 4f);                   
                    }
                }
                else
                {
                    enemyType = 3;
                }
            }

            if (currentWave == 30)
            {
                enemyAmount = 1;
                enemiesLeftToSpawn = 1;
                enemyType = 5; 
                bossText.SetActive(true); 
                StartCoroutine(HideBossText());
            }

            if (currentWave == 31)
            {
                Time.timeScale = 0;
                mainMenu.Win();
            }
        }
        
        if (infiniteMode == true)
        {
            enemyType = 0;
            if (CheckIfNew[0] == false)
            {
                CheckIfNew[0] = true;
                CheckNewEnemies.Add("Scorpion");
            }
        }
    }

    private void LevelThreeEnemies() // TEST
    {
        if (infiniteMode == true)
        {
            enemyType = 0;
            if (CheckIfNew[0] == false)
            {
                CheckIfNew[0] = true;
                CheckNewEnemies.Add("Scorpion");
            }
        }
    }

    public void SpawnAbility(Transform trans, int index, bool level)
    {
        if (trans == null) return;
        GameObject enemyPrefab = enemies[3];
        GameObject minion = Instantiate(enemyPrefab, trans.position, Quaternion.identity) as GameObject; // Spawn enemy at boss current position       
        minion.transform.rotation = trans.rotation; // Set minions rotation the same as the boss
        
        enemiesAlive++;
        if (level == true)
        {
            minion.GetComponent<EnemyMovement>().LevelTwo = true;
        }
        minion.GetComponent<EnemyMovement>().pathIndex = index; // Sets minions path to where the boss is at   
    }

    private IEnumerator HideBossText()
    {
        yield return new WaitForSeconds(5f);
        bossText.SetActive(false);
    }



    public void ShowHealthbar() 
    {
        if (healthbarToggle.isOn == true) 
        {
            active = true;
            EnemyVitals[] enemyVitals = FindObjectsOfType<EnemyVitals>(); 
            foreach (EnemyVitals vitals in enemyVitals)
            {
                if (vitals.isBoss == false)
                {           
                    vitals.EnableHealthbar(true);
                    vitals.showEnemyHealth = true;
                    data.healthBarShown = true;
                }
            }
        }
        else
        {
            active = false;
            EnemyVitals[] enemyVitals = FindObjectsOfType<EnemyVitals>(); 
            foreach (EnemyVitals vitals in enemyVitals)
            {
                if (vitals.isBoss == false)
                {                   
                    vitals.EnableHealthbar(false);
                    vitals.showEnemyHealth = false;
                    data.healthBarShown = false;
                }
            }
        }
    }

}
