using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Obj : MonoBehaviour
{
    bool isShowTip;
    public int fontsize = 30;
    InteractiveObj ict;
    string[] terrain = { "平原", "草地", "丘陵", "山地", "林地", "低级居民区", "中级居民区", "高级居民区", "低级沙漠地带", "高级沙漠地带", "禁区" };
    float[] normal = { 1, 2, 3, 5, 4, 3, 4, 5 };
    int oriDegree;

    [Multiline]
    string text;

    private void Start()
    {
        ict = GetComponent<InteractiveObj>();
        oriDegree = PollutionController.instance.Tolerance[(int)ict.types];
    }
    private void Update()
    {
        if ((int)ict.types < normal.Length)
        {
            text = "地形:" + terrain[(int)ict.types] + "\n" + "沙化度：" + ict.pollutionDegree + "/" + (normal[(int)ict.types]);
        }
        else if ((int)ict.types == normal.Length)
        {
            text = "地形:" + terrain[(int)ict.types] + "\n" + "沙化度：" + (ict.pollutionDegree - oriDegree) + "/" + 5;
        }
        else
        {
            text = "地形:" + terrain[(int)ict.types] + "\n" + "沙化度：" + "NAN";
        }
    }
    void OnMouseEnter()
    {
        isShowTip = true;
    }
    void OnMouseExit()
    {
        isShowTip = false;
    }
    void OnGUI()
    {
        if (isShowTip)
        {
            GUIStyle style1 = new GUIStyle();
            style1.fontSize = fontsize;
            style1.normal.textColor = Color.white;
            GUI.Label(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 400, 50), text, style1);
        }
    }
}