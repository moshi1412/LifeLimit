using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ChooseClass : MonoBehaviour
{
    public int sceneindex;
    // Start is called before the first frame update
    Button btn;
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SceneChange);
    }

    private void SceneChange()
    {
        SceneManager.LoadScene(sceneindex);
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
