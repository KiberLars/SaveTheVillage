using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClip : MonoBehaviour
{
    public AudioSource GameAudioSource;
    public AudioClip clickClip;

    public void click()
    {
        GameAudioSource.PlayOneShot(clickClip);
    }
}
