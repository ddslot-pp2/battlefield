using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tommy_State : Tank_State {
    void Awake()
    {
        level = 1;
        exp = 0;
        moveSpeed = 10.0f;
        maxHp = 140;
        hp = 140;
        tankType = 1; ;
        tankDefensive = 9;
        bulletAttribute = 1;
        bulletSize = 1;
        damage = 16;
        firstDamage = 16;
        fireRate = 1.4f;
        direct = 1f;
        forward = Vector3.forward;
        attAply = 0;
        range = 18.0f;
        bulletSpeed = 1800.0f;

        soldier = new GameObject[4];
    }
}
