using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Michael_State : Tank_State {
    void Awake()
    {
        level = 1;
        exp = 0;
        moveSpeed = 8.0f;
        maxHp = 180;
        hp = 180;
        tankType = 2;
        tankDefensive = 16;
        bulletAttribute = 1;
        bulletSize = 1.0f;
        damage = 26;
        firstDamage = 26;
        fireRate = 1.5f;
        direct = 1f;
        forward = Vector3.forward;
        attAply = 0;
        range = 16.0f;
        bulletSpeed = 1700.0f;

        soldier = new GameObject[4];
    }
}
