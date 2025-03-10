using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class FindHome : MonoBehaviour
{
    public Transform destination;  
    public EnemyDetails enemyDetails;
    public Slider healthBarPrefab;


    int currentHealth;
    NavMeshAgent ai;
    Slider healthBar;


    void Start()
    {
        ai = GetComponent<NavMeshAgent>();
        ai.SetDestination(destination.position);

        ai.speed = enemyDetails.speed;
        currentHealth = enemyDetails.maxHealth; 

        healthBar = Instantiate(healthBarPrefab, this.transform.position, Quaternion.identity);
        healthBar.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    
    void Update()
    {
        if(ai.remainingDistance < 0.5f && ai.hasPath)
        {
            LevelManager.RemoveEnemy();
            ai.ResetPath();
            Destroy(this.gameObject, 0.1f);

        }

        if (healthBar)
        {                                                                 //pivot position
            healthBar.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + Vector3.up * 1.2f);
        }
    }
}
