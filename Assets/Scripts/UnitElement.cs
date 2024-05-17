using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitElement
{
    // 基本数据
    public int counterType;       // 一个算子的 ID 编号。
    public string division;       //  属于第几个师（军队内师的编号，初期可不做，可写0留空，若不多有精力就填上，好看）


    // 进入战场数据
    public int comeAtTurn;        // 第几回合结束后进场   0：代表初始设置就进场的部队   1：第一回合结束后入场, 类推
    public int comeRow, comeCol;  // 进场时的坐标，第几行第几列 (若为-1，代表由玩家设置，此为待开发的功能)


    public UnitElement(int c, string div, int turn, int row, int col)
    {
        this.counterType = c;
        this.division = div;
        this.comeAtTurn = turn;
        this.comeRow = row;
        this.comeCol = col;
    }
}