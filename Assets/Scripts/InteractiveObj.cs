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
    public bool canClean;
    static bool isPause = true;
    bool ischosen = false;
    public Material linemat;
    public Material lightmat;
    static bool isprocessed = false;
    [NonSerialized]
    public int isPolluting;
    //private bool isfirst = true;
    Vector2[] derivative = new Vector2[] { new Vector2(-0.5f, -0.5f), new Vector2(-0.5f, 0.5f), new Vector2(-0.5f, 1.5f), new Vector2(0.5f, -0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 1.5f), new Vector2(1.5f, -0.5f), new Vector2(1.5f, 0.5f), new Vector2(1.5f, 1.5f) };
    Vector2[] der = new Vector2[] { new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(-1, 0), new Vector2(1, -1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, -1), new Vector2(0, -1) };
    public List<GameObject> adjacent = new List<GameObject>();
    Renderer rend;

    private void Start()
    {
        //cpartical = PollutionController.instance.cpartical;
        //ppartical = PollutionController.instance.ppartical;
        //cpartical.type = types;
        //ppartical.isClean = false;
        //fogobj = Resources.Load<GameObject>("Fog");
        // for (int i = 0; i < 9; i++)
        // {
        //     adjacent[i] = null;
        // }
        originPol = pollution;
        rend = GetComponent<Renderer>();
        normal = PollutionController.instance.Tolerance[(int)types];
        heavy = normal + 5;
    }
    private void Update()
    {
        // if(!isfirst&&Input.GetMouseButtonDown(0)&&!isEnter)//如果点过一次并且这次点击是在方格外面点的，就让它下去（做成动画也可）
        // {    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);return;}
        if (isPause && ischosen)
        {
            if (adjacent == null)
            {
                InteractiveObj[] objects = transform.parent.GetComponentsInChildren<InteractiveObj>();

                foreach (InteractiveObj obj in objects)
                {
                    foreach (Vector2 pos in derivative)
                    {
                        if (obj.transform.position == transform.position + (Vector3)pos)
                        {
                            adjacent.Add(obj.gameObject);
                        }
                    }
                    if (adjacent.Capacity == 8)
                    {
                        break;
                    }
                }
            }
            foreach (GameObject obj in adjacent)
            {
                if (obj.GetComponent<InteractiveObj>().types == types)
                {
                    obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, transform.position.z + 0.1f);
                    obj.GetComponent<MeshRenderer>().material = linemat;
                }
            }
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
            rend.material = linemat;
            Color co = GetComponent<MeshRenderer>().material.color;
            co.a = 255f / 255f;
            rend.material.color = co;
            ischosen = false;
        }
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
                GameObject fogobj = Resources.Load<GameObject>("fog");
                GameObject fog_clone = Instantiate(fogobj, transform.position + new Vector3(0.5f, 0.5f, 0), transform.rotation);
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
                pollutingTime += 1;
                MaterialsChange(PollutionController.instance.mildPM);

            }
            pollution = Pollution.mild;
        }
        else
        {
            if (pollution != originPol && heavypollutingTime == 0)
            {
                heavypollutingTime += 1;
                MaterialsChange(PollutionController.instance.heavyPM);

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
            Debug.Log(isPause);
            if (isprocessed)
            {
                //Debug.Log("isprocessed");
                isPause = true; isprocessed = false;
            }
            if (isPause)
            {
                if (!ischosen)
                {
                    Debug.Log("ischosen");
                    isPause = false;
                    ischosen = true;
                    if (adjacent.Count == 0)
                    {
                        Debug.Log(transform.parent.childCount);

                        for (int i = 0; i < transform.parent.childCount; i++)
                        {
                            foreach (Vector2 pos in der)
                            {
                                Debug.Log(transform.position + (Vector3)pos);
                                if (transform.parent.GetChild(i).position == transform.position + (Vector3)pos)
                                {
                                    adjacent.Add(transform.parent.GetChild(i).gameObject);
                                }
                            }
                            if (adjacent.Count == 8)
                            {
                                break;
                            }
                        }
                    }
                    foreach (GameObject obj in adjacent)
                    {
                        if (obj.GetComponent<InteractiveObj>().types == types)
                {
                    obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, transform.position.z - 0.1f);
                    obj.GetComponent<MeshRenderer>().material = lightmat;
                }
                    }
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
                    rend.material = lightmat;
                    Color co = GetComponent<MeshRenderer>().material.color;
                    co.a = 255f / 255f;
                    rend.material.color = co;
                    return;

                }

            }
            else
            {

                if (!ischosen) { isPause = true; return; }
                if (adjacent == null)
                {
                    InteractiveObj[] objects = transform.parent.GetComponentsInChildren<InteractiveObj>();

                    foreach (InteractiveObj obj in objects)
                    {
                        foreach (Vector2 pos in derivative)
                        {
                            if (obj.transform.position == transform.position + (Vector3)pos)
                            {
                                adjacent.Add(obj.gameObject);
                            }
                        }
                        if (adjacent.Capacity == 8)
                        {
                            break;
                        }
                    }
                }
                foreach (GameObject obj in adjacent)
                {
                    if (obj.GetComponent<InteractiveObj>().types == types)
                {
                    obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, transform.position.z + 0.1f);
                    obj.GetComponent<MeshRenderer>().material = linemat;
                }
                }
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.1f);
                ischosen = false;
                rend.material = linemat;
                Color co = GetComponent<MeshRenderer>().material.color;
                co.a = 255f / 255f;
                rend.material.color = co;
            }
            Debug.Log(1);
            //Debug.Log(types);
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

    public void MaterialsChange(GameObject material)
    {
        Debug.Log("MaterialsChange");
        StartCoroutine(MatFade(material));
        StopCoroutine(MatFade(material));
    }

    IEnumerator MatFade(GameObject material)
    {
        Debug.Log(transform.position);
        GameObject obj = Instantiate(material, transform.position, new Quaternion(1, 0, 0, 0));

        // obj.GetComponent<InteractiveObj>().pollutingTime = pollutingTime;
        // obj.GetComponent<InteractiveObj>().heavypollutingTime = heavypollutingTime;
        // obj.GetComponent<InteractiveObj>().pollutionDegree = pollutionDegree;
        // obj.GetComponent<InteractiveObj>().canPolluting = canPolluting;
        // obj.GetComponent<InteractiveObj>().canClean = canClean;
        obj.transform.parent = transform.parent;
        Color co = obj.GetComponent<MeshRenderer>().material.color;
        co.a = 0f / 255f;
        Material mat = obj.GetComponent<Renderer>().material;
        mat.color = co;
        mat.DOFade(1, PollutionController.instance.time);
        yield return new WaitForSeconds(PollutionController.instance.time);
        // obj.transform.parent = transform;
        // yield return new WaitForSeconds(PollutionController.instance.time);
        // rend.material.DOFade(0, PollutionController.instance.time);

        // yield return new WaitForSeconds(PollutionController.instance.time);
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)//被净化、污染检测
    {
        // if (isEnter)
        //     if (isfirst) 
        //     {
        //         transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);
        //         isfirst=false;
        //         return;
        //     }

        //需要获取一下同一个组的块？

        //Debug.Log(types);
        if (isPause) return;
        //isPause=true;
        Debug.Log("trigger");
        Debug.Log(collision.gameObject.GetComponent<PCpartical>().isClean + "and" + canPolluting);
        if (collision.gameObject.GetComponent<PCpartical>().isClean == false && canPolluting)
        {
            Debug.Log("polluted");
            GetPollution();
            isprocessed = true;
        }
        else if (collision.gameObject.GetComponent<PCpartical>().type == this.types && canClean)
        {
            GetClense();
            isprocessed = true;
            Destroy(collision.gameObject);
        }
        // isfirst = true;
    }


    private void OnMouseEnter()
    {
        //Debug.Log("enter");
        isEnter = true;
    }

    private void OnMouseExit()
    {
        //Debug.Log("exit");
        isEnter = false;
    }

}
