using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperationGUI : MonoBehaviour
{
    GameObject[] ArmyIconSelected;

    private void Awake()
    {
        ArmyIconSelected = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            ArmyIconSelected[i] = GameObject.Find("ArmyIconSelected" + i.ToString());
            GameObject redRectBorder = Utils.getChildObject(ArmyIconSelected[i], "RedRectBorder");
            redRectBorder.GetComponent<RedRectBorder>().setSelectedState(false);
        }
     }


    public int[] showArmyInSelectdGrid(Vector2Int rowcol)
    {
        int count = 0;
        // ����һ�����飬����Ϊ�ġ���¼ArmyIconSelected0��ArmyIconSelected1��ArmyIconSelected2 ��ŵ�ѡ�����ӱ��
        int[] selectedArmyIndex = new int[4];

        for (int i = 0; i < 4; i++)
        {
            ArmyIconSelected[i].GetComponent<Image>().sprite = GetComponent<CounterImage>().getCounterPicture(-1);
            GameObject redRectBorder = Utils.getChildObject(ArmyIconSelected[i], "RedRectBorder");
            redRectBorder.GetComponent<RedRectBorder>().setSelectedState(false);
            selectedArmyIndex[i] = -1;   // �����ӱ�� ������

        }

        for (int i = 0; i < GameData.units.Length; i++)
        {
            
            Unit u = GameData.units[i].GetComponent<Unit>();
            if (u.row == rowcol.x && u.col == rowcol.y)
            {

                selectedArmyIndex[count] = i;   //��¼�µ�count��army�Ĳ��ӱ��
                GameObject gobj = ArmyIconSelected[count];

                Sprite spriteBG = GetComponent<CounterImage>().getCounterPicture(u.counterID); //  ��� sprite
                gobj.GetComponent<Image>().sprite = spriteBG;    // GUI ��Ϸ�����ͼ�����ɶ�Ӧ��ͼ
    
                // ���µ�ǰ ArmyIconSelected[count] ������ӵ���Ϣ���ƶ���������ֵ������ֵ���غ��ƶ�����״̬�ȣ�
                UpdateArmyInfoInSelectedGrid(count, u);

                if (ArmyIconSelected[count].GetComponent<Image>().sprite != GetComponent<CounterImage>().getCounterPicture(-1))
                {

                    // ���µ�ǰ ����army ��� ����ѡ��״̬�� �����Ϻ�ɫ����
                    GameObject redRectBorder = Utils.getChildObject(ArmyIconSelected[count], "RedRectBorder");
                    redRectBorder.GetComponent<RedRectBorder>().setSelectedState(true);
                }

                count++;
            }
        }
        return selectedArmyIndex;
    }

    private void UpdateArmyInfoInSelectedGrid(int count, Unit u)  // �������GUI����ϵĲ�����Ϣ�����Ϊcount��
    {
        GameObject textArea = Utils.getChildObject(ArmyIconSelected[count], "TextArea");

        
        GameObject textMovePoint = Utils.getChildObject(textArea, "TextMovePoint");
        textMovePoint.GetComponent<Text>().text = "�ƶ�����" + u.movingPoint.ToString();

        GameObject textState = Utils.getChildObject(textArea, "TextState");
        textState.GetComponent<Text>().text = "״̬��" + (u.fullState ? "ս��ģʽ" : "�ƶ�ģʽ");

        int attack = u.fullState ?  Counter.counter[u.counterID].attackInjure : Counter.counter[u.counterID].attack;
        int endAttack = u.strategyMove ? 0 : attack;   //ս��ģʽʱ�޷�����
        GameObject textAttack = Utils.getChildObject(textArea, "TextAttack");
        textAttack.GetComponent<Text>().text = "����ֵ��" + endAttack.ToString();

        GameObject textDefense = Utils.getChildObject(textArea, "TextDefense");
        textDefense.GetComponent<Text>().text = "����ֵ��" + u.remainDefense.ToString();

        string moveReady = u.finishTurnMoved ? "���غ����ƶ�" : " ���غ�δ�ƶ�";
        GameObject textTurnMoved = Utils.getChildObject(textArea, "TextTurnMoved");
        textTurnMoved.GetComponent<Text>().text = moveReady;

        string attackReady = u.alreadyRaiseAttack ? "���غ��ѹ���" : " ���غ�δ����";
        GameObject TextTurnRasiedAtt = Utils.getChildObject(textArea, "TextTurnRasiedAtt");
        TextTurnRasiedAtt.GetComponent<Text>().text = attackReady;
        // ����״ֵ̬ to do 
        if (u.remainDefense <= 0)
        {
            ArmyIconSelected[count].GetComponent<Image>().sprite = GetComponent<CounterImage>().getCounterPicture(-1);
            GameObject redRectBorder = Utils.getChildObject(ArmyIconSelected[count], "RedRectBorder");
            redRectBorder.GetComponent<RedRectBorder>().setSelectedState(false);
            textMovePoint.GetComponent<Text>().text = "�ƶ�����";
            textAttack.GetComponent<Text>().text = "����ֵ��";
            textDefense.GetComponent<Text>().text = "����ֵ��";
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

    public GameObject getArmyIconSelectedFromIndex(int k)   // ��õ�k��ArmyIconSelected
    {
        return ArmyIconSelected[k];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
