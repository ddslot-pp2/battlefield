using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankKiller_State : Tank_State {
    void Awake()
    {
        level = 1;
        exp = 0;
        moveSpeed = 11.0f;
        maxHp = 200;
        hp = 200;
        tankType = 1;
        tankDefensive = 18;
        bulletAttribute = 1;
        bulletSize = 1.0f;
        damage = 6;
        firstDamage = 6;
        fireRate = 2.1f;
        direct = 1f;
        forward = Vector3.forward;
        attAply = 0;
        range = 20.0f;
        bulletSpeed = 2000.0f;

        soldier = new GameObject[4];
    }
}
