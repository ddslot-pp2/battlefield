using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimTypeBullet : MonoBehaviour {
    
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Tank")
        {
            if (Random.Range(1, 100) >= 50)
                BulletDamageManager.Instance.GetSlimEffect(other.gameObject);
        }
    }
   
}
