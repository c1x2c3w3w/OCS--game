using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitElement
{
    // ��������
    public int counterType;       // һ�����ӵ� ID ��š�
    public string division;       //  ���ڵڼ���ʦ��������ʦ�ı�ţ����ڿɲ�������д0���գ��������о��������ϣ��ÿ���


    // ����ս������
    public int comeAtTurn;        // �ڼ��غϽ��������   0�������ʼ���þͽ����Ĳ���   1����һ�غϽ������볡, ����
    public int comeRow, comeCol;  // ����ʱ�����꣬�ڼ��еڼ��� (��Ϊ-1��������������ã���Ϊ�������Ĺ���)


    public UnitElement(int c, string div, int turn, int row, int col)
    {
        this.counterType = c;
        this.division = div;
        this.comeAtTurn = turn;
        this.comeRow = row;
        this.comeCol = col;
    }
}