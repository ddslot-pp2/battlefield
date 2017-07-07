using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    public GameObject shield;

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Tank")
        {
            other.GetComponent<Tank_State>().shield = 5;
            GameObject a = Instantiate(shield, new Vector3(0, 0, 0), transform.rotation);
            a.transform.parent = other.transform;
            a.transform.localPosition = new Vector3(0, 0, 0);
            a.GetComponent<ShieldDamage>().owner = other.gameObject;
            Destroy(gameObject);
        }
    }
}
