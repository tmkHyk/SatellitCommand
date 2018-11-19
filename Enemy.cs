using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    GameObject player;

    public bool isChase = false;
    [SerializeField, Range(0, 1), Tooltip("敵の回転速度")]
    private float rotationSpeed;
    [SerializeField, Range(0, 1), Tooltip("速度")]
    private float speed;
    [SerializeField, Range(0, 50), Tooltip("追従開始距離")]
    float chaseDistance;
    [SerializeField, Range(0, 200), Tooltip("発射開始距離")]
    float shotDistance;

    GameObject target;//追跡するターゲット
    float Nowdisntance = 0;//自身との距離を測る距離用変数（初期値は０）

    public GameObject bullet;
    GameObject bullets; //複製用
    bool isShot = false; //発射判定
    int x = 0;
    [SerializeField, Range(100, 1000), Tooltip("弾丸のスピード")]
    float bulletspeed;
    [SerializeField, Range(50, 300), Tooltip("発射間隔(mm/s)")]
    float shotInterval;
    [SerializeField, Range(0, 5), Tooltip("弾丸の消滅時間")]
    float destroyTime;

    GameObject[] resources;  //フィールド上にある資源
    Vector3 fw;  //初期速度    
    public static float enemyScore;  //敵のスコア

    public Slider slider;

    GameObject explosion2;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        fw = transform.forward * speed;
        enemyScore = 0;

        slider.value = 10;

        explosion2 = GameObject.Find("Explosion2");
        explosion2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        foreach (var target in resources)
        {
            Physics.OverlapSphere(target.transform.position, 10);
            transform.position += transform.forward * speed;
        }
        */

        Chase();

        if (isChase)
        {
            //Playerを追従
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), rotationSpeed);
            transform.position += transform.forward * speed * Time.timeScale;
        }

        if (slider.value <= 0)
        {
            //Destroy(this.gameObject, 2.0f);
            PlayerControl.isDamage = false;
            SceneManager.LoadScene("GameClearScene");

            explosion2.SetActive(true);
        }
    }

    /// <summary>
    /// サーチするメソッド
    /// </summary>
    /// <param name="nowObj">自分</param>
    /// <param name="target_tagName">ターゲットが持つタグの名前</param>
    /// <returns>計算結果のオブジェクトを返す</returns>
    GameObject EnemySerch(GameObject nowObj, string target_tagName)
    {
        //サーチするオブジェクトを宣言
        GameObject targetObject = null;
        //一番近い距離を測る距離用変数(初期値は０）
        float Neardistance = 0;
        //指定されたオブジェクトを配列ですべて取得する
        foreach (GameObject targets in GameObject.FindGameObjectsWithTag(target_tagName))
        {
            //ターゲットとの距離を測る
            Nowdisntance = Vector3.Distance(targets.transform.position, nowObj.transform.position);
            //最初に測った距離より近くの距離に来た場合
            if (Neardistance == 0 || Neardistance > Nowdisntance)
            {
                //新たに更新
                Neardistance = Nowdisntance;
                targetObject = targets;
            }
        }
        //最終的に近いオブジェクトをターゲットにしてあげる
        return targetObject;
    }

    void Chase()
    {
        Shot();

        //Playerとの距離を計算
        target = EnemySerch(gameObject, "Player");

        //Playerとの距離がchaseDistanceより近い時追従開始
        if (Nowdisntance <= chaseDistance)
        {
            isChase = true;
        }
        else
        {
            isChase = false;
            Serch();
        }
    }

    void Shot()
    {
        //Playerとの距離を計算
        target = EnemySerch(gameObject, "Player");
        //Playerとの距離がshotDistanceより近い時発射開始
        if (Nowdisntance <= shotDistance)
        {
            x++;
            //shotIntervalの間隔で発射
            if (x >= shotInterval)
            {
                bullets = Instantiate(bullet) as GameObject;
                bullets.GetComponent<Rigidbody>().AddForce(transform.forward * bulletspeed);
                bullets.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
                Destroy(bullets, destroyTime);
                x = 0;
            }
            else if (x >= shotInterval)
            {
                x = 0;
            }
        }
    }

    void Serch()
    {
        resources = GameObject.FindGameObjectsWithTag("Items");

        //Resourceが無ければPlayerを追従
        if (resources == null)
        {
            isChase = true;
        }
        //ResourceがあればResourceへ向かう
        else
        {
            isChase = false;

            //最も近いItemを探す
            target = EnemySerch(gameObject, "Items");

            //Itemがあるとき
            if (target != null)
            {
                //最も近いItemへ移動
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), rotationSpeed);
                transform.position += transform.forward * speed * Time.timeScale;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Resourceに衝突したらResourceの削除
        if (other.gameObject.tag == "Items")
        {
            //スコア加算 +1
            enemyScore++;
            //Resourceの削除
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Bullet")
        {
            slider.value -= 1;
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "testTag")
        {
            transform.position += transform.forward * 0.1f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "testTag")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position += transform.forward * speed;
        }
    }
}