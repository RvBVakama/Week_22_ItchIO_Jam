﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZack : Enemy
{

    public EnemyZack() : base(2) { }

    public override void EnemyStart()
    {

    }

    public override void EnemyUpdate()
    {
        FollowObject("Player");
    }
}
