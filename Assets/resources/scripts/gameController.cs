// Developers: Dr4g0nbyt3
// Date: December 1st, 2015
// Email: dr4g0nbyt3@gmail.com

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[RequireComponent(typeof(Camera))]
public class gameController : MonoBehaviour
{

    // Public constants
    public const float NATIVE_WIDTH = 900;
    public const float NATIVE_HEIGHT = 1600;

    // Public New Variables
    public new Camera camera;

    // Public Static Variables
    public static gameController controller;
    public static bool hasNeverBeenRun;
    public static bool hasNeverHadTutorial;

    // Public Variables
    public bool isSelectingPrimary;
    public bool isSelectingSecondary;
    public float fadeDuration = 1.0f;
    public float fadEnd;
    public float timer;
    public GameObject background;
    public GameObject ballPrimary;
    public GameObject ballSecondary;
    public GameObject blockPrimary;
    public GameObject blockSecondary;
    public GameObject dr4g0nbyt3Logo;
    public GameObject flashingLights;
    public GUISkin guiSkin;
    public int highScore;
    public int score;
    public int ballsLeft;
    public int blockSpeed;
    public int ballSpeed;
    public string primaryColor = "white";
    public string secondaryColor = "black";
    public string skin = "customLight";

    // Private Variables
    private Vector3 topSpawnLocation;
    private Vector3 bottomSpawnLocation;
    private Vector3 ballSpawnLocation;

    // These will go
    public Color white = Color.white;
    public Color black = Color.black;

    // Clear this 
    private List<List<GameObject>> blockLanes = new List<List<GameObject>> { new List<GameObject>(), new List<GameObject>() };

    // Do not clear these
    private static List<string> spriteGroups = new List<string> { "balls", "blocks", "dr4g0nbyt3" };
    private static List<Dictionary<string, Sprite>> sprites = new List<Dictionary<string, Sprite>> { new Dictionary<string, Sprite>(), new Dictionary<string, Sprite>(), new Dictionary<string, Sprite>() };
    private static Dictionary<string, GUISkin> guiSkins = new Dictionary<string, GUISkin>();
    private static UnityEngine.Object[] temp;

    void Awake()
    {

    }

    void Start()
    { 
        Load();
        SceneController();
        ballsLeft = 3;

    }

    void FixedUpdate()
    {

    }

    void Update()
    {
        

    }

