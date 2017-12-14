using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour {


    public float Speed { get; set; }
    public bool Freeze { get; set; }

    Rigidbody2D rb;
    GameManager gm;
    bool pingpongSwitch = false;

    private void Awake()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        gm.RegisterEnemy(this);
  
    }

    public Enemy() { }
    public Enemy(float Speed)
    {
        this.Speed = Speed;
    }

	// Use this for initialization
	void Start () {

        rb = gameObject.GetComponent<Rigidbody2D>();
        
        EnemyStart();
	}
	
	// Update is called once per frame
	void Update () {
        if (Freeze)
            return;
        EnemyUpdate();
	}

    public void Gravity(float grav)
    {
        rb.gravityScale = grav;
    }

    public void PingPong()
    {
        List<GameObject> sensors =  new List<GameObject>();
        foreach(Transform t in gameObject.transform)
        {
            if(t.tag == "Sensor")
            {
                sensors.Add(t.gameObject);
            }
            
        }
        foreach(GameObject s in sensors)
        {
            if(!Physics2D.Raycast(s.gameObject.transform.position, Vector2.down))
            {
                pingpongSwitch = !pingpongSwitch;
            }
        }
        if(pingpongSwitch)
        {
            gameObject.transform.Translate(Vector2.left * Time.deltaTime * Speed);
        }
        else
        {
            gameObject.transform.Translate(Vector2.right * Time.deltaTime * Speed);
        }
    }

    public void FollowObject(string targetTag)
    {
        float xValue = rb.velocity.x;
        //This will probs drop FPS :D //////////////////////////////////
        GameObject target = GameObject.FindGameObjectWithTag(targetTag);
        ////////////////////////////////////////////////////////////////
        //Get the direction of the current target
        if (target == null)
            return;
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
        foreach(MapObject g in GameObject.FindObjectsOfType<MapObject>())
        {
            if (g.gameObject.GetComponent<Player>() == null)
            {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), g.GetComponent<Collider2D>(), true);
                foreach (Transform t in gameObject.transform)
                {
                    Physics2D.IgnoreCollision(t.gameObject.GetComponent<Collider2D>(), g.GetComponent<Collider2D>(), true);
                }
            }
        }
    }


    public abstract void EnemyStart();
    public abstract void EnemyUpdate();
}
