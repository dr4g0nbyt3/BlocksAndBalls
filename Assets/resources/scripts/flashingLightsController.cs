using UnityEngine;
using System;
using System.Collections;

public class flashingLightsController : MonoBehaviour {


    

	// Use this for initialization
	void Start ()
    {
       
        Invoke("Delete", 1.99f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        LerpAlpha();
	}

    void Delete()
    {
        Destroy(gameObject);
    }

    void LerpAlpha()
    {
        //float lerp = Mathf.PingPong(Time.time, duration) / duration;
        //myColor.a = Mathf.Lerp(0.0f, 1.0f, lerp);
        //GetComponent<SpriteRenderer>().color = myColor; 
            
    }
}
