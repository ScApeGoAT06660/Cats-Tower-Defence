using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class UIInterface : MonoBehaviour
{
    public GameObject rocketTurret;
    public GameObject gattlingTurret;
    public GameObject flammerTurret;

    public GameObject turretMenu;

    GameObject itemPrefab;
    GameObject focusObj;

    public void CreateRocket()
    {
        itemPrefab = rocketTurret;
        CreateItemForButton();
    }

    public void CreateFlammer()
    {
        itemPrefab = flammerTurret;
        CreateItemForButton();
    }

    public void CreateGasttling()
    {
        itemPrefab = gattlingTurret;
        CreateItemForButton();
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

    public void CloseTurretMenu()
    {
        turretMenu.SetActive(false);
    }

    void Update()
    {
        
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
                Vector3 newPosition = hit.collider.gameObject.transform.position;
                newPosition.y += 1f; 
                focusObj.transform.position = newPosition;

                hit.collider.gameObject.tag = "Occupied";

                focusObj.GetComponent<Collider>().enabled = true;
            }
            else
            {
                Destroy(focusObj);
            }
                
            focusObj = null;
        }
    }
}
