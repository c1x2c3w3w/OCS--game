using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public Vector2Int rowcolpos;        // �������λ�ڵڼ��еڼ���   rowcolpos.x ���У�rowcolpos.y ���У�
    public GameObject redBorderObject;      //��ɫ�߿�����
    public GameObject yellowBoarderObject;  //��ɫ�߿�����
    bool showSelectedBorder;                //�Ƿ���ʾ��߿�
    bool showYellowSelectedBorder;          //�Ƿ�չʾ����ɫ�߿򡱣��������ѡ��Ϊ�����������ĵо���
    MapHexElement mapHexElement;            //���Ǹ��ӵĵ�ͼ����
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
        setRedBorderStatus(showSelectedBorder); //��ʾ��ɫ���
        Debug.Log("on selected");
    }

    public void onAttackGridSelected()    // ����ѡ��Ϊ ����������ʱִ�иú�������ʾ��ɫ���
    {
        // ����ʵ�� ��ʾ��ɫ���
        showYellowSelectedBorder = true;
        setYellowBorderStatus(showYellowSelectedBorder);
        Debug.Log("on attackselected");
    }
    public List<int> getHexArmy()
    {

        List<int> hexArmy = new List<int>();
        Vector2Int rowcol = rowcolpos;   // Ҫ�жϵĸ��ӵ�����
        for (int i = 0; i < GameData.units.Length; i++)
        {
            Unit u = GameData.units[i].GetComponent<Unit>();
            if(GameData.units[i].activeSelf == true)
            {
                if (u.row == rowcol.x && u.col == rowcol.y)
                {
                    hexArmy.Add(i);   // ��¼�µ�count��army�Ĳ��ӱ��
                }
            }
            
        }
        return hexArmy;  
    }  
}
