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

        healthBar.maxValue = enemyDetails.maxHealth;
        healthBar.value = enemyDetails.maxHealth;

        healthBar.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    public void Hit(int power)
    {
        if (healthBar != null)
        {
            healthBar.value -= power;

            if (healthBar.value <= 0)
            {
                Destroy(healthBar.gameObject);
                Destroy(this.gameObject, 0.1f);
            }
        } 
    }


    void Update()
    {
        if(ai.remainingDistance < 0.5f && ai.hasPath)
        {
            LevelManager.RemoveEnemy();
            ai.ResetPath();
            Destroy(healthBar.gameObject);
            Destroy(this.gameObject, 0.1f);

        }

        if (healthBar)
        {                                                                 //pivot position
            healthBar.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + Vector3.up * 1.2f);
        }


    }
}
