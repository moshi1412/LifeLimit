using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioController : MonoBehaviour
{
    public static AudioController instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        GetComponent<AudioSource>().Play();
    }
    public void Update()
    {
        // Debug.Log(SceneManager.GetActiveScene().rootCount);
        if(SceneManager.GetActiveScene().buildIndex!= 0&& GetComponent<AudioSource>().clip.name!="inclass2")
        {
            GetComponent<AudioSource>().clip= Resources.Load<AudioClip>("effect/inclass2");
            GetComponent<AudioSource>().Play();
        }
    }
}