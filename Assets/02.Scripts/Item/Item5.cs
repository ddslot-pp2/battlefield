using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item5 : MonoBehaviour {

    public GameObject[] bullet5;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Tank")
        {
            Tank_State Ts = other.GetComponent<Tank_State>();
            Debug.Log(other.transform.name);
            if (Ts.bulletAttribute == 5)
            {
                if (Ts.bulletSize >= 1.8)
                {

                }

                else
                {
                    Ts.bulletSize += 0.2f;
                    Ts.damage += 1;
                    Debug.Log(Ts.damage);
                    Destroy(gameObject);
                }
            }
            else
            {
                Ts.bulletAttribute = 5;
                Ts.bullet = bullet5[Ts.tankType - 1];
                Ts.bulletSize = 1.0f;
                Ts.damage = Ts.firstDamage;
            }
            Destroy(gameObject);
        }
    }
}
