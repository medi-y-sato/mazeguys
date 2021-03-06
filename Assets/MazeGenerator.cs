﻿using System;
using System.Collections.Generic;

using UnityEngine;


public class MazeGenerator : MonoBehaviour
{
  public GameObject Floor;
  int[,] maze; // 1 = 床 0 = 穴 2=道(外とつながってる床)
  public int maze_x_def = 10;
  public int maze_y_def = 10;
  GameObject[,] mazeFloors;
  public GameObject Goal; // ゴール
  public GameObject Character; // キャラクタ

  public bool makingMazeFlug = false;

  // Start is called before the first frame update
  void Start()
  {
    createMaze();
  }

  // Update is called once per frame
  void Update()
  {
  }

  public void createMaze()
  {
    makingMazeFlug = false;

    // 既にchildが居たら全部消す
    for (int i = 0; i < this.transform.childCount; ++i)
    {
      GameObject.Destroy(this.transform.GetChild(i).gameObject);
    }

    // マス数を倍にする
    int maze_x = maze_x_def * 2 + 1;
    int maze_y = maze_y_def * 2 + 1;

    maze = new int[maze_x, maze_y];

    // 迷路作る
    int[,] maze_result = makeMaze(maze, maze_x, maze_y);

    // 板作る
    mazeFloors = new GameObject[maze_x, maze_y];

    for (int x = 0; x < maze_x; x++)
    {
      for (int y = 0; y < maze_y; y++)
      {
        mazeFloors[x, y] = makeFloorObjects(x, y, maze_result[x, y]);
      }
    }

    // ゴールを移動する
    Goal.transform.position = new Vector3(maze_x - 1, 0, maze_y - 1);

    // 迷路準備完了
    makingMazeFlug = true;
  }

  int[,] makeMaze(int[,] maze, int maze_x, int maze_y)
  {

    // 全部を穴にする
    for (int x = 0; x < maze_x; x++)
    {
      for (int y = 0; y < maze_y; y++)
      {
        maze[x, y] = 0;
      }
    }

    // 道の起点になるマス目を作る
    for (int y = 1; y < maze_y; y += 2)
    {
      for (int x = 1; x < maze_x; x += 2)
      {
        maze[x, y] = 1;
      }
    }

    // 適当な点から床を伸ばす
    List<int> randomTarget = new List<int>(maze_x * maze_y);
    for (int i = 0; i < maze_x * maze_y; i++) randomTarget.Add(i);

    System.Random r1 = new System.Random();
    do
    {
      int currentTarget = r1.Next(0, randomTarget.Count);
      int target_x = (int)Math.Ceiling(currentTarget / maze_x * 1.0);
      int target_y = currentTarget % maze_x;
      Debug.Log(target_x + " / " + target_y );
      if (maze[target_x, target_y] == 1)
      {
        extendFloor(target_x, target_y, maze_x, maze_y);
      }
      randomTarget.RemoveAt(currentTarget);
    } while (randomTarget.Count > 0);

    // 迷路を返して終わり
    return maze;

  }


  void extendFloor(int x, int y, int maze_x, int maze_y)
  {
    if (maze[x, y] == 1)
    { // 床だった場合

      maze[x, y] = 2; // 道にする

      // 上下左右の2つ先に床があるかチェック1
      List<string> checkFloorCanUse = new List<string>();

      // ↑
      if (y > 2 && maze[x, y - 2] == 1) { checkFloorCanUse.Add("up"); }
      // ↓
      if (y < maze_y - 2 && maze[x, y + 2] == 1) { checkFloorCanUse.Add("down"); }
      // ←
      if (x > 2 && maze[x - 2, y] == 1) { checkFloorCanUse.Add("left"); }
      // →
      if (x < maze_x - 2 && maze[x + 2, y] == 1) { checkFloorCanUse.Add("right"); }

      // 伸ばせる方向が存在したら
      if (checkFloorCanUse.Count > 0)
      {
        //サイコロ振って適当な方向で伸ばし、再帰
        System.Random r = new System.Random();
        int targetIndex = r.Next(0, checkFloorCanUse.Count);

        Debug.Log("go " + checkFloorCanUse[targetIndex]);

        switch (checkFloorCanUse[targetIndex])
        {
          case "up":
            maze[x, y - 1] = 2;
            extendFloor(x, y - 2, maze_x, maze_y);
            break;
          case "down":
            maze[x, y + 1] = 2;
            extendFloor(x, y + 2, maze_x, maze_y);
            break;
          case "left":
            maze[x - 1, y] = 2;
            extendFloor(x - 2, y, maze_x, maze_y);
            break;
          case "right":
            maze[x + 1, y] = 2;
            extendFloor(x + 2, y, maze_x, maze_y);
            break;
          default:
            break;
        }
      }
      else
      {
      }
    }
  }

  GameObject makeFloorObjects(int x, int y, int objectKey)
  {
    if (objectKey != 2)
    {
      GameObject go = UnityEngine.Object.Instantiate(Floor) as GameObject;
      go.transform.position = new Vector3(x, 0, y);

      go.SetActive(true);
      go.transform.parent = this.transform;

      return go;
    }
    else
    {
      return null;
    }
  }

}
