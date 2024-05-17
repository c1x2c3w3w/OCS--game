using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

    public static GameObject[,] hexGrid;                   //����ڵ�ͼ�ϵ���Ϸ����
    public static GameObject[] units;                     // ��� ���г��ϵĲ���
    public static TurnFlow turnflow = new TurnFlow();    // ��� �غ����̲�������


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
