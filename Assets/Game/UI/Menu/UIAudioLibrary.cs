using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class UIAudioLibrary : MonoBehaviour
{
    public static UIAudioLibrary instance = null;

    public AudioClip[] clickSounds;

    public AudioSource uiAudioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);

            uiAudioSource = GetComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
}
