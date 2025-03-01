using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnsController : MonoBehaviour
{
    public static bool canCleanse = true;
    public static bool endTurn;

    public GameObject Fail;
    public GameObject Vectory;

    bool isEnter;

    public static void StartTurn()
    {
        GameObject[] pollutionTag = GameObject.FindGameObjectsWithTag("Pollution");
        GameObject[] houseTag = GameObject.FindGameObjectsWithTag("House");
        if (PollutionController.times > 0)
        {
            foreach (GameObject obj in pollutionTag)
            {
                obj.GetComponent<InteractiveObj>().Polluting();
            }
            PollutionController.times -= 1;
        }
        if (PollutionController.times == 0)
        {
            canCleanse = true;
            PollutionController.times = 1;
            PollutionController.turn += 1;
        }
    }

    private void Update()
    {
        if (isEnter && Input.GetMouseButtonDown(0))
        {
            EndTurn();
        }
    }

    public void EndTurn()
    {
        StartCoroutine(DelayTurn());
        StopCoroutine(DelayTurn());
    }

    IEnumerator DelayTurn()
    {
        endTurn = true;
        PollutionController.times = 100;

        while (PollutionController.times > 0)
        {
            GameObject[] pollutionTag = GameObject.FindGameObjectsWithTag("Pollution");
            foreach (GameObject obj in pollutionTag)
            {
                obj.GetComponent<InteractiveObj>().Polluting();
            }
            PollutionController.times -= 1;
            GameObject[] houseTag = GameObject.FindGameObjectsWithTag("House");
            if (houseTag.Length == 0)
            {
                Debug.Log("游戏失败");
                Fail.SetActive(true);
                endTurn = false;
                PollutionController.times = 1;
                PollutionController.turn = 0;
                canCleanse = true;
                yield break;
            }
            yield return null;
        }
        Debug.Log("游戏胜利");
        Vectory.SetActive(true);
        canCleanse = true;
        PollutionController.times = 1;
        PollutionController.turn = 0;
        endTurn = false;
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
