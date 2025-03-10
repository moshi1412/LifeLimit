using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionController : MonoBehaviour
{
    public static PollutionController instance { get; private set; }

    public GameObject cleanM;
    public GameObject mildPM;
    public GameObject heavyPM;


    public GameObject BG;

    public PCpartical clean;
    public PCpartical lowPollution;

    public int[] Tolerance;

    public void Awake()
    {
        instance = this;
    }

    public static int turn = 0;
    public static int times = 1;

    public float time = 1;
    //Renderer rend;
    //public void MaterialsChange(Material material, InteractiveObj obj)
    //{
    //    rend = obj.GetComponent<Renderer>();
    //    StartCoroutine(MatFade(material, obj));
    //}

    //IEnumerator MatFade(Material material, InteractiveObj obj)
    //{
    //    rend.material.DOFade(0, time);
    //    yield return new WaitForSeconds(time);
    //    rend.material = material;
    //    Color co = obj.GetComponent<MeshRenderer>().material.color;
    //    co.a = 0f / 255f;
    //    rend.material.color = co;
    //    rend.material.DOFade(1, time);
    //    Debug.Log(rend.material.color);
    //}
}
