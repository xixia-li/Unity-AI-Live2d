using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Play_Audio : MonoBehaviour
{

    public AudioSource Audio;
    // Start is called before the first frame update
    void Start()
    {
        Audio =transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public  void Audio_Play(string url)
    {
        Debug.Log("一调用");
        AudioClip audioClip;

        AudioType audiotype = AudioType.MPEG;
        var WWW_Audio = UnityWebRequestMultimedia.GetAudioClip(url, audiotype);

        audioClip = DownloadHandlerAudioClip.GetContent(WWW_Audio);

        Audio.clip = audioClip;
        Audio.Play();
        Debug.Log(Audio.clip);

        
        
    }
}
