using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {

    private Vector3 topSpawnLocation;
    private Vector3 bottomSpawnLocation;
    private Collider2D blockPrimary2DCollider;
    private Collider2D blockSecondary2DCollider;
    private Collider2D ballPrimary2DCollider;
    private Collider2D ballSecondary2DCollider;
    public GameObject blockPrimary;
    public GameObject blockSecondary;
    public static string blockPrimaryName = "blockWhite";
    public static string blockSecondaryName = "blockBlack";

    // Use this for initialization
    void Start()
    {
        var block2DCollider = blockPrimary.GetComponent<Collider2D>();
        topSpawnLocation = new Vector3(Screen.width / 2 + block2DCollider.bounds.size.x / 2, (Screen.height / 2) - (block2DCollider.bounds.size.y / 2) * 4, 10);
        bottomSpawnLocation = new Vector3(-Screen.width / 2 - block2DCollider.bounds.size.x / 2, -(Screen.height / 2) + (block2DCollider.bounds.size.y / 2) * 4, 10);
        // Load block 1 and block 2 from saved file
    }

	// Update is called once per frame
	void Update () {
        Debug.Log("blockBlack");
	}

    void SpawnBlockTop(GameObject block) {
        Instantiate(block, topSpawnLocation, Quaternion.identity);
    }

    void SpawnBlockBottom(GameObject block) {
        Instantiate(block, bottomSpawnLocation, Quaternion.identity);
    }

}
