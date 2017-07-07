using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parker_State : Tank_State {
    void Awake()
    {
        level = 1;
        exp = 0;
        moveSpeed = 8.0f;
        maxHp = 160;
        hp = 160;
        tankType = 1; ;
        tankDefensive = 12;
        bulletAttribute = 1;
        bulletSize = 1;
        damage = 18;
        firstDamage = 18;
        fireRate = 1.5f;
        direct = 1.0f;
        forward = Vector3.forward;
        attAply = 0;
        range = 15.0f;
        bulletSpeed = 1400.0f;

        soldier = new GameObject[4];
    }

}
