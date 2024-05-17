using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class LookRuleTable
{
    // Start is called before the first frame update
    public static string[,] shootTable;
    public static string[,] FightTable;
    static LookRuleTable()
    {
        // Initialize shootTable here
        shootTable = new string[,]
        {
            { "0" , "0" ,"0" ,"0" ,"0" ,"0" ,"0" ,"0" ,"0" ,"DG","DG"},
            { "0" , "0" ,"0" ,"0" ,"0" ,"0" ,"0" ,"0" ,"DG","DG","DG"},
            { "0" , "0" ,"0" ,"0" ,"0" ,"0" ,"0" ,"DG","DG","DG","DG"},
            { "0" , "0" ,"0" ,"0" ,"0" ,"0" ,"DG","DG","DG","DG","1/2"},
            { "0" , "0" ,"0" ,"0" ,"0" ,"DG","DG","DG","DG","1/2","1/2"},
            { "0" , "0" ,"0" ,"0" ,"DG","DG","DG","DG","1/2","1/2","1/2"},
            { "0" , "0" ,"DG","DG","DG","1/2","1/2","1/2","1/2","1/2","1/2"},
            { "0" , "DG","DG","DG","1/2","1/2","1/2","1/2","1/2","1","1"},
            { "DG", "DG","DG","1/2","1/2","1/2","1/2","1/2","1","1","2"},
            { "DG", "1/2","1/2","1/2","1/2","1","1","1","1","2","3"},
        };
        FightTable = new string[,]
        {
            {"AL2","AL2", "AL2", "AL2", "AL2", "AL2", "AL2","AL1o1","AL1o1Do1","AL1o1Do1","AL1Do1","AL1Do1","AL1DL1o1"},
            {"AL2","AL2", "AL2", "AL2", "AL2", "AL2", "AL1o1","AL1o1Do1","AL1o1Do1","AL1Do1","AL1Do1","AL1DL1o1","AL1DL1o1"},
            {"AL2","AL2", "AL2", "AL2", "AL2", "AL1o1","AL1o1Do1","AL1o1Do1","AL1Do1","AL1Do1","AL1DL1o1","AL1DL1o1","AL1DL1o1"},
            {"AL2","AL2", "AL2", "AL2", "AL1o1","AL1o1Do1","AL1o1Do1","AL1Do1","AL1Do1","Ao1Do1","Ao1DL1o1","Ao1DL1o2","Ao1e4DL1o2"},
            {"AL2","AL2","AL2","AL1o1","AL1o1Do1","AL1o1Do1","AL1Do1","AL1Do1","Ao1Do1","Ao1DL1o1","Ao1DL1o1","Ao1e4DL1o2","Ae4DL1o2"},
            {"AL2","AL2","AL1o1","AL1o1Do1","AL1o1Do1","AL1Do1","AL1Do1","Ao1Do1","Ao1DL1o1","Ao1DL1o1","Ao1DL1o1", "Ae4DL1o2","Ae4DL1o2"},
            {"AL1o1","AL1o1","AL1o1Do1","AL1o1Do1","AL1o1Do1", "AL1Do1","AL1Do1","Ao1DL1o1","Ao1DL1o1","Ao1DL1o1","Ao1e4DL1o2","Ae4DL1o2","Ae3DL2o2DG"},
            {"AL1o1","AL1o1Do1","AL1o1Do1","AL1o1Do1", "AL1Do1","AL1Do1","Ao1DL1o1","Ao1DL1o1","Ao1DL1o1","Ao1e4DL1o2","Ae4DL1o2","Ae4DL1o2","Ae3DL2o2DG"},
            {"AL1o1Do1","AL1o1Do1","AL1o1Do1", "AL1Do1","Ao1Do1","Ao1Do1","Ao1DL1o1","Ao1DL1o1","Ao1e4DL1o2","Ae4DL1o2","Ae4DL1o2","Ae3DL2o2DG","Ae3DL2o2DG"},
            {"AL1o1Do1","AL1o1Do1", "AL1Do1","Ao1Do1","Ao1Do1","Ao1DL1o1","Ao1DL1o1","Ao1e4DL1o2","Ae4DL1o2","Ae4DL1o2","Ae3DL2o2DG","Ae3DL2o2DG","Ae2DL2o3DG"},
            {"AL1o1Do1", "AL1Do1","Ao1Do1","Ao1Do1","Ao1DL1o1","Ao1DL1o1","Ao1e4DL1o2","Ae4DL1o2","Ae4DL1o2","Ae3DL2o2DG","Ae3DL2o2DG","Ae3DL2o2DG","Ae2DL2o3DG"},
            {"AL1o1Do1", "Ao1Do1","Ao1Do1","Ao1DL1o1","Ao1DL1o1","Ao1DL1o1","Ao1e4DL1o2","Ao1e4DL1o2","Ae4DL1o2","Ae3DL2o2DG","Ae3DL2o2DG","Ae3DL2o3DG" ,"Ae3DL2o3DG"},
            {"Ao1Do1","Ao1Do1","Ao1DL1o1","Ao1DL1o1","Ao1DL1o2","Ao1e4DL1o2","Ao1e4DL1o2","Ae4DL1o2","Ae3DL2o2DG","Ae3DL2o2DG","Ae3DL2o3DG" ,"Ae3DL2o3DG" ,"Ae3DL2o3DG"},
            {"Ao1Do1","Ao1DL1o1","Ao1DL1o1","Ao1e4DL1o2","Ao1e4DL1o2","Ao1e4DL1o2","Ae4DL1o2","Ae3DL2o2DG","Ae3DL2o2DG","Ae3DL2o3DG" ,"Ae3DL2o3DG" ,"Ae3DL2o3DG","Ae3DL2o3DG"},
            {"Ao1DL1o1","Ao1DL1o1","Ao1e4DL1o2","Ao1e4DL1o2","Ao1e4DL1o2","Ae4DL1o2","Ae3DL2o2DG","Ae3DL2o3DG","Ae3DL2o3DG" ,"Ae3DL2o3DG" ,"Ae3DL2o3DG","Ae3DL2o3DG","Ae3DL2o3DG"}
        };
    }


    public static string LookUpRuleTable(List<int> selectedJoinAttackArmyIndex,List<int> armyInTargetHexGrid)
    {
        int targetHex_Defense = 0;
        int sumAttack = 0;
        int level = 0;
        int move = 0;
        bool stratageMovingState = false;
        int dice = Random.Range(2, 13);
        for (int i = 0; i < armyInTargetHexGrid.Count; i++)
        {
            Unit u = GameData.units[armyInTargetHexGrid[i]].GetComponent<Unit>();
            targetHex_Defense += Counter.counter[u.counterID].defense;
            if (u.strategyMove == true) stratageMovingState = true;
        }
        Debug.Log("总防御值：" + targetHex_Defense);
        for (int i = 0; i < selectedJoinAttackArmyIndex.Count; i++)
        {
            Unit u = GameData.units[selectedJoinAttackArmyIndex[i]].GetComponent<Unit>();
            sumAttack += u.fullState ? Counter.counter[u.counterID].attackInjure : Counter.counter[u.counterID].attack;
            Debug.Log("攻击值：" + sumAttack);
        }
        Debug.Log("总攻击：" + sumAttack + "   "+dice);

        if (sumAttack <= 1) level = 1;
        else if (sumAttack == 2) level = 2;
        else if (sumAttack >= 3 && sumAttack <=4) level = 3;
        else if (sumAttack >= 5 && sumAttack <= 7) level = 4;
        else if (sumAttack >= 8 && sumAttack <= 11) level = 5;
        else if (sumAttack >= 12 && sumAttack <= 16) level = 6;
        else if (sumAttack >= 17 && sumAttack <= 24) level = 7;
        else if (sumAttack >= 25 && sumAttack <= 40) level = 8;
        else if (sumAttack >= 41 && sumAttack <= 68) level = 9;
        else if (sumAttack >= 69 && sumAttack <= 116) level = 10;
        else level = 11;

        if (GameController.targetHex.terrain == 3 || GameController.targetHex.terrain == 4) move -= 1;   //如果地形为close,very close则左移1
        if (GameController.targetHex.terrain == 5) move -= 2;   //如果地形为extre close则左移2
        if (stratageMovingState == true) move += 3;   //如果格内任意一个为战略模式则右移3
        if (targetHex_Defense <= 1) move -= 1;
        else if (targetHex_Defense > 1 && targetHex_Defense <= 3) move += 0;
        else if (targetHex_Defense > 3 && targetHex_Defense <= 4) move += 1;
        else if (targetHex_Defense > 4 && targetHex_Defense <= 5) move += 2;
        else if (targetHex_Defense > 5 && targetHex_Defense <= 6) move += 3;
        else if (targetHex_Defense > 6) move += 4;


        int x = 0;
        int y = 0;
        x = dice - 2;
        y = level + move - 1;
        if (level + move - 1 < 0) x = 0;
        //if(shootTable[x,y] == "1/2" || shootTable[x,y] == "DG") MessageBox.Show("部队DG");
        Debug.Log("move" + move + "injure" + shootTable[x, y]);
        return shootTable[x, y];

    }

    public static string LookUpFightRuleTable(List<int> selectedJoinAttackArmyIndex, List<int> armyInTargetHexGrid)   //战斗表
    {
        int Defense = 0; //守方战斗值
        int Attack = 0;  //攻方战斗值
        int DRM = 0;
        int dice = Random.Range(1, 7);
        int dice1 = Random.Range(1, 7);
        int dice2 = Random.Range(1, 7);
        int fight = 0;
        int move = 0;    //如果发生奇袭列的左右移动数
        int level = 0;
        for (int i = 0; i < armyInTargetHexGrid.Count; i++)
        {
            Unit u = GameData.units[armyInTargetHexGrid[i]].GetComponent<Unit>();
            Defense += u.fullState ? Counter.counter[u.counterID].attackInjure : Counter.counter[u.counterID].attack;
            
        }
        Debug.Log("守方战斗值：" + Defense);
        for (int i = 0; i < selectedJoinAttackArmyIndex.Count; i++)
        {
            Unit u = GameData.units[selectedJoinAttackArmyIndex[i]].GetComponent<Unit>();
            Attack += u.fullState ? Counter.counter[u.counterID].attackInjure : Counter.counter[u.counterID].attack;
            Debug.Log("攻方战斗值：" + Attack);
        }
        fight = Attack / Defense;
        DRM = selectedJoinAttackArmyIndex.Max() - armyInTargetHexGrid.Max();
        if (dice1 + dice2 >= 10) move += dice;
        if (dice1 + dice2 <= 5) move -= dice;
        else move = 0;


        if(GameController.targetHex.terrain == 1 || GameController.targetHex.terrain == 2)   //open
        {
            if (fight < 1 / 4) level = 1;
            if (fight >= 1 / 4 || fight < 1 / 3) level = 2;
            if (fight >= 1 / 3 || fight < 1 / 2) level = 3;
            if (fight >= 1 / 2 || fight < 1 / 1) level = 4;
            if (fight >= 1 / 1 || fight < 2 / 1) level = 5;
            if (fight >= 2 / 1 || fight < 3 / 1) level = 6;
            if (fight >= 3 / 1 || fight < 4 / 1) level = 7;
            if (fight >= 4 / 1 || fight < 5 / 1) level = 8;
            if (fight >= 5 / 1 || fight < 7 / 1) level = 9;
            if (fight >= 7 / 1 || fight < 9 / 1) level = 10;
            if (fight >= 9 / 1 || fight < 11 / 1) level = 11;
            if (fight >= 11 / 1 || fight < 13 / 1) level = 12;
            if (fight >= 13 / 1) level = 13;
        }
        if (GameController.targetHex.terrain == 3)   //close
        {
            if (fight < 1 / 3) level = 1;
            if (fight >= 1 / 3 || fight < 1 / 2) level = 2;
            if (fight >= 1 / 2 || fight < 1 / 1) level = 3;
            if (fight >= 1 / 1 || fight < 2 / 1) level = 4;
            if (fight >= 2 / 1 || fight < 3 / 1) level = 5;
            if (fight >= 3 / 1 || fight < 4 / 1) level = 6;
            if (fight >= 4 / 1 || fight < 6 / 1) level = 7;
            if (fight >= 6 / 1 || fight < 8 / 1) level = 8;
            if (fight >= 8 / 1 || fight < 10 / 1) level = 9;
            if (fight >= 10 / 1 || fight < 12 / 1) level = 10;
            if (fight >= 12 / 1 || fight < 15 / 1) level = 11;
            if (fight >= 15 / 1 || fight < 18 / 1) level = 12;
            if (fight >= 18 / 1) level = 13;
        }
        if (GameController.targetHex.terrain == 4)   //very close
        {
            if (fight < 1 / 2) level = 1;
            if (fight >= 1 / 2 || fight < 1 / 1) level = 2;
            if (fight >= 1 / 1 || fight < 2 / 1) level = 3;
            if (fight >= 2 / 1 || fight < 3 / 1) level = 4;
            if (fight >= 3 / 1 || fight < 4 / 1) level = 5;
            if (fight >= 4 / 1 || fight < 6 / 1) level = 6;
            if (fight >= 6 / 1 || fight < 9 / 1) level = 7;
            if (fight >= 9 / 1 || fight < 12 / 1) level = 8;
            if (fight >= 12 / 1 || fight < 15 / 1) level = 9;
            if (fight >= 15 / 1 || fight < 18 / 1) level = 10;
            if (fight >= 18 / 1 || fight < 21 / 1) level = 11;
            if (fight >= 21 / 1 || fight < 24 / 1) level = 12;
            if (fight >= 24 / 1) level = 13;
        }
        if (GameController.targetHex.terrain == 5)   //extr close
        {
            if (fight < 1 / 1) level = 1;
            if (fight >= 1 / 1 || fight < 2 / 1) level = 2;
            if (fight >= 2 / 1 || fight < 3 / 1) level = 3;
            if (fight >= 3 / 1 || fight < 4 / 1) level = 4;
            if (fight >= 4 / 1 || fight < 8 / 1) level = 5;
            if (fight >= 8 / 1 || fight < 12 / 1) level = 6;
            if (fight >= 12 / 1 || fight < 16 / 1) level = 7;
            if (fight >= 16 / 1 || fight < 20 / 1) level = 8;
            if (fight >= 20 / 1 || fight < 28 / 1) level = 9;
            if (fight >= 28 / 1 || fight < 36 / 1) level = 10;
            if (fight >= 36 / 1 || fight < 44 / 1) level = 11;
            if (fight >= 44 / 1 || fight < 52 / 1) level = 12;
            if (fight >= 52 / 1) level = 13;
        }
        int x = dice1 + dice2 + DRM;
        if (x <= 1) x = 1;
        if (x >= 15) x = 15;
        Debug.Log("dice12" + (dice1 + dice2) +"DRM" + DRM +"dice" + dice +"move:" + move);
        int y = level + move;
        Debug.Log("战斗表：" + FightTable[x - 1, y - 1]);
        return FightTable[x-1,y-1];
    }
}
