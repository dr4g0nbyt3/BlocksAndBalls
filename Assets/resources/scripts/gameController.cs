// Developers: Dr4g0nbyt3
// Date: December 1st, 2015
// Email: dr4g0nbyt3@gmail.com

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[RequireComponent(typeof(Camera))]
public class gameController : MonoBehaviour {

    // Public New Variables
    public new Camera camera;

    // Public Static Variables
    public static gameController controller;
    public static bool hasNeverBeenRun;
    
    // Private Variables
    private Vector3 topSpawnLocation;
    private Vector3 bottomSpawnLocation;
    private Vector3 ballSpawnLocation;

    // Public Variables

    public GameObject background;
    public GameObject ballPrimary;
    public GameObject ballSecondary;
    public GameObject blockPrimary;
    public GameObject blockSecondary;
    public GameObject dr4g0nbyt3Logo;
    public int highScore;
    public int score;
    public int ballsLeft;
    public string primaryColor;
    public string secondaryColor;


    // These will go
    public Color white = Color.white;
    public Color black = Color.black;
    public Sprite ballBlack;
    public Sprite ballWhite;
    public Sprite ballRed;
    public Sprite ballBlue;
    public Sprite ballYellow;
    public Sprite ballOrange;
    public Sprite ballPurple;
    public Sprite ballGreen;
    public Sprite blockBlack;
    public Sprite blockWhite;
    public Sprite blockRed;
    public Sprite blockBlue;
    public Sprite blockYellow;
    public Sprite blockOrange;
    public Sprite blockPurple;
    public Sprite blockGreen;

    // Clear this 
    private List<List<GameObject>> blockLanes = new List<List<GameObject>> { new List<GameObject>(), new List<GameObject>() };

    // Do not clear these
    private static List<string> spriteGroups = new List<string> { "balls", "blocks" };
    private static List<Dictionary<string, Sprite>> sprites = new List<Dictionary<string, Sprite>> { new Dictionary<string, Sprite>(), new Dictionary<string, Sprite>() };
    private static UnityEngine.Object[] temp;



    void Awake()
    {
        GameControllerRules();

        camera = GetComponent<Camera>();
        if (Application.loadedLevelName == "dr4g0nbyt3Intro") {
            camera.backgroundColor = black;
            EstablishControls();
            LoadAllSprites();
        }
        else if (Application.loadedLevelName == "mainMenu") {
            camera.backgroundColor = white;
        }
        else if (Application.loadedLevelName == "game") {
            camera.backgroundColor = black;
            var block2DCollider = blockPrimary.GetComponent<Collider2D>();

            topSpawnLocation = new Vector3(0, 0, 50);
            topSpawnLocation = camera.ScreenToWorldPoint(topSpawnLocation);
            topSpawnLocation.x += block2DCollider.bounds.size.x * 2;
            topSpawnLocation.y -= block2DCollider.bounds.size.y / 2;
            bottomSpawnLocation = new Vector3(0, Screen.height, 50);
            bottomSpawnLocation = camera.ScreenToWorldPoint(bottomSpawnLocation);
            bottomSpawnLocation.x -= block2DCollider.bounds.size.x * 2;
            bottomSpawnLocation.y += block2DCollider.bounds.size.y / 2;
            // ballSpawnLocation = new Vector3(0, 0, 0);
            // Set Block one and two from saved files
        }
    }

    // Use this for initialization
    void Start()
    {
        if (Application.loadedLevelName == "dr4g0nbyt3Intro") {
            // Make this instantiation fit the screen size
            Instantiate(dr4g0nbyt3Logo, new Vector3(0, 0, 3), Quaternion.identity);
            Invoke("LoadGameScene", 5.0f);
        }

        if (Application.loadedLevelName == "mainMenu") {

        }

        // if game scene
        if (Application.loadedLevelName == "game")
        {

            // Load block 1 and block 2 from saved file
            // if game scene
            InvokeRepeating("SpawnBlocks", 1f, 0.3f);
            InvokeRepeating("SwapPrimaryAndSecondary", 10.0f, 10.0f);
        }
    }

    // Update is called once per frame
    void Update() {
    }

