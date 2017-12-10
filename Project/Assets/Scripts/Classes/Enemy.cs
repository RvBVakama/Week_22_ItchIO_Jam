using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EnemyStart();
	}
	
	// Update is called once per frame
	void Update () {
        EnemyUpdate();
	}



    public abstract void EnemyStart();
    public abstract void EnemyUpdate();
}
