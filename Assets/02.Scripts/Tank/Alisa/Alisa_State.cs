using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alisa_State : Tank_State {
    void Awake()
    {
        level = 1;
        exp = 0;
        moveSpeed = 16.0f;
        maxHp = 160;
        hp = 160;
        tankType = 2;
        tankDefensive = 11;
        bulletAttribute = 1;
        bulletSize = 1.0f;
        damage = 24;
        firstDamage = 24;
        fireRate = 0.8f;
        direct = 1f;
        forward = Vector3.forward;
        attAply = 0;
        range = 20.0f;
        bulletSpeed = 1600.0f;

        soldier = new GameObject[4];
    }
}
