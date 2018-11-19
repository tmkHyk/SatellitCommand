using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    private Slider slider, slider2;
    private GameObject[] items;

	// Use this for initialization
	void Start () {
        slider = GameObject.Find("PlayerScoreSlider").GetComponent<Slider>();
        slider2 = GameObject.Find("RemainderSlider2").GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {

        float playerScore = PlayerControl.Score / PlayerControl.ItemCount;
        float remainder = (PlayerControl.ItemCount - Enemy.enemyScore) / PlayerControl.ItemCount;

        slider.value = (playerScore == 0) ? 0 : playerScore;
        slider2.value = (remainder == 0) ? 0 : remainder;
    }
}