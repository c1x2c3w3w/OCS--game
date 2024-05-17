using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    static int delayTime = 0;
    static GameObject msgBox;
    static GameObject msgBoxText;

    private void Awake()
    {
        msgBox = GameObject.Find("MessageBox");   // 找出 消息框
        msgBoxText = GameObject.Find("MessageBoxText");   // 找出 消息框的文本
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        if (delayTime > 0)
        {
            delayTime--;
            if (delayTime == 0) { Hide(); }
        }
    }


    public static void Show(string str)
    {
        msgBoxText.GetComponent<Text>().text = str;
        msgBox.transform.localScale = new Vector3(1f, 1f, 1f);
        delayTime = 80;
    }
     
    public static void Hide()
    {
        msgBox.transform.localScale = new Vector3(0f, 0f, 0f);
    }

}
