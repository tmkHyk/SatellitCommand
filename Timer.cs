using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    [SerializeField, Tooltip("時間制限")]
    private float timer;

    Text t;
    Slider slider;
    GameObject sl;

	// Use this for initialization
	void Start () {
        t = GetComponent<Text>();
        slider = GameObject.Find("Slider").GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (slider.value <= 0 || GameObject.FindGameObjectsWithTag("Items").Length == 0)
        {
            Judge();
        }

        timer -= (Time.timeScale != 0) ? Time.deltaTime : 0;

        if (timer <= 0)
        {
            timer = 0;
            Judge();
        }

        t.text = "Time : " + (timer).ToString("f1");
	}

    void Judge()
    {
        if (SceneManager.GetActiveScene().name == "SampleDebug") 
        {
            if (PlayerControl.Score >= Enemy.enemyScore)
            {
                SceneManager.LoadScene("GameClearScene");
            }
            if (PlayerControl.Score < Enemy.enemyScore)
            {
                SceneManager.LoadScene("GameOverScene");
            }
        }
    }
}