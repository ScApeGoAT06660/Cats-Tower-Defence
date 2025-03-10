using UnityEngine;
using UnityEngine.EventSystems;

public class UIInterface : MonoBehaviour
{
    public GameObject tower;
    GameObject focusObj;
    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit))
                return;  

            focusObj = Instantiate(tower, hit.point, tower.transform.rotation);
            focusObj.GetComponent<Collider>().enabled = false;


        }
        else if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit))
                return;

            focusObj.transform.position = hit.point + new Vector3(0, 0.5f, 0);

        }
        else if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && 
                hit.collider.gameObject.CompareTag("Platform"))
            {
                hit.collider.gameObject.tag = "Occupied";
            }
            else
            {
                Destroy(focusObj);
            }
                

        }

    }
}
