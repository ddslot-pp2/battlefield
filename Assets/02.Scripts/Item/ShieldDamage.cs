using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDamage : MonoBehaviour {
    public GameObject owner;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(owner.GetComponent<Tank_State>().shield <= 0)
        {
            Destroy(gameObject);
        }
	}
}
