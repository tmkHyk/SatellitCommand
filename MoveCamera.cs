using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    GameObject player;
    [SerializeField, Range(0.1f, 2), Tooltip("カメラの回転速度")]
    float rotateSpeed;
    [SerializeField, Range(0, 60), Tooltip("回転可能の角度範囲")]
    float rotate;
    [SerializeField, Range(0.1f, 1), Tooltip("重力範囲内にplayerがいるときのカメラの距離")]
    float camDistance;

    float angle;  //2つのベクトルのなす角
    Vector3 cross;  //2つのベクトルの外積
    float Nowdisntance = 0;  //playerと検索対象のsphereの現在の距離
    GameObject nearSphere;  //最も近くにあるsphere

    public GameObject centerPos;  //playerと最も近いsphereの中心座標に置くオブジェクト
    public static float centerZ;  //PlayerControlに使いたい
    public static bool isRightIn;  //PlayerControlに使いたい

    public static bool isRotate = true;  //Rotateメソッドが使用されている

    Rigidbody rb;
   
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {       
        Serch();
        
        //重力がが働いていないなら範囲指定しながら回転
        if (!PlayerControl.isDis)
        {
            Rotate();
            isRotate = false;
        }
        else
        //重力が働いているならcenterPosででplayerに追従
        {
            if (!isRotate)
            {
                CenterPos();
                if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.Joystick1Button3))
                {
                    isRotate = true;
                }
            }
            else
            {
                Rotate();
                if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.Joystick1Button3))
                {
                    isRotate = false;
                }
            }
        }
    }

    //回転制限
    void Rotate()
    {
        //playerとcameraの各ベクトルがなす角度
        angle = Vector3.Angle(transform.forward, player.transform.forward);
        //playerとcameraの外積
        cross = Vector3.Cross(transform.forward, player.transform.forward);
        //外積が0以下ならangleを負にする
        if (cross.y > 0)
        {
            angle *= -1;
        }
        //回転速度をrotateSpeedでplayerに追従回転
        transform.rotation = Quaternion.RotateTowards(transform.rotation, player.transform.rotation, rotateSpeed);

        if (angle >= rotate)  //rotate以上回らない
        {
            transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, player.transform.localEulerAngles.y + rotate, transform.localEulerAngles.z);
        }
        else if (angle <= -rotate)  //rotate以上回らない
        {
            transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, player.transform.localEulerAngles.y - rotate, transform.localEulerAngles.z);
        }
    }
    
    void CenterPos()
    {
        //playerと最も近いsphereの中心値をcenterPosに
        centerPos.transform.position = (player.transform.position + nearSphere.transform.position) / 2;
        //centerPosをplayerに追従回転
        centerPos.transform.rotation = Quaternion.LookRotation(centerPos.transform.position - player.transform.position);
        //playerとsphereが円周を通る円の中心centerPosからの距離centerZをcameraのz値に
        centerZ = Vector3.Distance(centerPos.transform.position, nearSphere.transform.position)
            * Mathf.Sin(Camera.main.fieldOfView * 0.5f);
        
        //centerPosのスクリーン座標
        Vector3 cPos = Camera.main.WorldToScreenPoint(centerPos.transform.position);
        //最も近くにあるsphereのスクリーン座標
        Vector3 sPos = Camera.main.WorldToScreenPoint(nearSphere.transform.position);

        Camera.main.depth = centerZ;
        //transform.position = new Vector3(centerPos.transform.position.x, player.transform.position.y, centerPos.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, centerPos.transform.position, camDistance);

        //centerがsphereよりスクリーン上で左にいる場合
        if (cPos.x < sPos.x)
        {
            isRightIn = false;
            //sphereを中心に時計回り
            transform.rotation = Quaternion.LookRotation(-centerPos.transform.right);
        }
        else
        //centerがsphereよりスクリーン上で右にいる場合
        {
            isRightIn = true;
            //sphereを中心に反時計回り
            transform.rotation = Quaternion.LookRotation(centerPos.transform.right);
        }
    }

    //Playerと最も近いSphereを検索
    void Serch()
    {
        float Neardistance = 0;
        foreach (GameObject target in GameObject.FindGameObjectsWithTag("Pranet"))
        {
            Nowdisntance = Vector3.Distance(target.transform.position, player.transform.position);
            if (Neardistance == 0 || Neardistance > Nowdisntance)
            {
                //新たに更新
                Neardistance = Nowdisntance;
                nearSphere = target;
            }
        }
    }
}