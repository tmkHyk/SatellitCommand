using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLight : MonoBehaviour {

    [SerializeField, Range(0, 500), Tooltip("プレイヤーとアイテムとの距離")]
    float distance;
    [SerializeField, Range(0, 1), Tooltip("点滅速度")]
    float flashingSpeed;

    GameObject player;

    Light light;
    bool isNear;
    bool isUp;

    public float nowDistance;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {

        nowDistance = Vector3.Distance(gameObject.transform.position, player.transform.position);

        isNear = (nowDistance < distance) ? true : false;

        if (isNear)
        {
            if (light.intensity <= 0)
            {
                isUp = true;
            }
            if (light.intensity >= 2)
            {
                isUp = false;
            }

            light.intensity += (isUp) ? flashingSpeed : -flashingSpeed;
        }
        else
        {
            light.intensity = 0;
        }
	}
}
