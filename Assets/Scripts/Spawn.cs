using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform homeLocation;
    public float spawnRate = 0.3f;
    public int maxCount = 10;
    public float startDelay = 1.0f;

    int count = 0;

    void Start()
    {
         Restart();
    }

    public void Restart()
    {
        count = 0;
        InvokeRepeating("Spawner", startDelay, spawnRate);
    }

    public void Spawner()
    {
        GameObject goob = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        goob.GetComponent<FindHome>().destination = homeLocation;
        count++;

        if (count >= maxCount)
            CancelInvoke("Spawner");
    }
}
