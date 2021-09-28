using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmContral : MonoBehaviour
{
    public static AudioSource MapBGM;
    public static AudioSource BATTLEBGM;
    public static AudioClip[] BGM;
    public static bool firstLoad = true;
    // Start is called before the first frame update
    void Start()
    {
        if (!firstLoad) 
        {
            MapBGM.UnPause();
        }
        else
        {
            MapBGM = GameObject.Find("BGM").GetComponent<AudioSource>();
            DontDestroyOnLoad(MapBGM);
            MapBGM.Play();
        }
        firstLoad = false;
      
    }
    // Update is called once per frame
    void Update()
    {
        
    }
     void OnDisable()
    {

    }
}
