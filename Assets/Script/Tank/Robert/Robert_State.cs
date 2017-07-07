using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robert_State : Tank_State {
    void Awake()
    {
        level = 1;
        exp = 0;
        moveSpeed = 12.0f;
        maxHp = 140;
        hp = 140;
        tankType = 1; ;
        tankDefensive = 12;
        bulletAttribute = 1;
        bulletSize = 1;
        damage = 18;
        firstDamage = 18;
        fireRate = 1.1f;
        direct = 1f;
        forward = Vector3.forward;
        attAply = 0;
        range = 17.0f;
        bulletSpeed = 1400.0f;

        soldier = new GameObject[4];
    }
}
