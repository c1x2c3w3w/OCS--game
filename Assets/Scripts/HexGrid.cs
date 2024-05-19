using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public Vector2Int rowcolpos;        // 这个格子位于第几行第几列   rowcolpos.x 是行；rowcolpos.y 是列；
    public GameObject redBorderObject;      //红色边框物体
    public GameObject yellowBoarderObject;  //黄色边框物体
    bool showSelectedBorder;                //是否显示红边框
    bool showYellowSelectedBorder;          //是否展示“黄色边框”（代表格子选定为即将被攻击的敌军）
    MapHexElement mapHexElement;            //六角格子的地图数据
    // Start is called before the first frame update
    void Start()
    {
        showSelectedBorder = false;
        showYellowSelectedBorder = false;
        setRedBorderStatus(showSelectedBorder);
        setYellowBorderStatus(showYellowSelectedBorder);
        mapHexElement = new MapHexElement();
    }

    // Update is called once per frame
    public void setRedBorderStatus(bool status)
    {
        redBorderObject.SetActive(status);
    }
    public void setMapHexElement(MapHexElement mhe)
    {
        mapHexElement = mhe;
    }

    public void setYellowBorderStatus(bool status)
    {
        yellowBoarderObject.SetActive(status);
    }

    public void onSelected()
    {
        showSelectedBorder = true;
        setRedBorderStatus(showSelectedBorder); //显示红色外框
        Debug.Log("on selected");
    }

    public void onAttackGridSelected()    // 当被选定为 “被攻击格”时执行该函数，显示黄色外框
    {
        // 触发实现 显示黄色外框
        showYellowSelectedBorder = true;
        setYellowBorderStatus(showYellowSelectedBorder);
        Debug.Log("on attackselected");
    }
    public List<int> getHexArmy()
    {

        List<int> hexArmy = new List<int>();
        Vector2Int rowcol = rowcolpos;   // 要判断的格子的行列
        for (int i = 0; i < GameData.units.Length; i++)
        {
            Unit u = GameData.units[i].GetComponent<Unit>();
            if(GameData.units[i].activeSelf == true)
            {
                if (u.row == rowcol.x && u.col == rowcol.y)
                {
                    hexArmy.Add(i);   // 记录下第count个army的部队编号
                }
            }
            
        }
        return hexArmy;  
    }  
}
