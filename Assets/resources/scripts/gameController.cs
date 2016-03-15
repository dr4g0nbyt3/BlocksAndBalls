// Developers: Dr4g0nbyt3
// Date: December 1st, 2015
// Github: dr4g0nbyt3
// Website: dr4g0nbyt3.github.io
// Email: dr4g0nbyt3@gmail.com
// Twitter: @dr4g0nbyt3
// Youtube: dr4g0nbyt3

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
    public bool hasNewUnlocks;
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
    public int unblockablesPage;
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
    private List<GameObject> balls = new List<GameObject>();

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
        unblockablesPage = 1; 
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

        // Scene styling
        // Gui for dr4g0nbyt3 intro scene
        if (Application.loadedLevelName == "dr4g0nbyt3Intro")
        {
            if (sprites[0].Count != 0)
            {

                Texture tex = (Texture2D)Resources.Load("sprites/dr4g0nbyt3/icon");
                GUI.Box(new Rect(225, 575, 450, 450), tex, "box");
                GUI.Label(new Rect(0, 900, 900, 256), "DR4G0NBYT3");

            }
        }

        // Gui for pantlessgrandpa scene.
        else if (Application.loadedLevelName == "pantlessGrandpaIntro")
        {
            if (sprites[0].Count != 0)
            {
                Texture tex = (Texture2D)Resources.Load("sprites/p4ntsl3ssgr4ndp4/icon");
                GUI.Box(new Rect(225, 575, 450, 450), tex, "box");
                GUI.Label(new Rect(0, 900, 900, 256), "P4NTL3SS");
                GUI.Label(new Rect(0, 1000, 900, 256), "GR4NDP4");
            }
        }

        // Gui for game scene.
        else if (Application.loadedLevelName == "game")
        {
            GUI.Label(new Rect(0, 800, 450, 128), ballsLeft.ToString());
            GUI.Label(new Rect(450, 800, 450, 128), score.ToString());
            
        }

        // Gui for main menu scene.
        else if (Application.loadedLevelName == "menu")
        {

            // Main Menu Logo
            guiSkin.label.fontSize = 128;
            guiSkin.label.fontStyle = FontStyle.Bold;
            // Change color of font on main menu
            //guiSkin.label.normal.textColor
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
                LoadUnblockablesScene();
            }
            if (hasNewUnlocks)
            {
                GUI.Label(new Rect(0, 960, 900, 128), "!UNBLOCKABLES!");
            }
            else
            {
                GUI.Label(new Rect(0, 960, 900, 128), "UNBLOCKABLES");
            }
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

        // Gui for unblockables scene.
        else if (Application.loadedLevelName == "unblockables")
        {
            
            // Unblockable header formatting.
            GUI.Box(new Rect(0, 0, 900, 192), "UNBLOCKABLES", "unblockablesHeader");
            if (GUI.Button(new Rect(0, 32, 128, 128), "", "backButton"))
            {
                isSelectingPrimary = false;
                isSelectingSecondary = false;
                Save();
                LoadMenuScene();
            }
            if (GUI.Button(new Rect(0, 192, 450, 128), "", "unblockableSection0"))
            {
                isSelectingPrimary = true;
                isSelectingSecondary = false;
            }
            GUI.Label(new Rect(0, 192, 450, 128), "MAIN", "unblockableSection0");
            if (GUI.Button(new Rect(450, 192, 450, 128), "", "unblockableSection0"))
            {
                isSelectingSecondary = true;
                isSelectingPrimary = false;
            }
            GUI.Label(new Rect(450, 192, 450, 128), "BOOM", "unblockableSection0");

            GUI.Label(new Rect(0, 200, 900, 128), unblockablesPage.ToString(), "label");

            if (GUI.Button(new Rect(0, 800, 128, 128), "", "backButton"))
            {
                unblockablesPage--;
            }
            if (GUI.Button(new Rect(800, 800, 128, 128), "", "backButton"))
            {
                unblockablesPage++;
            }

            // Selections for page 0.
            if (unblockablesPage == 0)
            {
                // Place hackers theme here.
            }

            // Selections for page 1.
            if (unblockablesPage == 1)
            {

                // Unblock selection for Yellow.
                if (GUI.Button(new Rect(100, 458, 700, 256), "", "unblockableSection0"))
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
                GUI.Label(new Rect(100, 330, 700, 128), "YELLOW", "unblockablesHeader");
                Texture tmpTex = (Texture2D)Resources.Load("sprites/blocks/yellow");
                GUI.Box(new Rect(450, 458, 100, 256), tmpTex, "box");

                // Unblock selection for Orange.
                if (GUI.Button(new Rect(100, 860, 700, 256), "", "unblockableSection0"))
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
                GUI.Label(new Rect(100, 732, 700, 128), "ORANGE", "unblockablesHeader");

                // Unblock selection for Red.
                if (GUI.Button(new Rect(100, 1260, 700, 256), "", "unblockableSection0"))
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
                    else if (!isSelectingPrimary && !isSelectingSecondary)
                    {
                        Debug.Log("Abort missions both negative should be happening if no choice selected for main or secondary");
                    }

                    Save();
                }
                GUI.Label(new Rect(100, 1132, 700, 128), "RED", "unblockablesHeader");
            }

            // Selections for page 2.
            else if (unblockablesPage == 2)
            {
                // Unblock selection for Green.
                if (GUI.Button(new Rect(100, 458, 700, 256), "", "unblockableSection0"))
                {
                    if (isSelectingPrimary)
                    {
                        primaryColor = "green";
                        Debug.Log("Primary Color should be green now.");
                    }
                    if (isSelectingSecondary)
                    {
                        secondaryColor = "green";
                        Debug.Log("Secondary Color should be green now.");
                    }
                    Save();
                }
                GUI.Label(new Rect(100, 330, 700, 128), "GREEN", "unblockablesHeader");

                // Unblock selection for Blue.
                if (GUI.Button(new Rect(100, 860, 700, 256), "", "unblockableSection0"))
                {
                    if (isSelectingPrimary)
                    {
                        primaryColor = "blue";
                        Debug.Log("Primary Color should be blue now");
                    }
                    else if (isSelectingSecondary)
                    {
                        secondaryColor = "blue";
                        Debug.Log("Secondary Color should be blue now");
                    }
                    Save();
                }
                GUI.Label(new Rect(100, 732, 700, 128), "BLUE", "unblockablesHeader");

                // Unblock selection for Purple.
                if (GUI.Button(new Rect(100, 1260, 700, 256), "", "unblockableSection0"))
                {
                    if (isSelectingPrimary)
                    {
                        primaryColor = "purple";
                        Debug.Log("Primary Color should be purple now");
                    }
                    else if (isSelectingSecondary)
                    {
                        secondaryColor = "purple";
                        Debug.Log("Secondary Color should be purple now");
                    }
                    else if (!isSelectingPrimary && !isSelectingSecondary)
                    {
                        Debug.Log("Abort missions both negative should be happening if no choice selected for main or secondary");
                    }

                    Save();
                }
                GUI.Label(new Rect(100, 1132, 700, 128), "PURPLE", "unblockablesHeader");
            }

            // Unblock selections for page 3.
            else if (unblockablesPage == 3)
            {
                // Unblock selection for White.
                if (GUI.Button(new Rect(100, 458, 700, 256), "", "unblockableSection0"))
                {
                    if (isSelectingPrimary)
                    {
                        primaryColor = "white";
                        Debug.Log("Primary Color should be white now.");
                    }
                    if (isSelectingSecondary)
                    {
                        secondaryColor = "white";
                        Debug.Log("Secondary Color should be white now.");
                    }
                    Save();
                }
                GUI.Label(new Rect(100, 330, 700, 128), "WHITE", "unblockablesHeader");

                // Unblock selection for Black.
                if (GUI.Button(new Rect(100, 860, 700, 256), "", "unblockableSection0"))
                {
                    if (isSelectingPrimary)
                    {
                        primaryColor = "black";
                        Debug.Log("Primary Color should be black now");
                    }
                    else if (isSelectingSecondary)
                    {
                        secondaryColor = "black";
                        Debug.Log("Secondary Color should be black now");
                    }
                    Save();
                }
                GUI.Label(new Rect(100, 732, 700, 128), "BLACK", "unblockablesHeader");

                }
            }

        // Gui for credits scene
        else if (Application.loadedLevelName == "credits")
        {
            Debug.Log("On GUI just called if Credits");
            if (GUI.Button(new Rect(64, 64, 128, 128), "", "backButton"))
            {
                LoadMenuScene();
            }
        }

        // Gui for game over scene
        else if (Application.loadedLevelName == "gameOver")
        {
            // This needs to go
            Debug.Log("On GUI just called if gameOver");

            // game over buttons
            if (GUI.Button(new Rect(225, 800, 450, 128), ""))
            {
                LoadGameScene();
                StartGame();
            }
            GUI.Label(new Rect(0, 800, 900, 128), "PLAY");
            if (GUI.Button(new Rect(150, 960, 600, 128), ""))
            {
                LoadUnblockablesScene();
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

    // Loads pantslessgrandpa scene.
    public void LoadPantlessGrandpaIntroScene()
    {
        Application.LoadLevel("pantlessGrandpaIntro");
    }

    // Loads game scene.
    public void LoadGameScene()
    {
        Application.LoadLevel("game");
    }

    // Loads menu scene.
    public void LoadMenuScene()
    {
        Application.LoadLevel("menu");
    }

    // Loads options scene.
    public void LoadOptionsScene()
    {
        Application.LoadLevel("options");
    }

    // Loads unblockables scene.
    public void LoadUnblockablesScene()
    {
        Application.LoadLevel("unblockables");
    }

    // Loads game over scene
    // Change this to game over
    public void LoadGameOverScene()
    {
        Application.LoadLevel("g");
    }

    // Loads credits scene
    public void LoadCreditsScene()
    {
        Application.LoadLevel("credits");
    }

    // Exits game or editor
    public void QuitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    // Makes sure there is always a gameController.
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

    // Handles all of my magic.
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

        }
        if (Application.loadedLevelName == "gameOver")
        {


        }
    }

    void StartGame()
    {
        // Handles spawning of block spawn locations based on screen size.
        var block2DCollider = blockPrimary.GetComponent<Collider2D>();
        topSpawnLocation = new Vector3(Screen.width + Screen.width, Screen.height - Screen.height / 32, 11);
        topSpawnLocation = camera.ScreenToWorldPoint(topSpawnLocation);
        bottomSpawnLocation = new Vector3(-Screen.width, Screen.height / 32, 11);
        bottomSpawnLocation = camera.ScreenToWorldPoint(bottomSpawnLocation);
        bottomSpawnLocation.x += block2DCollider.bounds.size.x * 2;
        bottomSpawnLocation.y -= block2DCollider.bounds.size.y * 2;
        ballSpawnLocation = new Vector3(Screen.width/2, Screen.height/2, 11);
        // Set Block one and two from saved files
        Debug.Log(primaryColor);
        blockPrimary.GetComponent<SpriteRenderer>().sprite = sprites[1][primaryColor];
        blockSecondary.GetComponent<SpriteRenderer>().sprite = sprites[1][secondaryColor];


        // Load block 1 and block 2 from saved file
        // if game scene
        SpawnBall();
        InvokeRepeating("SpawnBlocks", 0.1f, 0.5f);
        InvokeRepeating("SwapPrimaryAndSecondary", 10.0f, 10.0f);
        InvokeRepeating("SpawnFlashingLights", 7.5f, 10.0f);
    }

    // Instantiate Kanye reference if need be.
    void SpawnFlashingLights()
    {
        Instantiate(flashingLights,new Vector3(0,0,11), Quaternion.identity);
    }

    // Not yet working... Might be female.
    void SpawnBall()
    {
        //ballPrimary.GetComponent<SpriteRenderer>().sprite = sprites[0][primaryColor];
        //ballSecondary.GetComponent<SpriteRenderer>().sprite = sprites[0][secondaryColor];
        Debug.Log("Spawnball has been called successfully");
        Instantiate(ballPrimary, ballSpawnLocation, Quaternion.identity); 
        balls.Add((GameObject)Instantiate(ballPrimary, ballSpawnLocation, Quaternion.identity));
    }

    // Handles spawning and making primary, secondary, or exclusive blocks.
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
        blockLanes[0][blockLanes[0].Count - 1].GetComponent<Rigidbody2D>().AddForce(new Vector2(-2 * Time.deltaTime, 0));
        blockLanes[1].Add((GameObject)Instantiate(bottomSpawn, bottomSpawnLocation, Quaternion.identity));
        blockLanes[1][blockLanes[1].Count - 1].GetComponent<Rigidbody2D>().AddForce(new Vector2(2 * Time.deltaTime, 0));
    }

    // Name speaks for itself.
    public void SwapPrimaryAndSecondary()
    {
        var temp = blockPrimary.GetComponent<SpriteRenderer>().sprite;
        blockPrimary.GetComponent<SpriteRenderer>().sprite = blockSecondary.GetComponent<SpriteRenderer>().sprite;
        blockSecondary.GetComponent<SpriteRenderer>().sprite = temp;
        temp = ballPrimary.GetComponent<SpriteRenderer>().sprite;
        ballPrimary.GetComponent<SpriteRenderer>().sprite = ballSecondary.GetComponent<SpriteRenderer>().sprite;
        ballSecondary.GetComponent<SpriteRenderer>().sprite = temp;
    }

    // Creates rules based on the platform of the game.
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

    // Guess what this does.
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

    // How about this one?
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

    // The almighty save function
    void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);

        GameData data = new GameData(hasNeverBeenRun, hasNeverHadTutorial, highScore, primaryColor, secondaryColor, skin);

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Saved File to " + Application.persistentDataPath + "/gameData.dat");
    }

    // The messed up load function (only upon new loading)
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
