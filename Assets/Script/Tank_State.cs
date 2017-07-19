﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_State : MonoBehaviour
{
    public int level;
    public int exp;
    public float moveSpeed;
    public int maxHp;
    public int hp;
    public int tankType;
    public int tankDefensive;
    public float bulletAttribute;
    public float bulletSize;
    public int damage;
    public GameObject bullet;
    public int firstDamage;
    public float fireRate;
    public Vector3 forward;
    public float direct;
    public int attAply;
    public float range;
    public int shield;
    public float bulletSpeed;

    public GameObject[] soldier;

	void Awake()
	{
		forward = Vector3.forward;
	}

	public void GetDamage(int damage )
	{
		hp = hp - damage;
		if (hp <= 0)
			hp = 0;
		
	}
}
