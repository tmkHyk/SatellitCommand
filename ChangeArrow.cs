using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeArrow : MonoBehaviour {

    GameObject arrow;
    GameObject reticle;

	// Use this for initialization
	void Start () {
        arrow = GameObject.Find("Arrow");
        reticle = GameObject.Find("Reticle");
	}
	
	// Update is called once per frame
	void Update () {
        
        arrow.SetActive(!PlayerControl.IsMode);
        reticle.SetActive(PlayerControl.IsMode);

    }
}
