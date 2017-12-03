using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    private List<char[][]> _level;


	// Use this for initialization
	void Start ()
    {
        _level = new List<char[][]>();

        // v = wall size 1
        // w = wall size 4
        // f = floor size 1
        // from 0-0 .. 0-9 .. 1-0 .. 10-9 : if [x,y]=w : if [x,y+1]=w && [x, y+2]=w && [x,y+3]=w : vertical wall size 4
        //                                               else if [x+1,y]=w && [x+1,y]=w && [x+3,y]=w : horizontal wall size 4
        //                                               else : wall size 1
        //                                  else [x,y]=f : floor

        char[][] floor = new char[12][] 
        {
            new char[10] { 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w' },
            new char[10] { 'w', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'w' },
            new char[10] { 'w', 'f', 'w', 'w', 'w', 'w', 'w', 'w', 'f', 'w' },
            new char[10] { 'w', 'f', 'w', ' ', ' ', ' ', ' ', 'w', 'f', 'w' },
            new char[10] { 'w', 'f', 'w', ' ', ' ', ' ', ' ', 'w', 'f', 'w' },
            new char[10] { 'w', 'f', 'w', 'w', 'w', 'w', 'w', 'w', 'f', 'w' },
            new char[10] { 'w', 'f', 'w', 'f', 'f', 'f', 'f', 'f', 'f', 'w' },
            new char[10] { 'w', 'f', 'w', 'f', 'w', 'w', 'w', 'w', 'f', 'w' },
            new char[10] { 'w', 'f', 'w', 'f', 'w', ' ', ' ', 'w', 'f', 'w' },
            new char[10] { 'w', 'f', 'w', 'f', 'w', ' ', ' ', 'w', 'f', 'w' },
            new char[10] {'w', 'f', 'f', 'f', 'w', ' ', ' ', 'w', 'f', 'w' },
            new char[10] {'w', 'w', 'w', 'w', 'w', ' ', ' ', 'w', ' ', 'w' },
        };

        _level.Add(floor);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
