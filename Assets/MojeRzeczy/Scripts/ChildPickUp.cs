using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildPickUp : MonoBehaviour {
    public float ChildCallories = 50;
    public Sprite BloodSprite;
    public float StainStayTime = 3;

    bool IsDead = false;
    float DeathTime;

    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.tag == "Player")
        {
            //Input ewentualnie tu wstawić
            PlayerController2D PlayContr = player.gameObject.GetComponent<PlayerController2D>();
            if (PlayContr != null)
            {
                PlayContr.EatChild(ChildCallories);
                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().sprite = BloodSprite;
                IsDead = true;
                DeathTime = Time.time;
            }
        }
    }
    void Update()
    {  
        if(IsDead)
        {
            if (Time.time - DeathTime > StainStayTime)
            {
                Destroy(gameObject);
            }
        }

    }
}
