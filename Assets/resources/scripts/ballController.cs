using UnityEngine;
using System.Collections;

public class ballController : MonoBehaviour {

    public static ballController Instance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }
    
    void OnBecameInvisible()
    {
        Destroy(gameObject);

    }

    void Awake ()
    {
        if(Instance)
        {
           
            Destroy(gameObject);
        }
        else
        {

            Instance = this;
        }
    }
}
