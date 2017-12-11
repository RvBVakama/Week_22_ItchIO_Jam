using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Eye : Enemy
{
    public Enemy1Eye() : base(3) { }
    public override void EnemyStart()
    {
        Gravity(1);
    }

    public override void EnemyUpdate()
    {
        PingPong();
    }
}
