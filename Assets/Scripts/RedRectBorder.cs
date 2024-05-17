using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRectBorder : MonoBehaviour
{
    public bool isSelected;             // 是否被选定，也决定了是否展示“红色方形边框”
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
        this.transform.localScale = new Vector3(1f, 1f, 1f);   // 缩放比例全设为1，就是原大小
    }
    void hideRedRectBorder()
    {
        this.transform.localScale = new Vector3(0f, 0f, 0f);   // 缩放比例全设为0，就看不见了
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
        Debug.Log("里面的" + isSelected); 
    }
    public void ArmyClicked2()
    {

        setSelectedState2(!isSelected2);
        Debug.Log("里面的2" + isSelected2);
    }
}
