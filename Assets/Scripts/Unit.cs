using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public int row, col;            //  算子位置
    public int counterID;           //  算子ID
    public string division;         //  师的编号
    public bool fullState;          //  状态值：  true  战斗模式状态面（正面）   false  移动模式面（反面）
    public bool alreadyRaiseAttack; //  状态值：  true  本回合已发起过攻击  false   本回合还未发起攻击
    public bool alreadAttacked;     //  状态值：  true  本回合已被攻击      false   本回合未被攻击过
    public bool finishTurnMoved;    //  状态值：  true  本回合已移动        false  本回合还未移动
    public bool strategyMove;       //  状态值：  true  本回合采用战略移动   false  本回合是普通移动模式
    public int movingPoint;        //   状态值：  本回合剩余移动力（简称 MP）
    public int remainDefense;       //  状态值：  算子剩余防守力
    public void setInitValue(int r, int c, int cID, string div)
    {
        row = r; col = c;
        counterID = cID;
        division = div;
        fullState = false;
        alreadyRaiseAttack = false;
        alreadAttacked = false;
        finishTurnMoved = false;
        strategyMove = false;
        movingPoint = fullState ? Counter.counter[counterID].moveAllow : Counter.counter[counterID].moveAllowInjure;
        remainDefense = Counter.counter[counterID].defense;
    }

    public void UpdateUnitPosition(Vector2Int rowcol)
    {
        Vector3 pos = GameData.getHexWorldCoordinate(rowcol.x, rowcol.y);
        this.transform.localPosition = pos;
        this.row = rowcol.x;
        this.col = rowcol.y;

    }

    public void useMovingPoint(int pointUsed)   // 使用掉移动点数，可能变成0以下（要置回0）
    {
        this.movingPoint -= pointUsed;
        if (this.movingPoint < 0) this.movingPoint = 0;
    }
}
