using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class InteractiveObj : MonoBehaviour
{
   // public GameObject fobobj;
    public enum Pollution
    {
        clean = 0,
        normal = 1,
        mild = 2,
        heavy = 3
    }

    public enum Types
    {
        plane,
        grass,
        hilly,
        mountain,
        forest,
        lowhouse,
        middlehouse,
        highhouse,
        lowPollution,
        highPollution,
        clean
    }

    public Types types;

    public Pollution pollution;
    Pollution originPol;
    public float pollutionDegree = 0;

    [NonSerialized]
    public float normal;
    float heavy;

    float pollutingTime;
    float heavypollutingTime;

    public PCpartical cpartical;
    public PCpartical ppartical;

    bool isEnter;
    bool canPolluting;
    bool canClean;
    [NonSerialized]
    public int isPolluting;

    Vector2[] derivative = new Vector2[] { new Vector2(-0.5f, -0.5f), new Vector2(-0.5f, 0.5f), new Vector2(-0.5f, 1.5f), new Vector2(0.5f, -0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 1.5f), new Vector2(1.5f, -0.5f), new Vector2(1.5f, 0.5f), new Vector2(1.5f, 1.5f) };

    Renderer rend;

    private void Start()
    {
        //cpartical = PollutionController.instance.cpartical;
        //ppartical = PollutionController.instance.ppartical;
        //cpartical.type = types;
        //ppartical.isClean = false;
        //fogobj = Resources.Load<GameObject>("Fog");
        originPol = pollution;
        rend = GetComponent<Renderer>();
        normal = PollutionController.instance.Tolerance[(int)types];
        heavy = normal + 5;
    }
    private void Update()
    {
        PollutionDegreeJudgement();
        PollutionJudgement();
        TagSet();

        CleanseTest();
    }

    public void Polluting()//污染
    {
        if (pollution == Pollution.mild || pollution == Pollution.heavy)
        {
            foreach (Vector2 pos in derivative)
            {
                Instantiate(ppartical.gameObject, transform.position + (Vector3)pos, transform.rotation);
            }
        }
    }

    void GetPollution()//被污染
    {
        if (TurnsController.endTurn)
        {
            pollutionDegree += 1;
        }
        else
        {
            if (isPolluting == 0)
            {
                pollutionDegree += 1;
                isPolluting += 1;
            }
        }
    }

    void GetClense()//被净化
    {
        if (this.tag == "House")
        {
            pollutionDegree = 0;
        }
        else
        {
            pollution = Pollution.clean;
            types = Types.clean;
            if (pollution != originPol)
            {
                //Debug.Log("destroy");
                GameObject fogobj=Resources.Load<GameObject>("fog");
                GameObject fog_clone=Instantiate(fogobj, transform.position+new Vector3(0.5f,0.5f,0), transform.rotation);
                fog_clone.GetComponent<ParticleSystem>().Play();
                MaterialsChange(PollutionController.instance.cleanM);
                // fog_clone.GetComponent<ParticleSystem>().Stop();
                // Destroy(fog_clone);
            }
        }
    }

    void Cleanse()//净化
    {
        if (types == Types.clean)
        {
            foreach (Vector2 pos in derivative)
            {
                Instantiate(PollutionController.instance.clean, transform.position + (Vector3)pos, transform.rotation);
            }
        }
        else
        {
            if (types == Types.lowPollution)
            {
                foreach (Vector2 pos in derivative)
                {
                    Instantiate(PollutionController.instance.lowPollution, transform.position + (Vector3)pos, transform.rotation);
                }
            }
            else
            {
                foreach (Vector2 pos in derivative)
                {
                    Instantiate(cpartical.gameObject, transform.position + (Vector3)pos, transform.rotation);
                }
            }
        }
    }

    public void PollutionJudgement()//污染判断
    {
        if (pollution == Pollution.clean || pollution == Pollution.heavy)
        {
            canPolluting = false;
            canClean = false;
        }
        else
        {
            canPolluting = true;
            canClean = true;
        }
    }

    public void PollutionDegreeJudgement()//污染度判断
    {
        if (pollution == Pollution.clean)
        {
            return;
        }
        if (pollutionDegree < normal)
        {
            pollution = Pollution.normal;
        }
        else if (pollutionDegree >= normal && pollutionDegree < heavy)
        {
            if (pollution != originPol && pollutingTime == 0)
            {
                MaterialsChange(PollutionController.instance.mildPM);
                pollutingTime += 1;
            }
            pollution = Pollution.mild;
        }
        else
        {
            if (pollution != originPol && heavypollutingTime == 0)
            {
                MaterialsChange(PollutionController.instance.heavyPM);
                heavypollutingTime += 1;
            }
            pollution = Pollution.heavy;
        }
    }

    public void TagSet()
    {
        if (pollution == Pollution.mild)
        {
            this.tag = "Pollution";
            this.types = Types.lowPollution;
        }
        else if (pollution == Pollution.heavy)
        {
            this.tag = "Pollution";
            this.types = Types.highPollution;
        }
        else
        {
            if (types == Types.lowhouse || types == Types.middlehouse || types == Types.highhouse)
            {
                this.tag = "House";
            }
            //else
            //{
            //    this.tag = "Clean";
            //}
        }
    }

    public void CleanseTest()
    {
        if (TurnsController.canCleanse && canClean && Input.GetMouseButtonDown(0) && isEnter)
        {
            Debug.Log(1);
            Cleanse();
            TurnsController.canCleanse = false;
            TurnsController.StartTurn();
            InteractiveObj[] objs = transform.parent.GetComponentsInChildren<InteractiveObj>();
            foreach (InteractiveObj obj in objs)
            {
                obj.isPolluting = 0;
            }
        }
    }

    public void MaterialsChange(Material material)
    {
        StartCoroutine(MatFade(material));
        StopCoroutine(MatFade(material));
    }

    IEnumerator MatFade(Material material)
    {
        rend.material.DOFade(0, PollutionController.instance.time);
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        yield return new WaitForSeconds(PollutionController.instance.time);
        rend.material = material;
        Color co = GetComponent<MeshRenderer>().material.color;
        co.a = 0f / 255f;
        rend.material.color = co;
        rend.material.DOFade(1, PollutionController.instance.time);
    }

    private void OnTriggerEnter2D(Collider2D collision)//被净化、污染检测
    {
        if (collision.gameObject.GetComponent<PCpartical>().isClean == false && canPolluting)
        {
            GetPollution();
        }
        else if (collision.gameObject.GetComponent<PCpartical>().type == this.types && canClean)
        {
            GetClense();
        }
        Destroy(collision.gameObject);
    }

    private void OnMouseEnter()
    {
        isEnter = true;
    }

    private void OnMouseExit()
    {
        isEnter = false;
    }
}
