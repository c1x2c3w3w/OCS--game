using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        }
        for (int i = 0; i < 5; i++)
        {
            DefenseArmy[i] = GameObject.Find("DefenseArmy" + i.ToString());
        }
    }


    public int[] showChooseArmy(List<int> selectedJoinAttackArmyIndex,List<int> armyInTargetHexGrid)
    {
        int count = 0;
        int count2 = 0;
        // ����һ�����飬����Ϊ�ġ���¼ArmyIconSelected0��ArmyIconSelected1��ArmyIconSelected2 ��ŵ�ѡ�����ӱ��
        int[] chooseArmyIndex = new int[15];

        for (int i = 0; i < 10; i++)
        {
            FightArmy[i].GetComponent<Image>().sprite = GetComponent<CounterImage>().getCounterPicture(-1);
            GameObject Army = Utils.getChildObject(FightArmy[i], "Army");
            Army.GetComponent<RedRectBorder>().setSelectedState2(false);
            GameObject redRectBorder = Utils.getChildObject(FightArmy[i], "RedSquareBorder");
            redRectBorder.GetComponent<RedRectBorder>().setSelectedState2(false);
            chooseArmyIndex[i] = -1;   // �������ӱ�� ������
        }
        for (int i = 0; i < 5; i++)
        {
            DefenseArmy[i].GetComponent<Image>().sprite = GetComponent<CounterImage>().getCounterPicture(-1);
            GameObject Army = Utils.getChildObject(DefenseArmy[i], "Army");
            Army.GetComponent<RedRectBorder>().setSelectedState2(false);
            GameObject redRectBorder = Utils.getChildObject(DefenseArmy[i], "RedSquareBorder");
            redRectBorder.GetComponent<RedRectBorder>().setSelectedState2(false);
            chooseArmyIndex[i+10] = -1;   // �������ӱ�� ������
        }
        for (int j = 0; j < selectedJoinAttackArmyIndex.Count; j++) 
        {
 
                int index = selectedJoinAttackArmyIndex[j];
                GameObject unitObject = GameData.units[index];
                Unit unit = unitObject.GetComponent<Unit>();
  
                chooseArmyIndex[count] = selectedJoinAttackArmyIndex[j];   //��¼�µ�count��army�Ĳ��ӱ��
                GameObject gobj = FightArmy[count];

                Sprite spriteBG = GetComponent<CounterImage>().getCounterPicture(unit.counterID); //  ��� sprite
                gobj.GetComponent<Image>().sprite = spriteBG;    // GUI ��Ϸ�����ͼ�����ɶ�Ӧ��ͼ

            // ���µ�ǰ ArmyIconSelected[count] ������ӵ���Ϣ���ƶ���������ֵ������ֵ���غ��ƶ�����״̬�ȣ�
                UpdateFightArmyInSelectedGrid(count, unit);

                // ���µ�ǰ ����army ��� ����ѡ��״̬�� �����Ϻ�ɫ����
                //GameObject redRectBorder = Utils.getChildObject(FightArmy[count], "Army");
                //redRectBorder.GetComponent<RedRectBorder>().setSelectedState2(false);

                count++; 
        }
        for (int j = 0; j < armyInTargetHexGrid.Count; j++)
        {

            int index = armyInTargetHexGrid[j];
            GameObject unitObject = GameData.units[index];
            Unit unit = unitObject.GetComponent<Unit>();

            chooseArmyIndex[count] = armyInTargetHexGrid[j];   //��¼�µ�count��army�Ĳ��ӱ��
            GameObject gobj = DefenseArmy[count2];

            Sprite spriteBG = GetComponent<CounterImage>().getCounterPicture(unit.counterID); //  ��� sprite
            gobj.GetComponent<Image>().sprite = spriteBG;    // GUI ��Ϸ�����ͼ�����ɶ�Ӧ��ͼ

            // ���µ�ǰ ArmyIconSelected[count] ������ӵ���Ϣ���ƶ���������ֵ������ֵ���غ��ƶ�����״̬�ȣ�
            UpdateDefenseArmyInSelectedGrid(count2, unit);

            // ���µ�ǰ ����army ��� ����ѡ��״̬�� �����Ϻ�ɫ����
            //GameObject redRectBorder = Utils.getChildObject(DefenseArmy[count2], "Army");
            //redRectBorder.GetComponent<RedRectBorder>().setSelectedState2(false);

            count++;
            count2++;
        }
        return chooseArmyIndex;
    }


    private void UpdateFightArmyInSelectedGrid(int count, Unit u)  // ����GUI����ϵĲ�����Ϣ�����Ϊcount��
    {
        GameObject textArea = Utils.getChildObject(FightArmy[count], "chooseTextArea");
        textArea.GetComponent<Text>().text =  "���ӱ��:" + u.counterID.ToString();

    }
    private void UpdateDefenseArmyInSelectedGrid(int count, Unit u)
    {
        GameObject textArea2 = Utils.getChildObject(DefenseArmy[count], "chooseTextArea");
        textArea2.GetComponent<Text>().text = "���ӱ��:" + u.counterID.ToString();
    }
    public static void ShowGameObject(GameObject gobj)
    {
        gobj.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public static void HideGameObject(GameObject gobj)
    {
        gobj.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    public GameObject getFightIconSelectedFromIndex(int k)   // ��õ�k��ArmyIconSelected
    {
        return FightArmy[k];
    }
    public GameObject getDenfenseIconSelectedFromIndex(int k)   // ��õ�k��ArmyIconSelected
    {
        return DefenseArmy[k];
    }
}