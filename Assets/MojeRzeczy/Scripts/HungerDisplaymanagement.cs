using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerDisplaymanagement : MonoBehaviour {
    Text text;
    float hunger;
    int fish;
    public GameObject player;
    PlayerController2D playerController;
    // Use this for initialization
    void Start () {
        text = GetComponent<Text>();
        playerController = player.GetComponent<PlayerController2D>();
	}
	
	// Update is called once per frame
	void Update () {
        hunger = playerController.GetHunger();
        fish = playerController.GetFish();
        text.text = "Hunger:" + Mathf.Round(hunger) + "\n\nFish:" + fish+"\n\n Health:"+playerController.health;
	}
}
