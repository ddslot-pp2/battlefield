using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robin_State : Tank_State {
    void Awake()
    {
        level = 1;
        exp = 0;
        moveSpeed = 11.0f;
        maxHp = 280;
        hp = 280;
        tankType = 5;
        tankDefensive = 18;
        bulletAttribute = 1;
        bulletSize = 1;
        damage = 80;
        firstDamage = 80;
        fireRate = 3.0f;
        direct = 1f;
        forward = Vector3.forward;
        attAply = 0;
        range = 240;
        bulletSpeed = 3000.0f;

        soldier = new GameObject[4];
    }

}
