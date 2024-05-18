using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public int row, col;            //  ����λ��
    public int counterID;           //  ����ID
    public string division;         //  ʦ�ı��
    public bool fullState;          //  ״ֵ̬��  true  ս��ģʽ״̬�棨���棩   false  �ƶ�ģʽ�棨���棩
    public bool alreadyRaiseAttack; //  ״ֵ̬��  true  ���غ��ѷ��������  false   ���غϻ�δ���𹥻�
    public bool alreadAttacked;     //  ״ֵ̬��  true  ���غ��ѱ�����      false   ���غ�δ��������
    public bool finishTurnMoved;    //  ״ֵ̬��  true  ���غ����ƶ�        false  ���غϻ�δ�ƶ�
    public bool strategyMove;       //  ״ֵ̬��  true  ���غϲ���ս���ƶ�   false  ���غ�����ͨ�ƶ�ģʽ
    public int movingPoint;        //   ״ֵ̬��  ���غ�ʣ���ƶ�������� MP��
    public int remainDefense;       //  ״ֵ̬��  ����ʣ�������
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

    public void useMovingPoint(int pointUsed)   // ʹ�õ��ƶ����������ܱ��0���£�Ҫ�û�0��
    {
        this.movingPoint -= pointUsed;
        if (this.movingPoint < 0) this.movingPoint = 0;
    }
}
