using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billy_State : Tank_State {
    void Awake()
    {
        level = 1;
        exp = 0;
        moveSpeed = 9.0f;
        maxHp = 120;
        hp = 120;
        tankType = 1; ;
        tankDefensive = 10;
        bulletAttribute = 1;
        bulletSize = 1;
        damage = 15;
        firstDamage = 15;
        fireRate = 0.6f;
        direct = 1f;
        forward = Vector3.forward;
        attAply = 0;
        range = 18.0f;
        bulletSpeed = 1700.0f;

        soldier = new GameObject[4];
    }
}
