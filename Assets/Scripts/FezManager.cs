using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Fez manager creates invisible cubes for the player to move on. The world is based in 3D to allow rotation, this means
/// there are varying levels of depth that the player could be at, these may not line up with the physical platforms we create.
/// The player is moving in 2D, so it looks like a platform is present where one may not be, depending on the depth of the platform
/// and the player. If they are different, we will create invisible cubes that the player can move on to fake the player being on
/// a 2D platform. When we have the chance, we will move the player's depth to the closest platform so when they next rotate
/// it will not disorient them.
/// </summary>
public class FezManager : MonoBehaviour
{
    //Script that controls the player sprite movement and animation control
    private FezMove fezMove;

    //Keeps track of the direction our player is oriented
    public FacingDirection facingDirection;

    //Access to the player gameObject, useful for getting spacial coordinates
    public GameObject Player;

    //Used to tell the FezMove script how much to rotate 90 or -90 degrees depending on input
    private float degree = 0;

    //Access to the Transform containing our Level Data - Platforms we can walk on
    public Transform Level;

    //Access to the Transform containing our Building Data - There for asthetics but we don't plan to move on it
    public Transform Building;

    //Dimensions of cubes used - so far only tested with 1. This could potentially be updated if cubes of a different
    //size are needed - Note: All cubes must be same size
    public float WorldUnits = 2.000f;

    // Use this for initialization
    void Start()
    {

        //Define our facing direction, must be the same as built in inspector
        //Cache our fezMove script located on the player and update our level data (create invisible cubes)
        facingDirection = FacingDirection.Front;
        fezMove = Player.GetComponent<FezMove>();
    }

    // Update is called once per frame
    void Update()
    {

        //Logic to control the player depth
        //If we're on an invisible platform, move to a physical platform, this comes in handy to make rotating possible
        //Try to move us to the closest platform to the camera, will help when rotating to feel more natural
        //If we changed anything, update our level data which pertains to our inviscubes
        FacingDirection tryRotateDirection;


        //Handle Player input for rotation command

        // DONT PUT FLOOR UNDER A WALL
        if ((Input.GetKeyDown(KeyCode.W) && Player.GetComponent<SpriteRenderer>().flipX) || (Input.GetKeyDown(KeyCode.S) && !Player.GetComponent<SpriteRenderer>().flipX))
        {
            tryRotateDirection = RotateDirectionLeft();
            if (fezMove.UpdateToFacingDirection(tryRotateDirection, degree + 90f, Level, WorldUnits))
            {
                facingDirection = tryRotateDirection;
                degree += 90f;
            }
        }
        else if ((Input.GetKeyDown(KeyCode.W) && !Player.GetComponent<SpriteRenderer>().flipX) || (Input.GetKeyDown(KeyCode.S) && Player.GetComponent<SpriteRenderer>().flipX))
        {
            tryRotateDirection = RotateDirectionRight();
            if (fezMove.UpdateToFacingDirection(tryRotateDirection, degree-90f, Level, WorldUnits))
            {
                facingDirection = tryRotateDirection;
                degree -= 90f;
            }
        }
        seeThroughBuilding(Player.transform.position);

    }


    private void seeThroughBuilding(Vector3 cube)
    {
        foreach (Transform item in Building)
        {
            if ((facingDirection == FacingDirection.Front && item.position.z < cube.z)
                || (facingDirection == FacingDirection.Back && item.position.z > cube.z)
                || (facingDirection == FacingDirection.Right && item.position.x > cube.x)
                || (facingDirection == FacingDirection.Left && item.position.x < cube.x))
            {
                    item.gameObject.SetActive(false);
                    continue;
            }
            else
                item.gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// Destroy current invisible platforms
    /// Create new invisible platforms taking into account the
    /// player's facing direction and the orthographic view of the 
    /// platforms
    /// </summary>

    /// <summary>
    /// Moves the player to the closest platform with the same height to the camera
    /// Only supports Unity cubes of size (1x1x1)
    /// </summary>



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
    /// Determines if any building cubes are between the "cube"
    /// and the camera
    /// </summary>
    /// <returns><c>true</c>, if transform building was found, <c>false</c> otherwise.</returns>
    /// <param name="cube">Cube.
    private bool FindTransformBuilding(Vector3 cube)
    {
        foreach (Transform item in Building)
        {
            if (facingDirection == FacingDirection.Front)
            {
                if (item.position.x == cube.x && item.position.y == cube.y && item.position.z < cube.z)
                {

                }// return true;
            }
            else if (facingDirection == FacingDirection.Back)
            {
                if (item.position.x == cube.x && item.position.y == cube.y && item.position.z > cube.z)
                    return true;
            }
            else if (facingDirection == FacingDirection.Right)
            {
                if (item.position.z == cube.z && item.position.y == cube.y && item.position.x > cube.x)
                    return true;

            }
            else
            {
                if (item.position.z == cube.z && item.position.y == cube.y && item.position.x < cube.x)
                    return true;

            }
        }
        return false;

    }



    /// <summary>
    /// Returns the player depth. Depth is how far from or close you are to the camera
    /// If we're facing Front or Back, this is Z
    /// If we're facing Right or Left it is X
    /// </summary>
    /// <returns>The player depth.</returns>
    private float GetPlayerDepth()
    {
        float ClosestPoint = 0f;

        if (facingDirection == FacingDirection.Front || facingDirection == FacingDirection.Back)
        {
            ClosestPoint = fezMove.transform.position.z;

        }
        else if (facingDirection == FacingDirection.Right || facingDirection == FacingDirection.Left)
        {
            ClosestPoint = fezMove.transform.position.x;
        }


        return Mathf.Round(ClosestPoint);

    }


    /// <summary>
    /// Determines the facing direction after we rotate to the right
    /// </summary>
    /// <returns>The direction right.</returns>
    private FacingDirection RotateDirectionRight()
    {
        int change = (int)(facingDirection);
        change++;
        //Our FacingDirection enum only has 4 states, if we go past the last state, loop to the first
        if (change > 3)
            change = 0;
        return (FacingDirection)(change);
    }
    /// <summary>
    /// Determines the facing direction after we rotate to the left
    /// </summary>
    /// <returns>The direction left.</returns>
    private FacingDirection RotateDirectionLeft()
    {
        int change = (int)(facingDirection);
        change--;
        //Our FacingDirection enum only has 4 states, if we go below the first, go to the last state
        if (change < 0)
            change = 3;
        return (FacingDirection)(change);
    }

}
//Used frequently to keep track of the orientation of our player and camera
public enum FacingDirection
{
    Front = 0,
    Right = 1,
    Back = 2,
    Left = 3

}