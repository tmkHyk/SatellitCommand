using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sound : MonoBehaviour {

    //BGM
    public AudioClip[] ac;
    //ac[0] title
    //ac[1] stage3
    //ac[2] stage2
    //ac[3] title3

    //SE
    public AudioClip[] se;
    //se[0] select2
    //se[1] select
    //se[2] damage2
    //se[3] shoot2
    //se[4] explosion3

    AudioSource aso, asoSE;

    // Use this for initialization
    void Start()
    {
        aso = GetComponent<AudioSource>();
        asoSE = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(gameObject);

        //BGM
        if (!aso.isPlaying)
        {
            aso.Play();
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "Title":
                aso.clip = ac[0];
                break;
            case "SampleDebug":
                if (!JumpSceneScript.isReset)
                {
                    aso.clip = ac[1];
                }
                else
                {
                    aso.Stop();
                    JumpSceneScript.isReset = false;
                }
                break;
            case "GameClearScene":
                aso.clip = ac[2];
                break;
            case "GameOverScene":
                aso.clip = ac[3];
                break;
        }

        //SE
        switch (SceneManager.GetActiveScene().name)
        {
            case "Title":
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)
                    || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    asoSE.PlayOneShot(se[0]);
                }
                //Enterが反応しない
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    asoSE.PlayOneShot(se[1]);
                }
                break;
            case "SampleDebug":
                //Pauseでのセレクト                
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)
                    || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    asoSE.PlayOneShot(se[0]);
                }
                if (PlayerControl.isDamage)
                {
                    asoSE.PlayOneShot(se[2]);
                }
                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Joystick1Button1))
                {
                    asoSE.PlayOneShot(se[3]);
                }
                if (PlayerControl.isExplosion)
                {
                    asoSE.PlayOneShot(se[4]);
                }
                break;
            case "GameClearScene":
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)
                    || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    asoSE.PlayOneShot(se[0]);
                }
                break;
            case "GameOverScene":
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)
                    || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    asoSE.PlayOneShot(se[0]);
                }
                break;
        }
    }
}
