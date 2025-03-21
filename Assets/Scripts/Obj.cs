using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Obj : MonoBehaviour
{
    public Material LightMaterial;
    bool isShowTip;
    public int fontsize = 50;
    InteractiveObj ict;
    string[] terrain = { "平原", "草地", "丘陵", "山地", "林地", "低级居民区", "中级居民区", "高级居民区", "低级沙漠地带", "高级沙漠地带", "禁区" };
    public float[] normal = { 1, 2, 3, 5, 4, 3, 4, 5 };
    public int oriDegree;
    Renderer render;

    [Multiline]
    string text;

    private void Start()
    {
        ict = GetComponent<InteractiveObj>();
        oriDegree = PollutionController.instance.Tolerance[(int)ict.types];
        render = GetComponent<Renderer>();
        fontsize = 30;
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
        // Debug.Log("enter");
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
            Texture2D texture = new Texture2D(1, 1);
            Color color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
            texture.SetPixel(0, 0, color); // 设置纹理的颜色为灰色
            texture.Apply();
            // Debug.Log("show");
            GUIStyle style1 = new GUIStyle();
            style1.fontSize = fontsize;
            // Debug.Log(style1.fontSize);
            style1.normal.textColor = Color.white;
            style1.normal.background = texture; // 设置背景纹理
            style1.padding = new RectOffset(10, 10, 10, 10); // 设置内边距
            style1.alignment = TextAnchor.MiddleCenter; //
            // 计算文本框的位置
            Rect labelRect = new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 18*text.Length, 100);

            GUI.Label(labelRect, text, style1);
        }
    }
}