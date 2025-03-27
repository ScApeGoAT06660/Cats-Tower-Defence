using UnityEngine;
using UnityEngine.Pool;

public class LevelManager : MonoBehaviour
{
    Spawn[] spawnPoints;
    static int totalEnemies = 0;

    public static int numberOfWaves = 3;
    public static int wavesEmitted = 0;
    public static int totalLives = 100;
    public static int totalMoney = 200;
    public static bool levelOver = false;
    public static bool nextWave = false;

    public ParticleSystem deathParticlePrefab;
    public static IObjectPool<ParticleSystem> deathParticlePool;
    public GameObject gameOverPanel;


    int timeBetweenWaves = 5;

    void Start()
    {
        Time.timeScale = 1;
        GameObject[] spawnP = GameObject.FindGameObjectsWithTag("Spawn");
        spawnPoints = new Spawn[spawnP.Length]; 

        for(int i = 0; i < spawnP.Length; i++)
        {
            spawnPoints[i] = spawnP[i].GetComponent<Spawn>();
            totalEnemies += spawnPoints[i].maxCount;
        }

        deathParticlePool = new ObjectPool<ParticleSystem>(CreateDeath, OnTakeFromPool, OnReturnToPool, null, true, 10, 20);
    }

    public static void OnSpeedChange(int speed)
    {
        Time.timeScale = speed;
    }

    ParticleSystem CreateDeath()
    {
        ParticleSystem particleSystem = Instantiate(deathParticlePrefab);
        particleSystem.Stop();
        return particleSystem;
    }

    void OnReturnToPool(ParticleSystem system)
    {
        system.gameObject.SetActive(false);
    }

    void OnTakeFromPool(ParticleSystem system)
    {
        system.gameObject.SetActive(true);
    }

    public static void DisplayDeath(Vector3 position)
    {
        ParticleSystem death = LevelManager.deathParticlePool.Get();

        if (death != null) 
        { 
            death.transform.position = position;
            death.Emit(1);
        }
    }

    public static void RemoveEnemy()
    {
        totalEnemies--;

        if (totalEnemies <= 0)
        {
            wavesEmitted++;
            nextWave = true;

            if(wavesEmitted >= numberOfWaves)
            {
                Debug.Log("Lvl over.");
                levelOver = true;
                nextWave = false;

            }
        }
    }

    public static void RemoveLife()
    {
        totalLives--;
        if (totalLives <= 0)
        {
            Debug.Log("Lvl over.");
            levelOver = true;
            nextWave = false;
        }
    }

    private void ResetSpawners()
    {
         foreach (Spawn item in spawnPoints)
            {
                totalEnemies += item.maxCount;
                item.Restart();
            }
    }

    void Update()
    {
        if (nextWave) 
        {
            nextWave = false;
            Invoke("ResetSpawners", timeBetweenWaves);
        }

        if (levelOver)
        {
            gameOverPanel.SetActive(true);

        }
    }
}
