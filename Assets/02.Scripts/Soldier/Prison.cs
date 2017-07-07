using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prison : DestroyObject {
    public GameObject[] Soldier;
   
    // Use this for initialization
    void Start () {
        hit = 3;
    
    }

    // Update is called once per frame
    void Update () {
		if(hit <= 0)
        {
            Instantiate(Soldier[Random.Range(0, 6)], transform.position, transform.rotation);
			ObjectManager.Instance.CreateObject(transform.position, transform.rotation, 4);

            Destroy(gameObject);
        }
	}
}
