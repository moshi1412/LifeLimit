using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndClass : MonoBehaviour
{
    public int model;
    public GameObject help;
    public GameObject helpButton;
    public GameObject TrunsController;
    public GameObject BG;
    bool isEnter;

    private void Update()
    {
        if (isEnter && Input.GetMouseButtonDown(0))
        {
            Debug.Log(0);
            if (model == 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (model == 2)
            {
                Application.Quit();
            }
            else if (model == 3)
            {
                help.SetActive(true);
                isEnter = false;
                TrunsController.SetActive(false);
                BG.SetActive(false);
                helpButton.SetActive(false);
            }
            else if (model == 4)
            {
                helpButton.SetActive(true);
                TrunsController.SetActive(true);
                BG.SetActive(true);
                isEnter = false;
                help.SetActive(false);
            }
            else if (model == 5)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                int i = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(i + 1);
            }
        }
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
