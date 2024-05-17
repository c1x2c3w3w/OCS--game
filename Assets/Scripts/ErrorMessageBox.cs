using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessageBox : MonoBehaviour
{
    static int delayTime = 0;
    static GameObject emsgBox;
    static GameObject emsgBoxText;

    private void Awake()
    {
        emsgBox = GameObject.Find("ErrorMessageBox");           // 找出 错误消息框
        emsgBoxText = GameObject.Find("ErrorMessageBoxText");   // 找出 错误消息框的文本
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
        emsgBoxText.GetComponent<Text>().text = str;
        emsgBox.transform.localScale = new Vector3(1f, 1f, 1f);
        delayTime = 100;
    }
     
    public void Hide()
    {
        emsgBox.transform.localScale = new Vector3(0f, 0f, 0f);
    }
}
