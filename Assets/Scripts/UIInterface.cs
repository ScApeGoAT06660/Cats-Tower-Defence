using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInterface : MonoBehaviour
{
    public Shoot rocketTurret;
    public Shoot gattlingTurret;
    public Shoot flammerTurret;

    public TMPro.TMP_Text waveText;
    public TMPro.TMP_Text moneyText;
    public TMPro.TMP_Text livesText;
    public TMPro.TMP_Text upgradeButton;

    public GameObject turretMenu;
    public AudioSource wrongSound;

    public Button slowSpeed;
    public Button mediumSpeed;
    public Button fastSpeed;
    

    GameObject itemPrefab;
    GameObject focusObj;

    Shoot currentClickedTurret;


    public void CreateRocket()
    {
        if(LevelManager.totalMoney >= rocketTurret.turretProperties.cost)
        {
            itemPrefab = rocketTurret.gameObject;
            CreateItemForButton();
            LevelManager.totalMoney -= (int)rocketTurret.turretProperties.cost;
        }
        else
        {
            wrongSound.Play();
        }
    }

    public void CreateFlammer()
    {
        if (LevelManager.totalMoney >= flammerTurret.turretProperties.cost)
        {
            itemPrefab = flammerTurret.gameObject;
            CreateItemForButton();
            LevelManager.totalMoney -= (int)flammerTurret.turretProperties.cost;
        }
        else
        {
            wrongSound.Play();
        }
    }

    public void CreateGasttling()
    {
        if (LevelManager.totalMoney >= gattlingTurret.turretProperties.cost)
        {
            itemPrefab = gattlingTurret.gameObject;
            CreateItemForButton();
            LevelManager.totalMoney -= (int)gattlingTurret.turretProperties.cost;
        }
        else
        {
            wrongSound.Play();
        }
    }

    private void Start()
    {
        slowSpeed.onClick.AddListener(SlowSpeedClicked);
        mediumSpeed.onClick.AddListener(MediumSpeedClicked);
        fastSpeed.onClick.AddListener(FastSpeedClicked);
    }

    void SlowSpeedClicked()
    {
        LevelManager.OnSpeedChange(1);
    }
    void MediumSpeedClicked()
    {
        LevelManager.OnSpeedChange(5);
    }
    void FastSpeedClicked()
    {
        LevelManager.OnSpeedChange(10);
    }

    void CreateItemForButton()
    {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit))
                return;  

            focusObj = Instantiate(itemPrefab, hit.point, itemPrefab.transform.rotation);
            focusObj.GetComponent<Collider>().enabled = false;
    }

    public void UpgradeTurretMenu()
    {
        if (LevelManager.totalMoney >= currentClickedTurret.turretProperties.cost)
        {
            currentClickedTurret.turretProperties.damage *= 1.2f;
            LevelManager.totalMoney -= (int)currentClickedTurret.turretProperties.cost;
            currentClickedTurret.turretProperties.cost *= 2;
            upgradeButton.text = "Lvl up: $" + currentClickedTurret.turretProperties.cost;
        }
        else
        {
            wrongSound.Play();
        }
    }

    public void DeleteTurretMenu()
    {
        Destroy(currentClickedTurret.gameObject, 0.1f);
        LevelManager.totalMoney -= (int)(currentClickedTurret.turretProperties.cost * 0.5f);
        turretMenu.SetActive(false);
    }

    public void CloseTurretMenu()
    {
        turretMenu.SetActive(false);
    }

    public void PlayAgain()
    {
        LevelManager.numberOfWaves = 3;
        LevelManager.wavesEmitted = 0;
        LevelManager.totalLives = 100;
        LevelManager.totalMoney = 200;
        LevelManager.levelOver = false;
        LevelManager.nextWave = false;
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    void Update()
    {
        if(LevelManager.wavesEmitted < LevelManager.numberOfWaves)
            waveText.text = (LevelManager.wavesEmitted + 1) + " of " + LevelManager.numberOfWaves;

        moneyText.text = "$" + LevelManager.totalMoney.ToString();

        if(LevelManager.totalLives >= 0)
            livesText.text = "HP: " + LevelManager.totalLives.ToString();


        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray,out hit))
            {
                if (hit.collider.tag == "Turret")
                {
                    turretMenu.transform.position = Input.mousePosition;
                    turretMenu.SetActive(true);
                    currentClickedTurret = hit.collider.gameObject.GetComponent<Shoot>();
                    upgradeButton.text = "Lvl up: $" + currentClickedTurret.turretProperties.cost;
                }
            }
        }
        else if (focusObj && Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit))
                return;

            focusObj.transform.position = hit.point;
            
        }
        else if (focusObj && Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && 
                hit.collider.gameObject.CompareTag("Platform"))
            {
                float platformRotation = hit.collider.gameObject.GetComponent<PlatformDetails>().rotation;
                Vector3 newPosition = hit.collider.gameObject.transform.position;
                newPosition.y += 1f; 
                focusObj.transform.position = newPosition;
                focusObj.transform.Rotate(0, platformRotation, 0);

                hit.collider.gameObject.tag = "Occupied";

                focusObj.GetComponent<Collider>().enabled = true; 
                focusObj.GetComponent<SphereCollider>().enabled = true;
            }
            else
            {
                LevelManager.totalMoney += (int)focusObj.GetComponent<Shoot>().turretProperties.cost;

                Destroy(focusObj, 0.2f);
            }
                
            focusObj = null;
        }
    }
}
