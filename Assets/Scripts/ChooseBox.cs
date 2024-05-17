using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseBox : MonoBehaviour
{
    static int delayTime = 0;
    static GameObject chooseBox;


    private void Awake()
    {
        chooseBox = GameObject.Find("ChooseBox");   // ÕÒ³ö ÏûÏ¢¿ò

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

    public static void Show()
    {
        chooseBox.transform.localScale = new Vector3(1f, 1f, 1f);
        delayTime = 80;
    }

    public static void Hide()
    {
        chooseBox.transform.localScale = new Vector3(0f, 0f, 0f);
    }

}
