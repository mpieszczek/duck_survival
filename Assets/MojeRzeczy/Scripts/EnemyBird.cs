using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBird : MonoBehaviour {
    bool chasing=false;
    Transform playerPosition;
    public float chasingDistance = 10;
    float distance;
    public float speed=1;
    Vector2 throwback;
    Animator anim;
    Vector3 NestPosition;
	// Use this for initialization
	void Start () {
        playerPosition = GameObject.FindWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        NestPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (chasing)
        {
            if (playerPosition != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerPosition.position, speed * Time.fixedDeltaTime);
                Quaternion rotation = Quaternion.LookRotation(playerPosition.position - transform.position, transform.TransformDirection(-Vector3.forward));
                transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            }
            else chasing = false;
        }
        else if (!chasing && transform.position != NestPosition)
        {
            Quaternion rotation = Quaternion.LookRotation(NestPosition - transform.position, transform.TransformDirection(-Vector3.forward));
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            transform.position = Vector2.MoveTowards(transform.position, NestPosition, speed * Time.fixedDeltaTime);
                
        }
        else
        {
            anim.SetBool("chasing", false);
        }
	}
    public void Chase()
    {
        chasing = true;
        anim.SetBool("chasing", true);
        InvokeRepeating("CheckDistance", 0, 2f);
    }
    void OnTriggerEnter2D(Collider2D player)
    {
        if(player.tag == "Player")
        {
            throwback = player.transform.position - transform.position;
            throwback.Normalize();
            player.GetComponent<PlayerController2D>().DealDamage(1,throwback);
            chasing = false;
            CancelInvoke();
        }
    }
    void CheckDistance()
    {
        if (playerPosition != null)
        {
            distance = (playerPosition.position - transform.position).magnitude;
            if (distance > chasingDistance)
            {
                chasing = false;
                CancelInvoke();
            }
        }
    }
}
