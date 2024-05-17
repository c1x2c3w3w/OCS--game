using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

    public static GameObject[,] hexGrid;                   //存放在地图上的游戏物体
    public static GameObject[] units;                     // 存放 所有场上的部队
    public static TurnFlow turnflow = new TurnFlow();    // 存放 回合流程参数操作


    public static Vector2 getHexWorldCoordinate(int row, int col)
    { 
        Vector2 pos = new Vector2();
        pos.x = hexGrid[row, col].transform.position.x;
        pos.y = hexGrid[row, col].transform.position.y;
        return pos;
    }
    public static Vector2Int getPosOfObject(string name)
    {
        string[] st = name.Split("_");
        Debug.Log(st[0] + "  " + st[1] + "  " + st[2]);
        int i = int.Parse(st[1]);
        int j = int.Parse(st[2]);
        return new Vector2Int(i, j);
    }
}
