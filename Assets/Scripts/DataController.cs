using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DataController : MonoBehaviour
{
    public Material[] materials;
    Material material;
    static Renderer rend;
    float time = 1;

    private void Start()
    {
        material = materials[0];
        rend = GetComponent<Renderer>();
        StartCoroutine(MatFade());
    }

    private void Update()
    {
        //rend.material = materials[0];
        //GetComponent<Renderer>().material.DOFade(1, 1).SetLoops(-1, LoopType.Yoyo);
        //GetComponent<Renderer>().material.DOFade(0, 2);
        //GetComponent<Renderer>().material = materials[0];
    }

    //public void MaterialsChange(Material material, InteractiveObj obj)
    //{
    //    rend = obj.GetComponent<Renderer>();
    //    StartCoroutine(MatFade(material, obj));
    //}

    IEnumerator MatFade()
    {
        rend.material.DOFade(0, time);
        yield return new WaitForSeconds(time);
        rend.material = material;
        Color co = GetComponent<MeshRenderer>().material.color;
        co.a = 0f / 255f;
        rend.material.color = co;
        rend.material.DOFade(1, time);
    }
}
