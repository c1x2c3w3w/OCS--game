using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRectBorder : MonoBehaviour
{
    public bool isSelected;             // �Ƿ�ѡ����Ҳ�������Ƿ�չʾ����ɫ���α߿�
    public bool isSelected2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void showRedRectBorder()
    {
        this.transform.localScale = new Vector3(1f, 1f, 1f);   // ���ű���ȫ��Ϊ1������ԭ��С
    }
    void hideRedRectBorder()
    {
        this.transform.localScale = new Vector3(0f, 0f, 0f);   // ���ű���ȫ��Ϊ0���Ϳ�������
    }

    public void setSelectedState(bool flag)
    {
        isSelected = flag;
        if (isSelected) showRedRectBorder(); else hideRedRectBorder();
    }
    public void setSelectedState2(bool flag)
    {
        isSelected2 = flag;
        if (isSelected2) showRedRectBorder(); else hideRedRectBorder();
    }
    public void ArmyClicked()
    {
        
        setSelectedState(!isSelected);
        Debug.Log("�����" + isSelected); 
    }
    public void ArmyClicked2()
    {

        setSelectedState2(!isSelected2);
        Debug.Log("�����2" + isSelected2);
    }
}
