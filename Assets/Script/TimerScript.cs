using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float MaxTime;
    public bool Tick;

    private Image myImage;
    private float currentTime;

    public AudioSource GameAudioSource;
    public AudioClip Clip;

    void Start()
    {
        myImage = GetComponent<Image>();
        currentTime = MaxTime;
    }

    private void Update()
    {
        TimerHandler();
    }

    private void TimerHandler()
    {
        Tick = false;
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            GameAudioSource.PlayOneShot(Clip);
            Tick = true;
            currentTime = MaxTime;
        }

        myImage.fillAmount = currentTime / MaxTime;
    }
}
