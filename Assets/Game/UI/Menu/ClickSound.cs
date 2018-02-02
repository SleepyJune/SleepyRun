using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class ClickSound : MonoBehaviour
{
    public enum ClickSounds
    {
        clickSound1, clickSound2, clickSound3
    };

    public ClickSounds clickSounds;

    UIAudioLibrary library;
    AudioSource source;

    void Start()
    {
        library = UIAudioLibrary.instance;
        source = library.uiAudioSource;

        GetComponent<Button>().onClick.AddListener(PlayAudioClip);
    }

    public void PlayAudioClip()
    {
        source.PlayOneShot(library.clickSounds[(int)clickSounds]);
    }
}
