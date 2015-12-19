// Developer: Dr4g0nbyt3
// Date: November 15th, 2015
// Email: dr4g0nbyt3@gmail.com
// References: http://gamedev.stackexchange.com/questions/79546/how-do-you-handle-aspect-ratio-differences-with-unity-2d


using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof (Camera))]
public class aspectRatioFix : MonoBehaviour {

    // region FIELDS
    public Color wireColor = Color.white;
    // Size of your scene in unity units
    public float UnitsSize = 1;
    public static CameraFit Instance;
    public new Camera camera;

    // Screen size
    private float screenWidth;
    private float screenHeight;

    private Vector3 bottomLeft;
    private Vector3 bottomCenter;
    private Vector3 bottomRight;

    private Vector3 middleLeft;
    private Vector3 middleCenter;
    private Vector3 middleRight;

    private Vector3 topLeft;
    private Vector3 topCenter;
    private Vector3 topRight;

    public float ScreenWidth
    {
        get
        {
            return screenWidth;
        }
    }

    public float ScreenHeight
    {
        get
        {
            return screenHeight;
        }
    }

    public Vector3 BottomLeft
    {
        get
        {
            return bottomLeft;
        }
    }

    public Vector3 BottomCenter
    {
        get
        {
            return bottomCenter;
        }
    }

    public Vector3 BottomRight
    {
        get
        {
            return bottomRight;
        }
    }

    public Vector3 MiddleLeft
    {
        get
        {
            return middleLeft;
        }
    }

    public Vector3 MiddleCenter
    {
        get
        {
            return middleCenter;
        }
    }

    public Vector3 MiddleRight
    {
        get
        {
            return middleRight;
        }
    }

    public Vector3 TopLeft
    {
        get
        {
            return topLeft;
        }
    }

    public Vector3 TopCenter
    {
        get
        {
            return topCenter;
        }
    }

    public Vector3 TopRight
    {
        get
        {
            return topRight;
        }
    }


    private void Awake()
    {
        camera = GetComponent<Camera>();
        ComputeResolution();
    }

    private void ComputeResolution()
    {
        float deviceWidth;
        float deviceHeight;
        float leftX;
        float rightX;
        float topY;
        float bottomY;

        deviceWidth = Screen.width;
        deviceHeight = Screen.height;

        if(deviceHeight < deviceWidth)
        {
            camera.orthographicSize = 1f / camera.aspect * UnitsSize / 2f;
        }
        else
        {
            camera.orthographicSize = UnitsSize / 2f;
        }

        screenHeight = 2f * camera.orthographicSize;
        screenWidth = screenHeight * camera.aspect;

        float cameraX;
        float cameraY;

        cameraX = camera.transform.position.x;
        cameraY = camera.transform.position.y;

        leftX = cameraX - screenWidth / 2;
        rightX = cameraX + screenWidth / 2;
        topY = cameraY + screenHeight / 2;
        bottomY = cameraY - screenHeight / 2;

        bottomLeft = new Vector3(-screenWidth / 2, -screenHeight / 2, 0);
        bottomCenter = new Vector3(0, -screenHeight / 2, 0);
        bottomRight = new Vector3(screenWidth / 2, -screenHeight / 2, 0);

        middleLeft = new Vector3(-screenWidth / 2, 0, 0);
        middleCenter = new Vector3(0, 0, 0);
        middleRight = new Vector3(screenWidth / 2, 0, 0);

        topLeft = new Vector3(-screenWidth / 2, screenHeight / 2, 0);
        topCenter = new Vector3(0, screenHeight / 2, 0);
        topRight = new Vector3(screenWidth / 2, screenHeight / 2, 0);

    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("The screen width: " + Screen.width);
        Debug.Log("The screen height: " + Screen.height);
    }

    public class CameraFit
    {
    }
}
