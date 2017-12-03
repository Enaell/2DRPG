 using UnityEngine;
using System;
using System.Collections.Generic;



public class LevelManager : MonoBehaviour
{
    // Using Serializable allows us to embed a class with sub properties in the inspector.

    private int col = 12;
    private int row = 10;


    //public char[][] levelMatrix = new char[4][]
    //{
    //    new char[4] { 'w', 'f', 'f', 'f'},
    //    new char[4] { 'w', 'f', 'f', 'f'},
    //    new char[4] { 'w', 'f', 'f', 'f'},
    //    new char[4] { 'w', 'f', 'f', ' '},
    //};

    public char[][] levelMatrixReadable = new char[12][]
    {               //  z0   z1
        new char[10] { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' }, // x0
        new char[10] { 'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w' }, // x1
        new char[10] { 'w', 'f', 'w', 'w', 'w', 'w', 'w', 'w', 'f', 'w' },
        new char[10] { 'w', 'f', 'w', ' ', ' ', ' ', ' ', 'w', 'f', 'w' },
        new char[10] { 'w', 'f', 'w', ' ', ' ', ' ', ' ', 'w', 'f', 'w' },
        new char[10] { 'w', 'f', 'w', 'w', 'w', 'w', 'w', 'w', 'f', 'w' },
        new char[10] { 'w', 'f', 'w', 'f', 'f', 'f', 'f', 'f', 'f', 'w' },
        new char[10] { 'w', 'f', 'w', 'f', 'w', 'w', 'w', 'w', 'f', 'w' },
        new char[10] { 'w', 'f', 'w', 'f', 'w', 'w', 'w', 'w', 'f', 'w' },
        new char[10] { 'w', 'f', 'w', 'f', 'w', 'w', 'w', 'w', 'f', 'w' },
        new char[10] { 'w', 'f', 'f', 'f', 'w', 'w', 'w', 'w', 's', 'w' },
        new char[10] { 'w', 'w', 'w', 'w', 'w', ' ', ' ', 'w', ' ', 'w' },
    };
    public char[][] levelMatrix;
    //Lower and upper limit for our random number of food items per level.
    public GameObject[] floorTiles;                                 //Array of floor prefabs.
    public GameObject[] wallTiles;                                  //Array of wall prefabs.

    public Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.

    public Transform floors;
    public Transform walls;

    //private bool[][] gridEmptyPositions = new bool[4][]
    //{
    //    new bool[4] { true, true, true, true},
    //    new bool[4] { true, true, true, true},
    //    new bool[4] { true, true, true, true},
    //    new bool[4] { true, true, true, true},
    //};
    private bool[][] gridEmptyPositionsReadable = new bool[12][]
    {
        new bool[10] { true, true, true, true, true, true, true, true, true, true },
        new bool[10] { true, true, true, true, true, true, true, true, true, true },
        new bool[10] { true, true, true, true, true, true, true, true, true, true },
        new bool[10] { true, true, true, true, true, true, true, true, true, true },
        new bool[10] { true, true, true, true, true, true, true, true, true, true },
        new bool[10] { true, true, true, true, true, true, true, true, true, true },
        new bool[10] { true, true, true, true, true, true, true, true, true, true },
        new bool[10] { true, true, true, true, true, true, true, true, true, true },
        new bool[10] { true, true, true, true, true, true, true, true, true, true },
        new bool[10] { true, true, true, true, true, true, true, true, true, true },
        new bool[10] { true, true, true, true, true, true, true, true, true, true },
        new bool[10] { true, true, true, true, true, true, true, true, true, true },
    };

    private bool[][] gridEmptyPositions;


    //Clears our list gridPositions and prepares it to generate a new board.
    void InitialiseList()
    {
        //Clear our list gridPositions.
        for (int i = 0; i < row; ++i)
            for (int y = 0; y < col; ++y)
                gridEmptyPositions[i][y] = true;

    }


    //Sets up the outer walls and floor (background) of the game board.
    void BoardSetup()
    {
        levelMatrix = new char[row][];
        for (int x = 0; x < row; ++x)
            levelMatrix[x] = new char[col];

        for (int x = 0; x < row; ++x)
        {
            for (int z = 0; z < col; ++z)
            {
                levelMatrix[x][z] = levelMatrixReadable[z][x];
            }
        }

        gridEmptyPositions = new bool[row][];
        for (int x = 0; x < row; ++x)
            gridEmptyPositions[x] = new bool[col];

        for (int x = 0; x < row; ++x)
        {
            for (int z = 0; z < col; ++z)
            {
                gridEmptyPositions[x][z] = gridEmptyPositionsReadable[z][x];
            }
        }

        //Instantiate Board and set boardHolder to its transform.
        boardHolder = new GameObject("Board").transform;
        floors = new GameObject("Floors").transform;
        walls = new GameObject("Walls").transform;

        //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy
        floors.SetParent(boardHolder);
        walls.SetParent(boardHolder);

        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (int x = 0; x < row; ++x)
        {
            //Loop along z axis, starting from -1 to place floor or outerwall tiles.
            for (int z = 0; z < col; ++z)
            {
                if (levelMatrix[x][z] == ' ' || gridEmptyPositions[x][z] == false)
                    continue;

                GameObject toInstantiate;
                GameObject instance;

                if (levelMatrix[x][z] == 'f' || levelMatrix[x][z] == 's')
                {
                    toInstantiate = floorTiles[0];
                    instance = Instantiate(toInstantiate, new Vector3(x, 0f, -z), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(floors);
                }
                else // levelMatrix[x][y] == w
                {
                    if (z + 3 < col && levelMatrix[x][z + 1] == 'w' && levelMatrix[x][z + 2] == 'w' && levelMatrix[x][z + 3] == 'w') // wall size 4
                    {
                        toInstantiate = wallTiles[0];
                        instance = Instantiate(toInstantiate, new Vector3(x, 0f, -z), Quaternion.Euler(0, 90, 0)) as GameObject;
                        for(int a = z; a < z + 4; ++a)
                           gridEmptyPositions[x][a] = false;
                        instance.transform.SetParent(walls);

                    }
                    else if (x + 3 < row && levelMatrix[x + 1][z] == 'w' && levelMatrix[x + 2][z] == 'w' && levelMatrix[x + 3][z] == 'w') // wall size 4
                    {
                        toInstantiate = wallTiles[0];
                        instance = Instantiate(toInstantiate, new Vector3(x, 0f, -z), Quaternion.identity) as GameObject;
                        for (int a = x; a < x + 4; ++a)
                            gridEmptyPositions[a][z] = false;
                        instance.transform.SetParent(walls);
                    }
                    else // wall size 1
                    {
                        toInstantiate = wallTiles[0];
                        instance = Instantiate(toInstantiate, new Vector3(x, 0f, -z), Quaternion.identity) as GameObject;
                        gridEmptyPositions[x][z] = false;
                        instance.transform.SetParent(walls);
                    }
                }
                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.

            }
        }
    }


    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene()
    {
        //Creates the outer walls and floor.
        BoardSetup();

        //Reset our list of gridpositions.
        InitialiseList();
    }
}