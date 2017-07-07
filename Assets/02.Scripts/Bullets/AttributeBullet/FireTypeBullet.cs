using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTypeBullet : MonoBehaviour {
   
    int Damage;
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Tank")
        {
            if (Random.Range(1, 100) >= 50)
                BulletDamageManager.Instance.GetFireEffect(other.gameObject , 10, transform.gameObject);
        }
    }
   
}
