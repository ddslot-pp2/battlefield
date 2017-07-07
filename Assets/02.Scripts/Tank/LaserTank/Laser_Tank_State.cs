using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Tank_State : Tank_State {
    void Awake()
    {
        level = 1;
        exp = 0;
        moveSpeed = 12.0f;
        maxHp = 300;
        hp = 300;
        tankType = 6;
        tankDefensive = 19;
        bulletAttribute = 1;
        bulletSize = 1;
        damage = 10;
        firstDamage = 20;
        fireRate = 2.6f;
        direct = 1f;
        forward = Vector3.forward;
        attAply = 0;
        bulletSpeed = 1900.0f;
        soldier = new GameObject[4];

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
