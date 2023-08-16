using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingToWater : MonoBehaviour {


    void OnTriggerExit2D(Collider2D player)
    {
        if (player.tag == "Player")
        {
            PlayerController2D PlayContr = player.gameObject.GetComponent<PlayerController2D>();
            if (PlayContr != null)
            {
                PlayContr.GetOffGround();
            }
        }
    }
}
