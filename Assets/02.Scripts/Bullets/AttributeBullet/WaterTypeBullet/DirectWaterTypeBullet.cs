using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectWaterTypeBullet : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Tank")
        {
            if (Random.Range(1, 100) >= 50)
                BulletDamageManager.Instance.GetWaterEffect(other.gameObject, GetComponent<DirectBullet>().first);
        }
    }
    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update () {
		
	}
}
