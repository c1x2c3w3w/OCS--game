using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHexElement
{
    public int terrain;
    public int construction;
    //0��ʾ�޺�����1��ʾ��С��
    public int[] rivers;
    //0��ʾ�޵�·��1��ʾ�е�·
    public int[] roads;
    // Start is called before the first frame update
    public MapHexElement()
    {
        // �����������Ϸ�Ϊ��һ�����(�±�λ0)��˳ʱ��ֱ�Ϊ�ڶ������(�±�λ1)...
        rivers = new int[6];
        roads = new int[6];
    }
}
