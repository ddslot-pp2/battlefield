using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brian_State : Tank_State {
    void Awake()
    {
        level = 1;
        exp = 0;
        moveSpeed = 10.0f;
        maxHp = 100;
        hp = 100;
        tankType = 1; ;
        tankDefensive = 8;
        bulletAttribute = 1;
        bulletSize = 1;
        damage = 20;
        firstDamage = 20;
        fireRate = 0.6f;
        direct = 1f;
        forward = Vector3.forward;
        attAply = 0;
        range = 16.0f;
        bulletSpeed = 1600.0f;

        soldier = new GameObject[4];
    }
}
