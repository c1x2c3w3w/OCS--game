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
        // 设置一个数组，长度为四。记录ArmyIconSelected0、ArmyIconSelected1、ArmyIconSelected2 存放的选定部队编号
        int[] selectedArmyIndex = new int[4];

        for (int i = 0; i < 4; i++)
        {
            ArmyIconSelected[i].GetComponent<Image>().sprite = GetComponent<CounterImage>().getCounterPicture(-1);
            GameObject redRectBorder = Utils.getChildObject(ArmyIconSelected[i], "RedRectBorder");
            redRectBorder.GetComponent<RedRectBorder>().setSelectedState(false);
            selectedArmyIndex[i] = -1;   // 代表部队编号 不存在

        }

        for (int i = 0; i < GameData.units.Length; i++)
        {
            
            Unit u = GameData.units[i].GetComponent<Unit>();
            if (u.row == rowcol.x && u.col == rowcol.y)
            {

                selectedArmyIndex[count] = i;   //记录下第count个army的部队编号
                GameObject gobj = ArmyIconSelected[count];

                Sprite spriteBG = GetComponent<CounterImage>().getCounterPicture(u.counterID); //  获得 sprite
                gobj.GetComponent<Image>().sprite = spriteBG;    // GUI 游戏物体的图，换成对应的图
    
                // 更新当前 ArmyIconSelected[count] 这个部队的信息（移动力、攻击值、防守值、回合移动攻击状态等）
                UpdateArmyInfoInSelectedGrid(count, u);

                if (ArmyIconSelected[count].GetComponent<Image>().sprite != GetComponent<CounterImage>().getCounterPicture(-1))
                {

                    // 更新当前 部队army 变成 “被选中状态” （加上红色方框）
                    GameObject redRectBorder = Utils.getChildObject(ArmyIconSelected[count], "RedRectBorder");
                    redRectBorder.GetComponent<RedRectBorder>().setSelectedState(true);
                }

                count++;
            }
        }
        return selectedArmyIndex;
    }

    private void UpdateArmyInfoInSelectedGrid(int count, Unit u)  // 更新左侧GUI面板上的部队信息（编号为count）
    {
        GameObject textArea = Utils.getChildObject(ArmyIconSelected[count], "TextArea");

        
        GameObject textMovePoint = Utils.getChildObject(textArea, "TextMovePoint");
        textMovePoint.GetComponent<Text>().text = "移动力：" + u.movingPoint.ToString();

        GameObject textState = Utils.getChildObject(textArea, "TextState");
        textState.GetComponent<Text>().text = "状态：" + (u.fullState ? "战斗模式" : "移动模式");

        int attack = u.fullState ?  Counter.counter[u.counterID].attackInjure : Counter.counter[u.counterID].attack;
        int endAttack = u.strategyMove ? 0 : attack;   //战略模式时无法攻击
        GameObject textAttack = Utils.getChildObject(textArea, "TextAttack");
        textAttack.GetComponent<Text>().text = "进攻值：" + endAttack.ToString();

        GameObject textDefense = Utils.getChildObject(textArea, "TextDefense");
        textDefense.GetComponent<Text>().text = "防守值：" + u.remainDefense.ToString();

        string moveReady = u.finishTurnMoved ? "本回合已移动" : " 本回合未移动";
        GameObject textTurnMoved = Utils.getChildObject(textArea, "TextTurnMoved");
        textTurnMoved.GetComponent<Text>().text = moveReady;

        string attackReady = u.alreadyRaiseAttack ? "本回合已攻击" : " 本回合未攻击";
        GameObject TextTurnRasiedAtt = Utils.getChildObject(textArea, "TextTurnRasiedAtt");
        TextTurnRasiedAtt.GetComponent<Text>().text = attackReady;
        // 其他状态值 to do 
        if (u.remainDefense <= 0)
        {
            ArmyIconSelected[count].GetComponent<Image>().sprite = GetComponent<CounterImage>().getCounterPicture(-1);
            GameObject redRectBorder = Utils.getChildObject(ArmyIconSelected[count], "RedRectBorder");
            redRectBorder.GetComponent<RedRectBorder>().setSelectedState(false);
            textMovePoint.GetComponent<Text>().text = "移动力：";
            textAttack.GetComponent<Text>().text = "进攻值：";
            textDefense.GetComponent<Text>().text = "防守值：";
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

    public GameObject getArmyIconSelectedFromIndex(int k)   // 获得第k个ArmyIconSelected
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
