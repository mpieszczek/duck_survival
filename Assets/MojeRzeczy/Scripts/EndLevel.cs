using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D player)
    {

        if (player.tag == "Player")
        {
            //Input ewentualnie tu wstawić
            PlayerController2D PlayContr = player.gameObject.GetComponent<PlayerController2D>();
            if (PlayContr != null)
            {
                PlayContr.EndLevel();
            }
        }

    }
}
