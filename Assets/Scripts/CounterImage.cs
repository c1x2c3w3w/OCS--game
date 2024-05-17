using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterImage : MonoBehaviour
{
    public Sprite[] counterPicture;  //���ӵ�ͼƬ
                                     // 0: ���� ����    1������ �ڱ�
                                     // 2: ���� ̹��    3������ �վ�
                                     // 4: �й� ����    5���й� �ڱ�
                                     // 6: ���� ����    7���Ϻ� ����
                                     // 8: �Ϻ� �ڱ�    9: ����ͼƬ

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
        counterPicture[9] = Resources.Load<Sprite>("images/units/ArmyTransparent"); // ͸����ͼ
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
