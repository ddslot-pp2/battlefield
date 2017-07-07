using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {
    public GameObject destroyParticle;
   
  
    void OnTriggerEnter(Collider other)
    {
       if(other.tag == "Tank")
        {
            GameObject destroy = Instantiate(destroyParticle, transform.position, transform.rotation);
            Destroy(destroy, 1.0f);

			BulletDamageManager.Instance.GetDamage(10, other.gameObject);

            Destroy(gameObject);
        }
    }
}
