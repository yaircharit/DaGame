using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BlockType { Grass };
public enum BlockSide { Top, Bottom, North, South, East, West };

public static class BlockData // TODO: Consider making this class to a base for Block class
{
    public static readonly Vector3[] vertices = new Vector3[8] {
        new Vector3(0,0,0),// 0         7-----------3
        new Vector3(0,0,1),// 1        /|          /|          U   E
        new Vector3(0,1,0),// 2       / |         / |        Y+|  / Z+
        new Vector3(0,1,1),// 3      6--+--------2  |          | /
        new Vector3(1,0,0),// 4      |  5--------|--1    X+    |/ 
        new Vector3(1,0,1),// 5      | /         | /   N <-----+----- S
        new Vector3(1,1,0),// 6      |/          |/           /|    X-
        new Vector3(1,1,1),// 7      4-----------0         Z-/ |Y-
    };                     //                               W  D

    public static readonly int[,] triangles = new int[,] {
        { 6, 3, 7, 6, 2, 3 }, // Y+ Top    
        { 5, 0, 4, 5, 1, 0 }, // Y- Bottom (counter clock-wise)
        { 7, 4, 6, 7, 5, 4 }, // X+ North
        { 2, 1, 3, 2, 0, 1 }, // X- South
        { 3, 5, 7, 3, 1, 5 }, // Z+ East
        { 6, 0, 2, 6, 4, 0 }, // Z- West
    };

    public static readonly Vector2[] uvs = new Vector2[] {
        new Vector2(0,1),//     1-----------3
        new Vector2(1,0),//     |           | 
        new Vector2(1,1),//     |           |
        new Vector2(0,1),//     |           |
        new Vector2(0,0),//     |           |
        new Vector2(1,0),//     0-----------2
    };

    public static readonly Vector3[] neighbors = new Vector3[] {
        new Vector3(0,1,0), // Top
        new Vector3(0,-1,0),// Bottom
        new Vector3(1,0,0), // North
        new Vector3(-1,0,0),// South
        new Vector3(0,0,1), // East
        new Vector3(0,0,-1) // Werst
    };

}