    void GameControllerRules() {
        if (controller == null)
        {
            DontDestroyOnLoad(gameObject);
            controller = this;
        }
        else if (controller != this)
        {
            Destroy(gameObject);
        }
    }

    void SpawnBlocks() {
        blockLanes[0].Add((GameObject)Instantiate(blockPrimary, topSpawnLocation, Quaternion.identity));
        blockLanes[1].Add((GameObject)Instantiate(blockSecondary, bottomSpawnLocation, Quaternion.identity));
    }

    public void SwapPrimaryAndSecondary() {
        var temp = blockPrimary.GetComponent<SpriteRenderer>().sprite;
        blockPrimary.GetComponent<SpriteRenderer>().sprite = blockSecondary.GetComponent<SpriteRenderer>().sprite;
        blockSecondary.GetComponent<SpriteRenderer>().sprite = temp;
    }

    void LoadMainMenuScene() {
        Application.LoadLevel("mainMenu");
    }

    void LoadGameScene() {
        Application.LoadLevel("game");
    }

    void EstablishControls() {

#if UNITY_EDITOR
        Debug.Log("Unity Editor");
        // Controls for blocks and balls Down arrow down Up arrow up Esc Pause

#elif UNITY_IOS
            Debug.Log("iOS");
            // Touch Controls

#elif UNITY_ANDROID
            Debug.Log("Android");
            // Touch Controls

#elif UNITY_PS4
            Debug.Log("Playstation 3");
            // Controls for blocks and balls X down Triangle up Start pause

#elif UNITY_PS4
            Debug.Log("Playstation 4");
            // Controls for blocks and balls X down Triangle up Start pause

#elif UNITY_XBOX360
            Debug.Log("xbox");
            // Controls for blocks and balls A down Y up Start pause

#elif UNITY_XBOXONE
            Debug.Log("xbox");
            // Controls for blocks and balls A down Y up Start pause

#endif
    }

    private static void LoadAllSprites()
    {
        for (int i = 0; i < spriteGroups.Count; ++i)
        {
            temp = Resources.LoadAll<Sprite>("sprites/" + spriteGroups[i] + "/");
            for (int j = 0; j < temp.Length; ++j)
            {
                sprites[i].Add((temp[j]).name, (Sprite)temp[j]);
            }
        }
    }

    void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameData.dat");

        GameData data = new GameData();
        // Set function
        data.m_highScore = highScore;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Saved File to " + Application.persistentDataPath + "/gameData.dat");
    }

    void Load() {
        if (File.Exists(Application.persistentDataPath + "/gameData.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);
            GameData data = (GameData) bf.Deserialize(file);
            file.Close();
            // get function
            highScore = data.GetHighScore();

            Debug.Log("Loaded File");

        }
    }
}

[Serializable]
class GameData
{
    public bool m_hasNeverBeenRun;
    public int m_highScore;
    public string m_primaryColor;
    public string m_secondaryColor;

    public GameData() {
        m_hasNeverBeenRun = true;
        m_highScore = 0;
        m_primaryColor = "white";
        m_secondaryColor = "black";
    }

    public GameData(bool hasNeverBeenRun, int highScore, string primaryColor, string secondaryColor) {
        m_hasNeverBeenRun = hasNeverBeenRun;
        m_highScore = highScore;
        m_primaryColor = primaryColor;
        m_secondaryColor = secondaryColor;
    }

    public bool GetHasNeverBeenRun() {
        return m_hasNeverBeenRun;
    }

    public int GetHighScore() {
        return m_highScore;
    }

    public string GetPrimaryColor() {
        return m_primaryColor;
    }

    public string GetSecondaryColor() {
        return m_secondaryColor;
    }

    public void SetHasNeverBeenRun(bool hasNeverBeenRun) {
        m_hasNeverBeenRun = hasNeverBeenRun;
    }

    public void SetHighScore(int highScore) {
        m_highScore = highScore;
    }

    public void SetPrimaryColor(string primaryColor)
    {
        m_primaryColor = primaryColor;
    }

    public void SetSecondaryColor(string secondaryColor) {
        m_secondaryColor = secondaryColor;
    }
}
