using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MidScore : MonoBehaviour {

    private Slider slider;

	// Use this for initialization
	void Start () {

        slider = GameObject.Find("ScoreSlider").GetComponent<Slider>();
        slider.value = 0.5f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float total = PlayerControl.Score + Enemy.enemyScore;

        float score = (PlayerControl.Score == 0)?
            1- (Enemy.enemyScore/total):
            PlayerControl.Score / total;

        slider.value= (total == 0) ? 0.5f : score;

        //float midScore = (total == 0) ? 0.5f : score;
        //float a = (slider.value < midScore) ? -0.01f : 0.01f;
        //slider.value = (midScore - 0.05f < slider.value || slider.value > midScore + 0.05f) ? midScore : slider.value + a;
    }
}
