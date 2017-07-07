using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ted_State : Tank_State {
    void Awake()
    {
        level = 1;
        exp = 0;
        moveSpeed = 8.0f;
        maxHp = 120;
        hp = 120;
        tankType = 2;
        tankDefensive = 13;
        bulletAttribute = 1;
        bulletSize = 1.0f;
        damage = 22;
        firstDamage = 22;
        fireRate = 1.2f;
        direct = 1f;
        forward = Vector3.forward;
        attAply = 0;
        range = 19.0f;
        bulletSpeed = 1600.0f;

        soldier = new GameObject[4];
    }
}
