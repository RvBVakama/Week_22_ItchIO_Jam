using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour {


    public float Speed { get; set; }
    public bool Freeze { get; set; }

    Rigidbody2D rb;

    public Enemy() { }
    public Enemy(float Speed)
    {
        this.Speed = Speed;
    }

	// Use this for initialization
	void Start () {

        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        EnemyStart();
	}
	
	// Update is called once per frame
	void Update () {
        if (Freeze)
            return;
        EnemyUpdate();
	}


    public void FollowObject(string targetTag)
    {
        float xValue = rb.velocity.x;
        //This will probs drop FPS :D //////////////////////////////////
        GameObject target = GameObject.FindGameObjectWithTag(targetTag);
        ////////////////////////////////////////////////////////////////
        //Get the direction of the current target
        Vector3 dir = target.transform.position - gameObject.transform.position;
        //Add the force to move towards the current target
        rb.AddForce(dir * Speed);

        if (xValue < 0.1f)
        {
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0, 180, 0), 0.05f);
        }
        else if(xValue > -0.1f)
        {
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0, 0, 0), 0.05f);
        }
    }


    public abstract void EnemyStart();
    public abstract void EnemyUpdate();
}
