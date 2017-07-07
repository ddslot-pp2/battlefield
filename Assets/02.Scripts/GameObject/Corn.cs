using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corn : DestroyObject {
    // Use this for initialization
    void Start()
    {
        hit = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit <= 0)
        {
			ObjectManager.Instance.CreateObject(transform.position, transform.rotation, 5);

            Destroy(gameObject);
        }
    }
}
