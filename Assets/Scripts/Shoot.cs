using System.Xml.Serialization;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    
    public GameObject core;
    public GameObject gun;
    public TurretProperties turretProperties;
    public AudioSource firingSound;

    GameObject currentTarget;
    FindHome currentTargetCode;
    Quaternion coreStartRotation;
    Quaternion gunStartRotation;
    bool cooldown = true;


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("goob") && currentTarget == null)
        {
            currentTarget = col.gameObject;
            currentTargetCode = currentTarget.GetComponent<FindHome>();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject == currentTarget)
        {
            currentTarget = null;
        }
    }

    void Start()
    {
        coreStartRotation = core.transform.rotation;
        gunStartRotation = gun.transform.localRotation;
    }

   void CoolDown()
    {
        cooldown = true;
    }

    void ShootTarget()
    {
        if (currentTarget && cooldown)
        { 
            currentTargetCode.Hit((int)turretProperties.damage);
            firingSound.Play();
            cooldown = false;
            Invoke("CoolDown", turretProperties.reloadTime);
        }
    }
    
    void Update()
    {
        if (currentTarget != null)
        {
            Vector3 aimAt = new Vector3(currentTarget.transform.position.x,
                core.transform.position.y,
                currentTarget.transform.position.z);

            //gun.transform.LookAt(currentTarget.transform.position);
            //core.transform.LookAt(aimAt);

            float distToTarget = Vector3.Distance(aimAt, gun.transform.position);

            Vector3 relativeTargetPosition = gun.transform.position + gun.transform.forward * distToTarget;
            relativeTargetPosition = new Vector3(relativeTargetPosition.x, currentTarget.transform.position.y, relativeTargetPosition.z);

            gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation,
                Quaternion.LookRotation(relativeTargetPosition - gun.transform.position), Time.deltaTime * turretProperties.turnSpeed);

            core.transform.rotation = Quaternion.Slerp(core.transform.rotation,
                Quaternion.LookRotation(aimAt - core.transform.position), Time.deltaTime * turretProperties.turnSpeed);

            Vector3 directionToTarget = currentTarget.transform.position - gun.transform.position;

            if(Vector3.Angle(directionToTarget, gun.transform.forward) < 10) //10 is the accuracy
                if(Random.Range (0, 100) < turretProperties.accuracy)
                    ShootTarget();
        }
        else
        {
            gun.transform.localRotation = Quaternion.Slerp(gun.transform.localRotation,
               gunStartRotation, Time.deltaTime * turretProperties.turnSpeed);

            core.transform.rotation = Quaternion.Slerp(core.transform.rotation,
                coreStartRotation, Time.deltaTime * turretProperties.turnSpeed);
        }
    }
}
