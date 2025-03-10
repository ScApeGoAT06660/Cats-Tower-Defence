using UnityEngine;

public class LevelManager : MonoBehaviour
{
    GameObject[] spawnPoints;
    static int totalEnemies = 0;

    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");

        foreach (var item in spawnPoints)
        {
            totalEnemies += item.GetComponent<Spawn>().maxCount;
        }
        //Debug.Log(totalEnemies);
    }

    public static void RemoveEnemy()
    {
        totalEnemies--;

        if (totalEnemies <= 0)
            Debug.Log("Lvl over.");
    }

    void Update()
    {
        
    }
}
