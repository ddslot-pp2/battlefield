using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour {
    public float delayFire;
    public LineRenderer lineDraw;
    public RaycastHit hitPoint;
    public GameObject attacker;
    public GameObject firstPos;
    public int damage;
    public bool attack = false;
   
	// Use this for initialization
	void Start () {
        lineDraw = GetComponent<LineRenderer>();
    }


    public void GetDamageType(int damage, GameObject attacker, GameObject firstPos)
    {
        this.damage = damage;
        this.attacker = attacker;
        this.firstPos = firstPos;

        attack = true;
        delayFire = Time.time + 3.0f;
        InvokeRepeating("DamegeCheck", 0, 0.5f);
    }
}
