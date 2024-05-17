using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter //  ����ඨ�塰���ӡ�  ÿ�����Ӿ������桢�������ݡ�
{
    public int nation;            //  �����ĸ�����        0���й��뱱����     1���������Ϻ�
    public int type;              //  �����������͵Ĳ���   0������    1���ڱ�     2��̹��   3:�վ�

    public int attackArea;        //  ������Χ
    public int attack;            //  ������ ��ֵ
    public int actionRating;      //  �ж��ȼ�
    public int moveAllow;         //  �ƶ�����
    public int defense;           //  ������=�ż���Ŀ ��ֵ
    public int attackInjure;      //  ս��ģʽ�Ľ����� ��ֵ
    public int actionRatingInjure; // ս��ģʽ�� �ж��ȼ�
    public int moveAllowInjure;    //  ս��ģʽ���ƶ�����

    public static Counter[] counter = Counter.GenerateCounterData();

    public static Counter[] GenerateCounterData()
    {
        Counter[] ct =
        {
            new Counter(0, 0, 1, 5, 3, 3, 1, 3, 3, 4),       //0
            new Counter(0, 0, 1, 5, 3, 3, 1, 3, 3, 5),       //1
            new Counter(0, 0, 1, 12, 3, 3, 3, 6, 3, 5),      //2
            new Counter(0, 0, 1, 12, 2, 3, 3, 6, 2, 4),      //3
            new Counter(0, 0, 1, 12, 3, 3, 3, 6, 3, 4),      //4
            new Counter(0, 0, 1, 10, 2, 3, 3, 5, 2, 4),      //5
            new Counter(0, 1, 2, 9, 1, 2, 1, 5, 1, 4),       //6
            new Counter(0, 1, 2, 12, 1, 2, 1, 6, 1, 4),      //7
            new Counter(1, 0, 1, 5, 3, 3, 1, 3, 3, 5),       //8
            new Counter(1, 0, 1, 10, 5, 4, 1, 5, 5, 16),     //9
            new Counter(1, 0, 1, 10, 4, 4, 1, 5, 4, 16),     //10
            new Counter(1, 1, 3, 30, 3, 3, 1, 15, 3, 14),    //11
            new Counter(1, 1, 3, 29, 2, 5, 1, 15, 2, 14),    //12
            new Counter(1, 2, 1, 7, 4, 6, 1, 4, 4, 16),      //13
            new Counter(1, 2, 1, 7, 4, 6, 1, 4, 4, 16),      //14
            new Counter(1, 0, 1, 9, 3, 3, 1, 5, 3, 14),      //15
            new Counter(1, 1, 3, 30, 2, 3, 1, 15, 2, 14),    //16
            new Counter(1, 3, 102, 56, 0, 102, 1, 56, 0, 102),  //17
            new Counter(1, 3, 135, 10, 0, 135, 1, 10, 0, 135),   //18
        };
     return ct;
    }

    public Counter(int n, int t, int aa, int a, int r, int m,  int d, int ai, int ri, int mi)
    {
        nation = n; type = t;  attackArea = aa;
        attack = ai;actionRating = ri; moveAllow = mi;
        defense = d;
        attackInjure = a;actionRatingInjure = r; moveAllowInjure = m;
        
    }
}


