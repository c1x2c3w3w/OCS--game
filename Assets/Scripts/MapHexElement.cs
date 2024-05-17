using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHexElement
{
    public int terrain;
    public int construction;
    //0表示无河流，1表示有小河
    public int[] rivers;
    //0表示无道路，1表示有道路
    public int[] roads;
    // Start is called before the first frame update
    public MapHexElement()
    {
        // 以六边形正上方为第一方向边(下标位0)，顺时针分别为第二方向边(下标位1)...
        rivers = new int[6];
        roads = new int[6];
    }
}
