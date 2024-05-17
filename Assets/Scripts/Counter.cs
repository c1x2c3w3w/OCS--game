using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter //  这个类定义“算子”  每个算子具有正面、反面数据。
{
    public int nation;            //  属于哪个国家        0：中国与北朝鲜     1：美国与南韩
    public int type;              //  属于哪种类型的部队   0：步兵    1：炮兵     2：坦克   3:空军

    public int attackArea;        //  攻击范围
    public int attack;            //  进攻力 数值
    public int actionRating;      //  行动等级
    public int moveAllow;         //  移动点数
    public int defense;           //  防守力=团级数目 数值
    public int attackInjure;      //  战斗模式的进攻力 数值
    public int actionRatingInjure; // 战斗模式的 行动等级
    public int moveAllowInjure;    //  战斗模式的移动点数

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


