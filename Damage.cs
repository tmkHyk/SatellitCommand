using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour {
    
    private Image img;
    float alpha;
    private float count;

    private bool isUp;

	// Use this for initialization
	void Start () {

        img = GameObject.Find("DamagePanel").GetComponent<Image>();
        alpha = 0;

        isUp = false;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerControl.isDamage)
        {
            count += Time.deltaTime;

            if (count <= 3)
            {

                if (alpha <= 0)
                {
                    isUp = true;
                }
                if (alpha > 0.2f)
                {
                    isUp = false;
                    count++;
                }

                alpha = (isUp) ? alpha + 0.01f : alpha - 0.01f;
            }
            else
            {
                count = 0;
            }
        }
        else
        {
            isUp = true;
            alpha = 0;
        }

        img.color = new Color(255, 0, 0, alpha);
    }
}
