using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Sprite TurnOnImage;
    public Sprite TurnOffImage;
    public Button VolumeButton;
    
    private bool volume = true;

    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = 1;
    }

    public void ChangeVolume()
    {
        if (volume)
        {
            VolumeButton.image.sprite = TurnOffImage;
            AudioListener.volume = 0;
        }
        else
        {
            VolumeButton.image.sprite = TurnOnImage;
            AudioListener.volume = 1;
        }
        volume = !volume;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
