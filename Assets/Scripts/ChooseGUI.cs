using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class ChooseGUI : MonoBehaviour
{
    GameObject[] FightArmy;
    GameObject[] DefenseArmy;
    private void Awake()
    {
        FightArmy = new GameObject[10];
        DefenseArmy = new GameObject[5];
        for (int i = 0; i < 10; i++)
        {
            FightArmy[i] = GameObject.Find("FightArmy" + i.ToString());
            GameObject redRectBorder = Utils.getChildObject(FightArmy[i], "RedSquareBorder");
            redRectBorder.GetComponent<RedRectBorder>().setSelectedState2(false);
        }
        for (int i = 0; i < 5; i++)
        {
            DefenseArmy[i] = GameObject.Find("DefenseArmy" + i.ToString());
            GameObject redRectBorder = Utils.getChildObject(DefenseArmy[i], "RedSquareBorder");
            redRectBorder.GetComponent<RedRectBorder>().setSelectedState2(false);
        }
    }


    public int[] showChooseArmy(List<int> selectedJoinAttackArmyIndex,List<int> armyInTargetHexGrid)
    {
        int count = 0;
        int count2 = 0;
        // 设置一个数组，长度为四。记录ArmyIconSelected0、ArmyIconSelected1、ArmyIconSelected2 存放的选定部队编号
        int[] chooseArmyIndex = new int[15];

        for (int i = 0; i < 10; i++)
        {
            FightArmy[i].GetComponent<Image>().sprite = GetComponent<CounterImage>().getCounterPicture(-1);
            GameObject Army = Utils.getChildObject(FightArmy[i], "Army");
            Army.GetComponent<RedRectBorder>().setSelectedState2(false);
            GameObject redRectBorder = Utils.getChildObject(FightArmy[i], "RedSquareBorder");
            redRectBorder.GetComponent<RedRectBorder>().setSelectedState2(false);
            chooseArmyIndex[i] = -1;   // 代表部队编号 不存在
        }
        for (int i = 0; i < 5; i++)
        {
            DefenseArmy[i].GetComponent<Image>().sprite = GetComponent<CounterImage>().getCounterPicture(-1);
            GameObject Army = Utils.getChildObject(DefenseArmy[i], "Army");
            Army.GetComponent<RedRectBorder>().setSelectedState2(false);
            GameObject redRectBorder = Utils.getChildObject(DefenseArmy[i], "RedSquareBorder");
            redRectBorder.GetComponent<RedRectBorder>().setSelectedState2(false);
            chooseArmyIndex[i+10] = -1;   // 代表部队编号 不存在
        }
        for (int j = 0; j < selectedJoinAttackArmyIndex.Count; j++) 
        {
 
                int index = selectedJoinAttackArmyIndex[j];
                GameObject unitObject = GameData.units[index];
                Unit unit = unitObject.GetComponent<Unit>();
  
                chooseArmyIndex[count] = selectedJoinAttackArmyIndex[j];   //记录下第count个army的部队编号
                GameObject gobj = FightArmy[count];

                Sprite spriteBG = GetComponent<CounterImage>().getCounterPicture(unit.counterID); //  获得 sprite
                gobj.GetComponent<Image>().sprite = spriteBG;    // GUI 游戏物体的图，换成对应的图

            // 更新当前 ArmyIconSelected[count] 这个部队的信息（移动力、攻击值、防守值、回合移动攻击状态等）
                UpdateFightArmyInSelectedGrid(count, unit);

                count++; 
        }
        for (int j = 0; j < armyInTargetHexGrid.Count; j++)
        {

            int index = armyInTargetHexGrid[j];
            GameObject unitObject = GameData.units[index];
            Unit unit = unitObject.GetComponent<Unit>();

            chooseArmyIndex[count] = armyInTargetHexGrid[j];   //记录下第count个army的部队编号
            GameObject gobj = DefenseArmy[count2];

            Sprite spriteBG = GetComponent<CounterImage>().getCounterPicture(unit.counterID); //  获得 sprite
            gobj.GetComponent<Image>().sprite = spriteBG;    // GUI 游戏物体的图，换成对应的图

            // 更新当前 ArmyIconSelected[count] 这个部队的信息（移动力、攻击值、防守值、回合移动攻击状态等）
            UpdateDefenseArmyInSelectedGrid(count2, unit);
            count++;
            count2++;
        }
        return chooseArmyIndex;
    }


    private void UpdateFightArmyInSelectedGrid(int count, Unit u)  // 更新GUI面板上的部队信息（编号为count）
    {
        GameObject textArea = Utils.getChildObject(FightArmy[count], "chooseTextArea");
        textArea.GetComponent<Text>().text =  "部队编号:" + u.counterID.ToString();
        if(u.remainDefense <= 0)
        {
            FightArmy[count].GetComponent<Image>().sprite = GetComponent<CounterImage>().getCounterPicture(-1);
            textArea.GetComponent<Text>().text = "部队编号:";
        }
    }
    private void UpdateDefenseArmyInSelectedGrid(int count, Unit u)
    {
        GameObject textArea2 = Utils.getChildObject(DefenseArmy[count], "chooseTextArea");
        textArea2.GetComponent<Text>().text = "部队编号:" + u.counterID.ToString();
        if (u.remainDefense <= 0)
        {
            DefenseArmy[count].GetComponent<Image>().sprite = GetComponent<CounterImage>().getCounterPicture(-1);
            textArea2.GetComponent<Text>().text = "部队编号:";
        }
    }
    public static void ShowGameObject(GameObject gobj)
    {
        gobj.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public static void HideGameObject(GameObject gobj)
    {
        gobj.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    public GameObject getFightIconSelectedFromIndex(int k)   // 获得第k个ArmyIconSelected
    {
        return FightArmy[k];
    }
    public GameObject getDenfenseIconSelectedFromIndex(int k)   // 获得第k个ArmyIconSelected
    {
        return DefenseArmy[k];
    }
}
