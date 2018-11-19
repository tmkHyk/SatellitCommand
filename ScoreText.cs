using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    private Text t, t1;

	// Use this for initialization
	void Start () {

        t = GameObject.Find("PlayerScoreText").GetComponent<Text>();
        t1 = GameObject.Find("EnemyScoreText").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        
        t.text = (PlayerControl.Score).ToString();
        t1.text = (Enemy.enemyScore).ToString();
	}
}
