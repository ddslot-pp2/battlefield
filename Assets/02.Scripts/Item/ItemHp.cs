using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHp : MonoBehaviour {
    public GameObject[] bullet3;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Tank")
        {
            Tank_State Ts = other.GetComponent<Tank_State>();
            if (Ts.hp + 50 > Ts.maxHp)
            {
                Ts.hp = Ts.maxHp;
            }
            else
            {
                Ts.hp += 50;
            }
            
            Destroy(gameObject);
        }
    }
}
