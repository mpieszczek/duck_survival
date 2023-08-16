using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerDropPickUp : MonoBehaviour {

    public int amount=1;
    public GameObject[] Defenders;
    private EnemyBird temp;
    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.tag == "Player")
        {
            //Input ewentualnie tu wstawić
            PlayerController2D PlayContr = player.gameObject.GetComponent<PlayerController2D>();
            if (PlayContr != null)
            {
                PlayContr.FishPickUp(amount);
                foreach(GameObject def in Defenders)
                {
                    temp = def.GetComponent<EnemyBird>();
                    if (temp != null)
                    {
                        temp.Chase();
                    }
                }
                Destroy(gameObject);
            }
        }
    }
}