using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chris_State : Tank_State {
    void Awake()
    {
        level = 1;
        exp = 0;
        moveSpeed = 9.0f;
        maxHp = 160;
        hp = 160;
        tankType = 1; ;
        tankDefensive = 10;
        bulletAttribute = 1;
        bulletSize = 1;
        damage = 16;
        firstDamage = 16;
        fireRate = 1.0f;
        direct = 1f;
        forward = Vector3.forward;
        attAply = 0;
        range = 16.0f;
        bulletSpeed = 1300.0f;

        soldier = new GameObject[4];
    }
}
