using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jason_State : Tank_State {
    void Awake()
    {
        level = 1;
        exp = 0;
        moveSpeed = 13.0f;
        maxHp = 240;
        hp = 240;
        tankType = 1; ;
        tankDefensive = 16;
        bulletAttribute = 1;
        bulletSize = 1;
        damage = 7;
        firstDamage = 7;
        fireRate = 2.8f;
        direct = 1f;
        forward = Vector3.forward;
        attAply = 0;
        range = 16.0f;
        bulletSpeed = 1800.0f;
        soldier = new GameObject[4];
    }
}