    void OnGUI()
    {
        // Scaling
        float rx = Screen.width / NATIVE_WIDTH;
        float ry = Screen.height / NATIVE_HEIGHT;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));
        GUI.skin = guiSkin;

        if (Application.loadedLevelName == "dr4g0nbyt3Intro")
        {
            if (sprites[0].Count != 0)
            {

                Texture tex = (Texture2D)Resources.Load("sprites/dr4g0nbyt3/icon");
                GUI.Box(new Rect(225, 575, 450, 450), tex, "box");
                GUI.Label(new Rect(0, 900, 900, 256), "DR4G0NBYT3");

            }
        }
        else if (Application.loadedLevelName == "pantlessGrandpaIntro")
        {
            if (sprites[0].Count != 0)
            {
                Texture tex = (Texture2D)Resources.Load("sprites/p4ntsl3ssgr4ndp4/icon");
                GUI.Box(new Rect(225, 575, 450, 450), tex, "box");
                GUI.Label(new Rect(0, 900, 900, 256), "PANTLESS");
                GUI.Label(new Rect(0, 1000, 900, 256), "GRANDPA");
            }
        }
        else if (Application.loadedLevelName == "game")
        {
            GUI.Label(new Rect(0, 900, 900, 256), ballsLeft.ToString());
            GUI.Label(new Rect(0, 700, 900, 256), score.ToString());
            
        }
        else if (Application.loadedLevelName == "menu")
        {

            // Main Menu Logo
            guiSkin.label.fontSize = 128;
            guiSkin.label.fontStyle = FontStyle.Bold;
            GUI.Label(new Rect(0, 64, 900, 256), "BLOCKS");
            guiSkin.label.fontSize = 72;
            GUI.Label(new Rect(0,288,900,256 ), "AND");
            guiSkin.label.fontSize = 128;
            GUI.Label(new Rect(0, 512, 900, 256), " BALLS ");
            guiSkin.label.fontSize = 72;
            guiSkin.label.fontStyle = FontStyle.Normal;

            // Main Menu Buttons
            if (GUI.Button(new Rect(225, 800, 450, 128), ""))
            {
                LoadGameScene();
                StartGame();
            }
            GUI.Label(new Rect(0, 800, 900, 128), "PLAY");
            if (GUI.Button(new Rect(150, 960, 600, 128), ""))
            {
                LoadOptionsScene();
            }
            GUI.Label(new Rect(0, 960, 900, 128), "UNBLOCKABLES");
            if (GUI.Button(new Rect(150, 1120, 600, 128), ""))
            {
                LoadOptionsScene();
            }
            GUI.Label(new Rect(0, 1120, 900, 128), "OPTIONS");
            if (GUI.Button(new Rect(250, 1280, 400, 128), ""))
            {
                LoadCreditsScene();
            }
            GUI.Label(new Rect(0, 1280, 900, 128), "CREDITS");
            if (GUI.Button(new Rect(300, 1440, 300, 128), ""))
            {
                QuitGame();
            }
            GUI.Label(new Rect(0, 1440, 900, 128), "QUIT");
        }
        else if (Application.loadedLevelName == "options")
        {
            GUI.Box(new Rect(0, 0, 900, 256), "UNBLOCKABLES", "unblockablesHeader");
            if (GUI.Button(new Rect(32, 60, 128, 128), "", "backButton"))
            {
                isSelectingPrimary = false;
                isSelectingSecondary = false;
                Save();
                LoadMenuScene();
            }
            if (GUI.Button(new Rect(180, 210, 180, 180), "MAIN", "unblockablesHeader"))
            {
                isSelectingPrimary = true;
                isSelectingSecondary = false;
            }
            if (GUI.Button(new Rect(540, 210, 180, 180), "BOOM", "unblockablesHeader"))
            {
                isSelectingSecondary = true;
                isSelectingPrimary = false;
            }
            GUI.Box(new Rect(100, 332, 700, 256), "YELLOW", "unblockablesHeader");
            if (GUI.Button(new Rect(100, 500, 700, 256), "", "unblockableSection1"))
            {
                if (isSelectingPrimary)
                {
                    primaryColor = "yellow";
                    Debug.Log("Primary Color should be yellow now.");
                }
                if (isSelectingSecondary)
                {
                    secondaryColor = "yellow";
                    Debug.Log("Secondary Color should be yellow now.");
                }
                Save();
            }
            GUI.Box(new Rect(100, 732, 700, 256), "ORANGE", "unblockablesHeader");
            if (GUI.Button(new Rect(100, 900, 700, 256), "", "unblockableSection1"))
            {
                if (isSelectingPrimary)
                {
                    primaryColor = "orange";
                    Debug.Log("Primary Color should be orange now");
                }
                else if (isSelectingSecondary)
                {
                    secondaryColor = "orange";
                    Debug.Log("Secondary Color should be orange now");
                }
                Save();
            }
            GUI.Box(new Rect(100, 1132, 700, 256), "RED", "unblockablesHeader");
            if (GUI.Button(new Rect(100, 1300, 700, 256), "", "unblockableSection1"))
            {
                if (isSelectingPrimary)
                {
                    primaryColor = "red";
                    Debug.Log("Primary Color should be red now");
                }
                else if (isSelectingSecondary)
                {
                    secondaryColor = "red";
                    Debug.Log("Secondary Color should be red now");
                }
                else if (!isSelectingPrimary && !isSelectingSecondary) {
                    Debug.Log("Abort missions both negative should be happening if no choice selected for main or secondary");
                }
                
                Save();
            }

        }
        else if (Application.loadedLevelName == "credits")
        {
            Debug.Log("On GUI just called if Credits");
            if (GUI.Button(new Rect(64, 64, 128, 128), "", "backButton"))
            {
                LoadMenuScene();
            }
        }
        else if (Application.loadedLevelName == "gameOver")
        {
            Debug.Log("On GUI just called if gameOver");


            // Main Menu Buttons
            if (GUI.Button(new Rect(225, 800, 450, 128), ""))
            {
                LoadGameScene();
                StartGame();
            }
            GUI.Label(new Rect(0, 800, 900, 128), "PLAY");
            if (GUI.Button(new Rect(150, 960, 600, 128), ""))
            {
                LoadOptionsScene();
            }
            GUI.Label(new Rect(0, 960, 900, 128), "UNBLOCKABLES");
            if (GUI.Button(new Rect(150, 1120, 600, 128), ""))
            {
                LoadOptionsScene();
            }
            GUI.Label(new Rect(0, 1120, 900, 128), "OPTIONS");
            if (GUI.Button(new Rect(250, 1280, 400, 128), ""))
            {
                LoadCreditsScene();
            }
            GUI.Label(new Rect(0, 1280, 900, 128), "CREDITS");
            if (GUI.Button(new Rect(300, 1440, 300, 128), ""))
            {
                QuitGame();
            }
            GUI.Label(new Rect(0, 1440, 900, 128), "QUIT");

        }
        else
        {
            LoadMenuScene();
        }

    }

    public void LoadPantlessGrandpaIntroScene()
    {
        Application.LoadLevel("pantlessGrandpaIntro");
    }

    public void LoadGameScene()
    {
        Application.LoadLevel("game");

    }

    public void LoadMenuScene()
    {
        Application.LoadLevel("menu");

    }

    public void LoadOptionsScene()
    {
        Application.LoadLevel("options");

    }

    public void LoadGameOverScene()
    {
        Application.LoadLevel("g");

    }

    public void LoadCreditsScene()
    {
        Application.LoadLevel("credits");

    }

    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    void GameControllerRules()
    {
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

    void SceneController()
    {
        GameControllerRules();
        camera = GetComponent<Camera>();
        LoadAllSprites();
        LoadAllGuiSkins();
        if (Application.loadedLevelName == "dr4g0nbyt3Intro")
        {
            EstablishControls();

            camera.backgroundColor = new Color32(32, 32, 32, 32);

            Load();
            Invoke("LoadPantlessGrandpaIntroScene", 3.0f);
            Invoke("LoadMenuScene", 6.0f);
            Invoke("LoadGameOverScene", 9.0f);
        }
        if (Application.loadedLevelName == "pantlessGrandpaIntro")
        {

        }
        if (Application.loadedLevelName == "menu")
        {

        }
        if (Application.loadedLevelName == "options")
        {

        }
        if (Application.loadedLevelName == "game")
        {
            Invoke("LoadGameOver", 5.0f);
        }
        if (Application.loadedLevelName == "gameOver")
        {


        }
    }

    void StartGame()
    {

        //camera.backgroundColor = Color.gray;
        var block2DCollider = blockPrimary.GetComponent<Collider2D>();

        topSpawnLocation = new Vector3(Screen.width + Screen.width, Screen.height - Screen.height / 32, 11);
        topSpawnLocation = camera.ScreenToWorldPoint(topSpawnLocation);
        bottomSpawnLocation = new Vector3(-Screen.width, Screen.height / 32, 11);
        bottomSpawnLocation = camera.ScreenToWorldPoint(bottomSpawnLocation);
        bottomSpawnLocation.x += block2DCollider.bounds.size.x * 2;
        bottomSpawnLocation.y -= block2DCollider.bounds.size.y * 2;
        ballSpawnLocation = new Vector3(450, 800, 11);
        // Set Block one and two from saved files
        Debug.Log(primaryColor);
        blockPrimary.GetComponent<SpriteRenderer>().sprite = sprites[1][primaryColor];
        blockSecondary.GetComponent<SpriteRenderer>().sprite = sprites[1][secondaryColor];
        ballPrimary.GetComponent<SpriteRenderer>().sprite = sprites[0][primaryColor];
        ballSecondary.GetComponent<SpriteRenderer>().sprite = sprites[0][secondaryColor];

        // Load block 1 and block 2 from saved file
        // if game scene
        InvokeRepeating("SpawnBlocks", 1f, 0.3f);
        InvokeRepeating("SwapPrimaryAndSecondary", 10.0f, 10.0f);
        InvokeRepeating("SpawnFlashingLights", 7.5f, 10.0f);
        SpawnBall();
    }

    void SpawnFlashingLights()
    {
        Instantiate(flashingLights,new Vector3(0,0,11), Quaternion.identity);
    }

    void SpawnBall()
    {
        Instantiate(ballPrimary, ballSpawnLocation, Quaternion.identity);
    }

    void SpawnBlocks()
    {
        int topRandom = UnityEngine.Random.Range(0, 10);
        int bottomRandom = UnityEngine.Random.Range(0, 10);
        GameObject topSpawn;
        GameObject bottomSpawn;
        if (topRandom < 4)
        {
            topSpawn = blockSecondary;
        }
        else
        {
            topSpawn = blockPrimary;
        }
        if (bottomRandom < 4)
        {
            bottomSpawn = blockSecondary;
        }
        else
        {
            bottomSpawn = blockPrimary;
        }
        blockLanes[0].Add((GameObject)Instantiate(topSpawn, topSpawnLocation, Quaternion.identity));
        blockLanes[0][blockLanes[0].Count - 1].GetComponent<Rigidbody2D>().AddForce(new Vector2(-3 * Time.deltaTime, 0));
        blockLanes[1].Add((GameObject)Instantiate(bottomSpawn, bottomSpawnLocation, Quaternion.identity));
        blockLanes[1][blockLanes[1].Count - 1].GetComponent<Rigidbody2D>().AddForce(new Vector2(3 * Time.deltaTime, 0));
    }

    public void SwapPrimaryAndSecondary()
    {
        var temp = blockPrimary.GetComponent<SpriteRenderer>().sprite;
        blockPrimary.GetComponent<SpriteRenderer>().sprite = blockSecondary.GetComponent<SpriteRenderer>().sprite;
        blockSecondary.GetComponent<SpriteRenderer>().sprite = temp;
        temp = ballPrimary.GetComponent<SpriteRenderer>().sprite;
        ballPrimary.GetComponent<SpriteRenderer>().sprite = ballSecondary.GetComponent<SpriteRenderer>().sprite;
        ballSecondary.GetComponent<SpriteRenderer>().sprite = temp;
    }

    void EstablishControls()
    {
        if (hasNeverBeenRun)
        {
            if (Application.isMobilePlatform)
            {
                // Enable touch controls
            }

            else if (Application.isConsolePlatform)
            {
                // Enable controler controls
            }
            else if (Application.isEditor)
            {
                Debug.Log("Houston we appear to be working");
            }
        }
    }

    private static void LoadAllSprites()
    {
        for (int i = 0; i < spriteGroups.Count; ++i)
        {
            if (sprites[i].Count == 0)
            {
                temp = Resources.LoadAll<Sprite>("sprites/" + spriteGroups[i] + "/");
                for (int j = 0; j < temp.Length; ++j)
                {
                    sprites[i].Add((temp[j]).name, (Sprite)temp[j]);
                }
            }
        }
    }


    private static void LoadAllGuiSkins()
    {
        if (guiSkins.Count == 0)
        {
            temp = Resources.LoadAll<GUISkin>("skins/");
            for (int j = 0; j < temp.Length; ++j)
            {
                guiSkins.Add((temp[j]).name, (GUISkin)temp[j]);
            }
        }

    }

    void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);

        GameData data = new GameData(hasNeverBeenRun, hasNeverHadTutorial, highScore, primaryColor, secondaryColor, skin);

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Saved File to " + Application.persistentDataPath + "/gameData.dat");
    }

    void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/gameData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();
            hasNeverBeenRun = data.m_hasNeverBeenRun;
            //guiSkin = guiSkins[data.m_skin];
            skin = data.m_skin;
            highScore = data.m_highScore;
            primaryColor = data.m_primaryColor;
            secondaryColor = data.m_secondaryColor;
            Debug.Log(primaryColor);
            Debug.Log(secondaryColor);

            Debug.Log("Loaded Save File");
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gameData.dat");
            GameData data = new GameData();
            bf.Serialize(file, data);
            file.Close();
            Debug.Log("Loaded New Save file");
        }
    }
}

[Serializable]
class GameData
{

    public bool m_hasNeverBeenRun;
    public bool m_hasNeverHadTutorial;
    public int m_highScore;
    public string m_primaryColor;
    public string m_secondaryColor;
    public string m_skin;


    public GameData()
    {
        m_hasNeverBeenRun = false;
        m_hasNeverHadTutorial = true;
        m_highScore = 0;
        m_primaryColor = "white";
        m_secondaryColor = "black";
        m_skin = "customLight";
    }

    public GameData(bool hasNeverBeenRun, bool hasNeverHadTutorial, int highScore, string primaryColor, string secondaryColor, string skin)
    {
        m_hasNeverBeenRun = hasNeverBeenRun;
        m_hasNeverHadTutorial = hasNeverHadTutorial;
        m_highScore = highScore;
        m_primaryColor = primaryColor;
        m_secondaryColor = secondaryColor;
        m_skin = skin;
    }
}
