using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{

    Text t, t2;
    public float time0,time2,time3;
    //falseのときtimeScale = 0  trueのときtimeScale = 1
    bool isScale = false;
    GameObject enemy;
    GameObject wr;
    Image arrowIM, arrowIM2, arrowIM3;

    int a = 0;
    public int count = 0;
    public int p = 0;

    bool isFinish = false;

    // Use this for initialization
    void Start()
    {
        time0 = 0;
        time2 = 0;
        time3 = 0;

        t = GameObject.Find("TutorialText").GetComponent<Text>();
        t2 = GameObject.Find("EnterText").GetComponent<Text>();

        wr = GameObject.Find("WorldRange");
        wr.SetActive(false);

        enemy = GameObject.Find("Enemy");
        enemy.SetActive(false);

        arrowIM = GameObject.Find("GageArrow").GetComponent<Image>();
        arrowIM.color = new Color(0, 0, 0, 0);

        arrowIM2 = GameObject.Find("HpArrow").GetComponent<Image>();
        arrowIM2.color = new Color(0, 0, 0, 0);

        arrowIM3 = GameObject.Find("TimerArrow").GetComponent<Image>();
        arrowIM3.color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        isScale = true;

        if (isScale)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }


        if (isFinish && (Input.GetKey(KeyCode.Return)))
        {
                SceneManager.LoadScene("Title");
        }

        switch (count)
        {
            case 0:
                Basic();
                break;
            case 1:
                Dis();
                break;
            case 2:
                Pause();
                break;
            case 3:
                Finish();
                break;
        }
    }

    void Basic()
    {
        time0 += Time.deltaTime;
        switch ((int)time0)
        {
            case 1:
                t.text = "【基本操作】\n" + " ↑↓→←で回転移動\n" + "Wで加速 Sで減速します";
                break;
            case 6:
                t.text = "Fで弾丸を発射\n" + "Vで視点を切り替えることができます";
                break;
            case 10:
                t.text = "この世界のどこかに星があります\n" + "探して近づいてみましょう";
                wr.SetActive(true);
                break;
            case 15:
                t.text = "";
                count = 1;
                break;
        }
    }

    void Dis()
    {
        if (PlayerControl.isDis)
        {
            a++;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                p += 1;
            }

            switch (p)
            {
                case 0:
                    Time.timeScale = 0;
                    t.text = "星の重力範囲内に入りました\n" + "ゲームでは星に衝突したらゲームオーバーです";
                    t2.text = "Enterキーを押してください";
                    t2.color = new Color(210, 0, 0, Mathf.Sin(a * 0.05f));
                    break;
                case 1:
                    Time.timeScale = 0;
                    t.text = "星の周りにある青い球体は資源です";
                    t2.text = "Enterキーを押してください";
                    t2.color = new Color(210, 0, 0, Mathf.Sin(a * 0.05f));
                    break;
                case 2:
                    Time.timeScale = 0;
                    t.text = "近づいて資源を回収してみてください";
                    t2.text = "Enterキーを押してください";
                    t2.color = new Color(210, 0, 0, Mathf.Sin(a * 0.05f));
                    break;
                case 3:
                    t.text = "";
                    t2.text = "";
                    Time.timeScale = 1;
                    if (PlayerControl.isGetItem)
                    {
                        Time.timeScale = 0;
                        t.text = "ゲージに変化がありました";
                        arrowIM.color = new Color(0, 0, 255, 255);
                        t2.text = "Enterキーを押してください";
                        t2.color = new Color(210, 0, 0, Mathf.Sin(a * 0.05f));
                    }
                    break;
                case 4:
                    Time.timeScale = 0;
                    t.text = "青いゲージがプレイヤーの資源回収率です";
                    t2.text = "Enterキーを押してください";
                    t2.color = new Color(210, 0, 0, Mathf.Sin(a * 0.05f));
                    break;
                case 5:
                    Time.timeScale = 0;
                    t.text = "赤の敵ゲージに負けないように\n"+"資源を回収しましょう";
                    t2.text = "Enterキーを押してください";
                    t2.color = new Color(210, 0, 0, Mathf.Sin(a * 0.05f));
                    break;
                case 6:
                    arrowIM.color = new Color(0, 0, 0, 0);
                    Time.timeScale = 0;
                    t.text = "今度はスピードを上げて\n" + "重力範囲内から出てみましょう";
                    t2.text = "Enterキーを押してください";
                    t2.color = new Color(210, 0, 0, Mathf.Sin(a * 0.05f));
                    break;
                case 7:
                    t.text = "";
                    t2.color = new Color(0, 0, 0, 0);
                    Time.timeScale = 1;
                    break;
            }
        }
        if (PlayerControl.isExit)
        {
            count = 2;
        }
    }

    void Pause()
    {
        time2 += Time.deltaTime;
        switch ((int)time2)
        {
            case 1:
                t.text = "重力範囲外に出ることができましたね";
                break;
            case 3:
                t.text = "続いてスペースキーを押してください";
                break;
            case 4:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    t.text = "ポーズ画面を表示します";
                }
                break;
            case 6:
                t.text = "もう一度スペースキーでポーズ画面を終了します";
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    count = 3;
                }
                break;
            case 7:
                t.text = "";
                count = 3;
                break;
        }
    }

    void Finish()
    {
        time3 += Time.deltaTime;
        switch ((int)time3)
        {
            case 1:
                t.text = "これで操作説明は終了です";
                break;
            case 4:
                t.text = "最後にUIの説明です";
                break;
            case 7:
                t.text = "これは体力ゲージです";
                arrowIM2.color = new Color(0, 0, 255, 255);
                break;
            case 10:
                t.text = "敵に衝突したり、敵に撃たれると\n"+"ダメージを受けます\n"+"ゲージが0になるとゲームオーバーです";
                break;
            case 13:
                arrowIM2.color = new Color(0, 0, 0, 0);
                t.text = "続いてこれは制限時間です";
                arrowIM3.color = new Color(0, 0, 255, 255);
                break;
            case 16:
                t.text = "0になるとゲーム終了です";
                arrowIM3.color = new Color(0, 0, 0, 0);
                break;
            case 17:
                t.text = "クリア条件は\n" + "制限時間以内に敵より多く資源を集める";
                break;
            case 20:
                t.text = "または、敵を倒したらゲームクリアです";
                break;
            case 23:
                t.text = "これでチュートリアルは終了です";
                break;
            case 25:
                t.text = "Enterキーでタイトルに戻ります";
                isFinish = true;
                break;
        }
    }
}