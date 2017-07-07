using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cathy_State : Tank_State {
    void Awake()
    {
        level = 1;
        exp = 0;
        moveSpeed = 10.0f;
        maxHp = 160;
        hp = 160;
        tankType = 1; ;
        tankDefensive = 15;
        bulletAttribute = 1;
        bulletSize = 1;
        damage = 20;
        firstDamage = 20;
        fireRate = 0.9f;
        direct = 1f;
        forward = Vector3.forward;
        attAply = 0;
        range = 22.0f;
        bulletSpeed = 1900.0f;
        soldier = new GameObject[4];
    }
}
