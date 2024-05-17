using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterImage : MonoBehaviour
{
    public Sprite[] counterPicture;  //算子的图片
                                     // 0: 美国 步兵    1：美国 炮兵
                                     // 2: 美国 坦克    3：美国 空军
                                     // 4: 中国 步兵    5：中国 炮兵
                                     // 6: 朝鲜 步兵    7：南韩 步兵
                                     // 8: 南韩 炮兵    9: 覆盖图片

    private void Awake()
    {
        counterPicture = new Sprite[10];
        counterPicture[0] = Resources.Load<Sprite>("images/units/AmericaArtyInfantry");
        counterPicture[1] = Resources.Load<Sprite>("images/units/AmericaArty");
        counterPicture[2] = Resources.Load<Sprite>("images/units/AmericaTank");
        counterPicture[3] = Resources.Load<Sprite>("images/units/AmericaFighter");
        counterPicture[4] = Resources.Load<Sprite>("images/units/ChinaInfantry");
        counterPicture[5] = Resources.Load<Sprite>("images/units/ChinaArty");
        counterPicture[6] = Resources.Load<Sprite>("images/units/NorthKoreaInfantry");
        counterPicture[7] = Resources.Load<Sprite>("images/units/SouthKoreaInfantry");
        counterPicture[8] = Resources.Load<Sprite>("images/units/SouthKoreaArty");
        counterPicture[9] = Resources.Load<Sprite>("images/units/ArmyTransparent"); // 透明空图
    }
    public Sprite getCounterPicture(int id)
    {
        int type = 0;
        switch (id)
        {
            case -1: type = 9; break;
            case 0:
            case 1: type = 6; break;
            case 2:
            case 3:
            case 4:
            case 5: type = 4; break;
            case 6:
            case 7: type = 5; break;
            case 8: type = 7; break;
            case 9:
            case 10: 
            case 15: type = 0; break;
            case 11:
            case 12: 
            case 16: type = 1; break;
            case 13:
            case 14: type = 2; break;
            case 17: 
            case 18: type = 3; break;
        }
        return counterPicture[type];
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
