using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {

    //Script that controls the player sprite movement and animation control
    private WitchMove witchMove;

    //Keeps track of the direction our player is oriented
    public FacedDirection facedDirection;

    //Access to the player gameObject, useful for getting spacial coordinates
    public GameObject Player;

    //Used to tell the FezMove script how much to rotate 90 or -90 degrees depending on input
    private float degree = 0;

    //Access to the Transform containing our Level Data - Platforms we can walk on
    public Transform Level;

    //Access to the Transform containing our Building Data - There for asthetics but we don't plan to move on it
    public Transform Building;

    public Animator anim;


    //Dimensions of cubes used - so far only tested with 1. This could potentially be updated if cubes of a different
    //size are needed - Note: All floor cubes must be same size
    public float WorldUnits = 1.000f;


    public static WorldManager instance = null;   
    
    //Static instance of GameManager which allows it to be accessed by any other script.
    private LevelManager boardScript;                       //Store a reference to our BoardManager which will set up the level.


    //Awake is always called before any Start functions
    void Awake()
    {
        //singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);


        //Sets this to not be destroyed when reloading scene
       // DontDestroyOnLoad(gameObject);

        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<LevelManager>();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    void InitGame()
    {
        //Call the SetupScene function of the BoardManager script, pass it current level number.
        boardScript.SetupScene();
        Level = boardScript.floors;
        Building = boardScript.walls;
    }


    // Use this for initialization
    void Start()
    {
        //Define our faced direction, must be the same as built in inspector
        //Cache our fezMove script located on the player and update our level data (create invisible cubes)
        facedDirection = FacedDirection.Front;
        witchMove = Player.GetComponent<WitchMove>();
    }

    //// Update is called once per frame
    void Update()
    {
        FacedDirection tryRotateDirection;


        //Handle Player input 
        // DONT PUT FLOOR UNDER A WALL
        if ((Input.GetKeyDown(KeyCode.W) && witchMove.GetHorizontal() == -1) || (Input.GetKeyDown(KeyCode.S) && witchMove.GetHorizontal() == 1))
        {
            tryRotateDirection = RotateDirectionLeft();
            if (witchMove.UpdateToFacedDirection(tryRotateDirection, degree + 90f, Level, WorldUnits))
            {
                facedDirection = tryRotateDirection;
                degree += 90f;
            }
        }
        else if ((Input.GetKeyDown(KeyCode.W) && witchMove.GetHorizontal() == 1) || (Input.GetKeyDown(KeyCode.S) && witchMove.GetHorizontal() == -1))
        {
            tryRotateDirection = RotateDirectionRight();
            if (witchMove.UpdateToFacedDirection(tryRotateDirection, degree - 90f, Level, WorldUnits))
            {
                facedDirection = tryRotateDirection;
                degree -= 90f;
            }
        }
        seeThroughBuilding(Player.transform.position);

    }

    // Update is called once per frame
    //void Update()
    //{
    //    FacedDirection tryRotateDirection;


    //    //Handle Player input 
    //    // DONT PUT FLOOR UNDER A WALL
    //    if ((Input.GetKeyDown(KeyCode.W) && witchMove.GetHorizontal() == -1) || (Input.GetKeyDown(KeyCode.S) && witchMove.GetHorizontal() == 1))
    //    {
    //        tryRotateDirection = RotateDirectionLeft();
    //        if (witchMove.UpdateToFacedDirection(tryRotateDirection, degree + 90f, boardScript.levelMatrix, WorldUnits))
    //        {
    //            facedDirection = tryRotateDirection;
    //            degree += 90f;
    //        }
    //    }
    //    else if ((Input.GetKeyDown(KeyCode.W) && witchMove.GetHorizontal() == 1) || (Input.GetKeyDown(KeyCode.S) && witchMove.GetHorizontal() == -1))
    //    {
    //        tryRotateDirection = RotateDirectionRight();
    //        if (witchMove.UpdateToFacedDirection(tryRotateDirection, degree - 90f, boardScript.levelMatrix, WorldUnits))
    //        {
    //            facedDirection = tryRotateDirection;
    //            degree -= 90f;
    //        }
    //    }
    //    seeThroughBuilding(Player.transform.position);

    //}


    private void seeThroughBuilding(Vector3 cube)
    {
        foreach (Transform item in Building)
        {
            if (((facedDirection == FacedDirection.Front && item.position.z < cube.z)
                || (facedDirection == FacedDirection.Back && item.position.z > cube.z)
                || (facedDirection == FacedDirection.Right && item.position.x > cube.x)
                || (facedDirection == FacedDirection.Left && item.position.x < cube.x)))
            {
                item.gameObject.SetActive(false);
                continue;
            }
            else
                item.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Looks for a physical (visible) cube in our level data at position 'cube'
    /// </summary>
    /// <returns><c>true</c>, if transform level was found, <c>false</c> otherwise.</returns>
    /// <param name="cube">Cube.
    private bool FindTransformLevel(Vector3 cube)
    {
        foreach (Transform item in Level)
        {
            if (item.position == cube)
                return true;

        }
        return false;

    }

    /// <summary>
    /// Determines the faced direction after we rotate to the right
    /// </summary>
    /// <returns>The direction right.</returns>
    private FacedDirection RotateDirectionRight()
    {
        int change = (int)(facedDirection);
        change++;
        //Our FacedDirection enum only has 4 states, if we go past the last state, loop to the first
        if (change > 3)
            change = 0;
        return (FacedDirection)(change);
    }
    /// <summary>
    /// Determines the faced direction after we rotate to the left
    /// </summary>
    /// <returns>The direction left.</returns>
    private FacedDirection RotateDirectionLeft()
    {
        int change = (int)(facedDirection);
        change--;
        //Our FacedDirection enum only has 4 states, if we go below the first, go to the last state
        if (change < 0)
            change = 3;
        return (FacedDirection)(change);
    }
}
//Used frequently to keep track of the orientation of our player and camera
public enum FacedDirection
{
    Front = 0,
    Right = 1,
    Back = 2,
    Left = 3

}