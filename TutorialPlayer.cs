using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialPlayer : MonoBehaviour
{

    public float move;
    [SerializeField, Range(0.001f, 1f), Tooltip("速度加算値\n速度減算値は1/2")]
    float speed;
    [SerializeField, Range(0, 10), Tooltip("回転速度")]
    float RotateSpeed;
    [SerializeField, Range(0, -20), Tooltip("カメラの奥行")]
    private float z_camPosition;

    public static float ItemCount;  //ワールドに生成されたItemの数(Scoreで使いたい)
    public static float Score;  //スコア

    //CameraMoveでRotateSpeedを使用したい
    public static float rotateSpeed;
    public static float PlayerSpeed;
    //引力が働く範囲に入ったか判定する CameraMoveで使いたい
    public static bool isDis;

    private Slider slider;  //Hp
    public static bool isDamage;

    float time = 1;  //sphereに衝突してから爆発アニメーションまでの時間
    public static bool isExplosion = false;  //アニメーションがActiveかどうか
    GameObject explosion;

    //falseはTPS
    public static bool IsMode = false;

    public GameObject bullet;
    [SerializeField, Range(100, 1000), Tooltip("バレットの発射速度")]
    float bulletSpeed;
    [SerializeField, Range(0, 10), Tooltip("バレットの消失時間")]
    float destroyTime;

    // Use this for initialization
    void Start()
    {
        isDis = false;
        IsMode = false;
        isDamage = false;
        Score = 0;

        transform.position = GameObject.Find("Center").transform.position;

        move = speed / 10;//最低速度

        rotateSpeed = RotateSpeed;
        PlayerSpeed = speed;

        slider = GameObject.Find("Slider").GetComponent<Slider>();
        slider.value = 10;//HpMax

        explosion = GameObject.Find("Explosion");
        explosion.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ItemCount = GameObject.FindGameObjectsWithTag("Items").Length + Score + Enemy.enemyScore;

        PlayerMove();
        Rotate();
    }

    void PlayerMove()
    {
        Transform cam = Camera.main.transform;

        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            Shot();
        }

        if (!isDis)
        {
            if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                IsMode = !IsMode;
            }

            cam.position = (IsMode) ? transform.position + cam.TransformDirection(new Vector3(0, 4, z_camPosition + 40)) :
                cam.position = transform.position + cam.TransformDirection(new Vector3(0, 4, z_camPosition));
        }
        else
        {
            if (MoveCamera.isRotate)
            {
                cam.position = transform.position + cam.TransformDirection(new Vector3(0, 4, z_camPosition));
            }
            else
            {

                cam.position += (MoveCamera.isRightIn) ? cam.TransformDirection(new Vector3(3, 0, MoveCamera.centerZ)) :
                    cam.TransformDirection(new Vector3(-3, 0, MoveCamera.centerZ));
            }
        }

        //加速
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Joystick1Button4))
        {
            move += Input.GetAxis("Vertical") * Time.deltaTime * speed * Time.timeScale;
        }
        //減速
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.Joystick1Button5))
        {
            move += Input.GetAxis("Vertical") * Time.deltaTime * (speed / 2f);
            if (move <= speed / 10)
            {
                move = speed / 10 * Time.timeScale;
            }
        }
        transform.position += transform.forward * move * Time.timeScale;
    }

    //先輩のスクリプトの変更
    public void Shot()
    {
        GameObject bullets = new GameObject();
        bullets = Instantiate(bullet) as GameObject;
        bullets.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed * 100);
        bullets.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
        Destroy(bullets, destroyTime);
    }

    public void Rotate()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, RotateSpeed * Time.timeScale, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -RotateSpeed * Time.timeScale, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(RotateSpeed * Time.timeScale, 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(-RotateSpeed * Time.timeScale, 0, 0);
        }

        transform.Rotate(Input.GetAxis("Vertical2") * Time.timeScale, Input.GetAxis("Horizontal2") * Time.timeScale, 0);

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isDamage = true;
            slider.value -= 1f;
        }
        if (other.gameObject.tag == "Pranet")
        {
            isExplosion = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Bullet")
        {
            isDamage = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Bullet")
        {
            isDamage = true;
            slider.value -= 0.5f;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Items")
        {
            //Resouceの削除
            Destroy(other.gameObject);
            //スコア加算 +1
            Score++;
        }
    }

    void OnTriggerStay(Collider other)
    {
        //引力が働く範囲に入ったら
        if (other.gameObject.tag == "testTag")
        {
            isDis = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //引力が働く範囲に出たら
        if (other.gameObject.tag == "testTag")
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            isDis = false;
        }
    }
}