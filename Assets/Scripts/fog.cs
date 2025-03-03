using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fog : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem particleSystem;
    private Button button;
    void Start()
    {
        button=GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    public void OnButtonClick()
    {
        particleSystem.Play();
    }
}
