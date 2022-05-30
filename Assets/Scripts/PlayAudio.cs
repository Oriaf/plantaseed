using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{ 
    private AudioSource AudioPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        AudioPlayer = GetComponent<AudioSource>();
        AudioPlayer.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void stopAudio()
    {
        AudioPlayer.Stop();
    }
    
    public void playClip(AudioClip sound)
    {
        AudioPlayer.PlayOneShot(sound);
    }
    
    public void stopMusic()
    {
            GameObject[] musicPlayer = GameObject.FindGameObjectsWithTag("Music");
            foreach (GameObject player in musicPlayer)
            {
                AudioSource source = player.GetComponent<AudioSource>();
                if(source != null) source.Stop();
            }
    }

    public void inverseMusic()
    {
            GameObject[] musicPlayer = GameObject.FindGameObjectsWithTag("Music");
            foreach (GameObject player in musicPlayer)
            {
                AudioSource source = player.GetComponent<AudioSource>();
                if (source != null)
                {
                    source.pitch = 0.5f;
                    source.Play();
                }
            }
    }
}
